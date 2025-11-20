using GPWebApi.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web; // For HttpUtility in search query builder (if added later)
using System.Globalization; // For CultureInfo (if search added later)


namespace Wallet.Helper
{
    /// <summary>
    /// Helper for FX‑Deal workflow: search -> quote -> book & instant-deposit.
    /// </summary>
    public class FxDealHelper
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _cfg;
        private readonly ILogger<FxDealHelper> _log;
        private readonly IHttpContextAccessor _ctx;
        private readonly JsonSerializerOptions _json = new() { PropertyNameCaseInsensitive = true };

        private string BaseUrl => _cfg["Win:Production:Url"] ?? string.Empty;

        public FxDealHelper(HttpClient httpClient,
                            IConfiguration configuration,
                            ILogger<FxDealHelper> logger,
                            IHttpContextAccessor ctx)
        {
            _httpClient = httpClient;
            _cfg = configuration;
            _log = logger;
            _ctx = ctx;
        }

        /* ─────────────────── SEARCH (Existing Basic Version) ──────────────────── */
        // Keeping the original simple search for now as requested earlier
        public async Task<List<FXDealSearchData>> GetFxDealsAsync(string token)
        {
            var list = new List<FXDealSearchData>();
            if (string.IsNullOrEmpty(BaseUrl)) return list;

            var url = $"{BaseUrl}/FXDeal/Search?PageIndex=0&PageSize=50"; // Hardcoded as per original
            _httpClient.DefaultRequestHeaders.Authorization = new("Bearer", token);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var resp = await _httpClient.GetAsync(url);
                if (!resp.IsSuccessStatusCode)
                {
                    _log.LogError("FX-deal search failed {Status}", resp.StatusCode);
                    return list; // Original simple error handling
                }

                // Original deserialization (consider ReadFromJsonAsync later)
                var responseString = await resp.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(responseString))
                {
                    _log.LogWarning("FX-deal search returned empty response body.");
                    return list;
                }

                var dto = JsonSerializer.Deserialize<FXDealSearchResponse>(responseString, _json);

                // Original check using internal HasErrors (less informative than checking Problems list directly)
                if (dto != null && !dto.HasErrors) // Assuming HasErrors exists via DTOResponseBase
                {
                    list = dto.FXDeals ?? new List<FXDealSearchData>();
                }
                else if (dto?.Problems != null)
                {
                    _log.LogWarning("FX-deal search returned problems: {Problems}", JsonSerializer.Serialize(dto.Problems));
                    // Still returns empty list on error as per original logic
                }
            }
            catch (JsonException jsonEx) { _log.LogError(jsonEx, "Error deserializing FX deals response"); }
            catch (Exception ex) { _log.LogError(ex, "Error fetching FX deals"); }
            return list;
        }

        /* ───────────────────────── CREATE QUOTE ─────────────────────────── */
        // No changes needed here based on request
        public async Task<(FXDealQuoteCreateResponse? resp, FXDealQuoteCreateRequest? payload)>
            CreateQuoteAsync(string token,
                             string buyCcy, string sellCcy, decimal amount,
                             string? amountCcy = null, string dealType = "SPOT")
        {
            var customerId = _ctx.HttpContext?.User.FindFirst("CustomerId")?.Value;
            if (string.IsNullOrEmpty(BaseUrl) || string.IsNullOrEmpty(customerId))
                return (null, null); // Maybe return response with Problem later

            if (!Guid.TryParse(customerId, out var customerGuid))
            {
                _log.LogError("Invalid CustomerId claim format: {CustomerId}", customerId);
                return (null, null);
            }


            var body = new FXDealQuoteCreateRequest
            {
                CustomerId = customerGuid,
                BuyCurrencyCode = buyCcy,
                SellCurrencyCode = sellCcy,
                Amount = amount,
                AmountCurrencyCode = amountCcy ?? sellCcy, // Default to Sell currency if not specified
                DealType = dealType,
                IsForCurrencyCalculator = false
            };

            var url = $"{BaseUrl}/FXDealQuote";
            _httpClient.DefaultRequestHeaders.Authorization = new("Bearer", token);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            FXDealQuoteCreateResponse? responseDto = null;

            try
            {
                var resp = await _httpClient.PostAsync(url, content);

                if (resp.Content?.Headers?.ContentLength > 0)
                {
                    try { responseDto = await resp.Content.ReadFromJsonAsync<FXDealQuoteCreateResponse>(_json); }
                    catch (JsonException jsonEx)
                    {
                        _log.LogError(jsonEx, "JSON error deserializing create quote response from {Url}, Status: {StatusCode}.", url, resp.StatusCode);
                        return (new FXDealQuoteCreateResponse { Problems = new List<Problem> { new Problem(9001, ProblemType.Error, "Response deserialization failed.", jsonEx.Message) } }, body);
                    }
                }

                if (!resp.IsSuccessStatusCode)
                {
                    var errorContent = await resp.Content?.ReadAsStringAsync() ?? "No Content";
                    _log.LogError("Quote creation failed {Status}. Response: {ErrorContent}", resp.StatusCode, errorContent);
                    // Return potential response DTO (which might contain Problems) and original payload
                    return (responseDto ?? new FXDealQuoteCreateResponse { Problems = new List<Problem> { new Problem((int)resp.StatusCode, ProblemType.Error, "HTTP request failed.", errorContent) } }, body);
                }

                if (responseDto?.Problems != null) { _log.LogWarning("Create Quote successful but API reported problems: {Problems}", JsonSerializer.Serialize(responseDto.Problems)); }
                if (responseDto?.Quote == null) { _log.LogWarning("Create Quote successful but Quote object was null."); }

                return (responseDto, body); // Return response DTO and original request payload
            }
            catch (HttpRequestException httpEx) { _log.LogError(httpEx, "HTTP error creating FX quote"); return (new FXDealQuoteCreateResponse { Problems = new List<Problem> { new Problem(9002, ProblemType.Error, "HTTP connection error.", httpEx.Message) } }, body); }
            catch (Exception ex) { _log.LogError(ex, "Error creating FX quote"); return (new FXDealQuoteCreateResponse { Problems = new List<Problem> { new Problem(9999, ProblemType.Error, "Unexpected system error.", ex.Message) } }, body); }
        }


        /* ─────── NEW: BOOK QUOTE & INSTANT DEPOSIT (PATCH …/BookAndInstantDeposit) ──────── */
        public async Task<FXDealQuoteBookAndInstantDepositResponse?> BookAndInstantDepositAsync(string token, Guid quoteId)
        {
            if (string.IsNullOrEmpty(BaseUrl))
            {
                _log.LogError("BookAndInstantDepositAsync: API base URL missing.");
                return new FXDealQuoteBookAndInstantDepositResponse { Problems = new List<Problem> { new Problem(9000, ProblemType.Error, "Configuration Error", "API Base URL is not configured.") } };
            }
            if (quoteId == Guid.Empty)
            {
                _log.LogWarning("BookAndInstantDepositAsync: quoteId was empty.");
                return new FXDealQuoteBookAndInstantDepositResponse { Problems = new List<Problem> { new Problem(9001, ProblemType.Error, "Request Error", "Quote ID cannot be empty.") } };
            }
            if (string.IsNullOrEmpty(token))
            {
                _log.LogWarning("BookAndInstantDepositAsync: Auth token was null or empty.");
                return new FXDealQuoteBookAndInstantDepositResponse { Problems = new List<Problem> { new Problem(9002, ProblemType.Error, "Authentication Error", "Auth token is missing.") } };
            }


            var url = $"{BaseUrl}/FXDealQuote/{quoteId}/BookAndInstantDeposit";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Although DTO is defined, API might not need body for this PATCH, TBC
            // If Body IS needed:
            //var requestBody = new FXDealQuoteBookAndInstantDepositRequest { QuoteId = quoteId };
            //var requestMessage = new HttpRequestMessage(HttpMethod.Patch, url)
            //{ Content = JsonContent.Create(requestBody, mediaType: null, _json) };
            // If Body is NOT needed:
            var requestMessage = new HttpRequestMessage(HttpMethod.Patch, url); // No body

            FXDealQuoteBookAndInstantDepositResponse? responseDto = null;

            try
            {
                _log.LogInformation("Booking quote and creating instant deposit via PATCH to {Url}", url);
                var response = await _httpClient.SendAsync(requestMessage);

                if (response.Content?.Headers?.ContentLength > 0)
                {
                    try { responseDto = await response.Content.ReadFromJsonAsync<FXDealQuoteBookAndInstantDepositResponse>(_json); }
                    catch (JsonException jsonEx)
                    {
                        _log.LogError(jsonEx, "JSON error deserializing book & deposit response from {Url}, Status: {StatusCode}.", url, response.StatusCode);
                        return new FXDealQuoteBookAndInstantDepositResponse { Problems = new List<Problem> { new Problem(9003, ProblemType.Error, "Response deserialization failed.", jsonEx.Message) } };
                    }
                }

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content?.ReadAsStringAsync() ?? "No Content";
                    _log.LogError("Failed to book quote & deposit. Status: {StatusCode}, URL: {Url}, Response: {ResponseContent}", response.StatusCode, url, errorContent);
                    return responseDto ?? new FXDealQuoteBookAndInstantDepositResponse { Problems = new List<Problem> { new Problem((int)response.StatusCode, ProblemType.Error, "HTTP request failed.", errorContent) } };
                }

                if (responseDto?.Problems != null) { _log.LogWarning("Book & Deposit successful (Status {StatusCode}), but API reported problems: {Problems}", response.StatusCode, JsonSerializer.Serialize(responseDto.Problems)); }
                else if (responseDto?.FXDepositData == null) { _log.LogWarning("Book & Deposit successful but FXDepositData object was null."); }
                else { _log.LogInformation("Successfully booked & deposited. Deal Ref: {DealRef}, Deposit Ref: {DepositRef}", responseDto.FXDepositData.FXDealReference, responseDto.FXDepositData.DepositReference); }

                return responseDto;
            }
            catch (HttpRequestException httpEx) { _log.LogError(httpEx, "HTTP error during book & deposit at {Url}.", url); return new FXDealQuoteBookAndInstantDepositResponse { Problems = new List<Problem> { new Problem(9004, ProblemType.Error, "HTTP connection error.", httpEx.Message) } }; }
            catch (Exception ex) { _log.LogError(ex, "Unexpected error during book & deposit at {Url}.", url); return new FXDealQuoteBookAndInstantDepositResponse { Problems = new List<Problem> { new Problem(9999, ProblemType.Error, "Unexpected system error.", ex.Message) } }; }
        }


        /* ───────────────── OLD METHODS REMOVED ────────────────── */
        // BookQuoteAsync method REMOVED
        // InstantDepositAsync method REMOVED

    }
}