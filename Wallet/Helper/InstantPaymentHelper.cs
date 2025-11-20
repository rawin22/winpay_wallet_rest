using GPWebApi.DTO; // Contains all the InstantPayment DTOs including Problem, ProblemType
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
using System.Web; // Required for HttpUtility

namespace Wallet.Helper
{
    /// <summary>
    /// Helper class for interacting with the Instant Payment API endpoints.
    /// </summary>
    public class InstantPaymentHelper
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _cfg;
        private readonly ILogger<InstantPaymentHelper> _log;
        private readonly IHttpContextAccessor _ctx;
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };
        private string BaseUrl => _cfg["Win:Production:Url"] ?? string.Empty;

        public InstantPaymentHelper(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<InstantPaymentHelper> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _cfg = configuration;
            _log = logger;
            _ctx = httpContextAccessor;
        }

        // --- SEARCH PAYMENTS ---
        public async Task<InstantPaymentSearchResponse?> SearchPaymentsAsync(string token, InstantPaymentSearchRequest searchRequest)
        {
            if (string.IsNullOrEmpty(BaseUrl)) { _log.LogError("SearchPaymentsAsync: API base URL missing."); return null; }
            if (searchRequest == null) { _log.LogWarning("SearchPaymentsAsync: searchRequest parameter was null."); return null; }

            var queryString = BuildSearchQueryString(searchRequest);
            var url = $"{BaseUrl}/InstantPayment/Search{queryString}";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            InstantPaymentSearchResponse? responseDto = null; // Define outside try block

            try
            {
                _log.LogInformation("Searching instant payments using URL: {Url}", url);
                var response = await _httpClient.GetAsync(url);

                if (response.Content?.Headers?.ContentLength > 0)
                {
                    try { responseDto = await response.Content.ReadFromJsonAsync<InstantPaymentSearchResponse>(_jsonOptions); }
                    catch (JsonException jsonEx)
                    {
                        _log.LogError(jsonEx, "JSON error deserializing instant payment search response from {Url}, Status: {StatusCode}.", url, response.StatusCode);
                        // *** CORRECTED: Use Problem constructor ***
                        return new InstantPaymentSearchResponse { Problems = new List<Problem> { new Problem(9001, ProblemType.Error, "Response deserialization failed.", jsonEx.Message) } };
                    }
                }

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content?.ReadAsStringAsync() ?? "No content";
                    _log.LogError("Failed to search instant payments. Status Code: {StatusCode}, URL: {Url}, Response: {ResponseContent}", response.StatusCode, url, errorContent);
                    // *** CORRECTED: Use Problem constructor, use HTTP status as code ***
                    return responseDto ?? new InstantPaymentSearchResponse { Problems = new List<Problem> { new Problem((int)response.StatusCode, ProblemType.Error, "HTTP request failed.", errorContent) } };
                }

                if (responseDto?.Problems != null) { _log.LogWarning("Search successful (Status {StatusCode}), but API reported problems: {Problems}", response.StatusCode, JsonSerializer.Serialize(responseDto.Problems)); }
                if (responseDto?.Records == null) { _log.LogWarning("Search successful but 'Records' object was null."); }
                else if (responseDto.Records.Payments == null) { _log.LogWarning("Search successful but 'Records.Payments' list was null."); }

                return responseDto;
            }
            // *** CORRECTED: Use Problem constructor in catch blocks ***
            catch (HttpRequestException httpEx) { _log.LogError(httpEx, "HTTP error searching instant payments at {Url}.", url); return new InstantPaymentSearchResponse { Problems = new List<Problem> { new Problem(9002, ProblemType.Error, "HTTP connection error.", httpEx.Message) } }; }
            catch (JsonException jsonEx) { _log.LogError(jsonEx, "General JSON error during search processing at {Url}.", url); return new InstantPaymentSearchResponse { Problems = new List<Problem> { new Problem(9003, ProblemType.Error, "JSON processing error.", jsonEx.Message) } }; }
            catch (Exception ex) { _log.LogError(ex, "Unexpected error searching instant payments at {Url}.", url); return new InstantPaymentSearchResponse { Problems = new List<Problem> { new Problem(9999, ProblemType.Error, "Unexpected system error.", ex.Message) } }; }
        }

        private string BuildSearchQueryString(InstantPaymentSearchRequest request)
        {
            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            queryParams["PageIndex"] = request.PageIndex.ToString();
            if (request.PageSize <= 0) queryParams["PageSize"] = "25"; else queryParams["PageSize"] = request.PageSize.ToString();
            if (!string.IsNullOrEmpty(request.PaymentReference)) queryParams["PaymentReference"] = request.PaymentReference;
            if (request.Status != InstantPaymentStatus.All) queryParams["Status"] = ((int)request.Status).ToString();
            if (!string.IsNullOrEmpty(request.FromCustomerAlias)) queryParams["FromCustomerAlias"] = request.FromCustomerAlias;
            if (!string.IsNullOrEmpty(request.ToCustomerAlias)) queryParams["ToCustomerAlias"] = request.ToCustomerAlias;
            if (!string.IsNullOrEmpty(request.FromCustomerName)) queryParams["FromCustomerName"] = request.FromCustomerName;
            if (!string.IsNullOrEmpty(request.ToCustomerName)) queryParams["ToCustomerName"] = request.ToCustomerName;
            if (request.AmountMin != decimal.Zero) queryParams["AmountMin"] = request.AmountMin.ToString(System.Globalization.CultureInfo.InvariantCulture);
            if (request.AmountMax != decimal.Zero) queryParams["AmountMax"] = request.AmountMax.ToString(System.Globalization.CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(request.CurrencyCode)) queryParams["CurrencyCode"] = request.CurrencyCode;
            if (!string.IsNullOrEmpty(request.ValueDateMin)) queryParams["ValueDateMin"] = request.ValueDateMin;
            if (!string.IsNullOrEmpty(request.ValueDateMax)) queryParams["ValueDateMax"] = request.ValueDateMax;
            if (request.SortBy != InstantPaymentSearchSortBy.None) queryParams["SortBy"] = ((int)request.SortBy).ToString();
            if (request.SortBy != InstantPaymentSearchSortBy.None && request.SortDirection != InstantPaymentSortDirection.Descending) queryParams["SortDirection"] = ((int)request.SortDirection).ToString();
            string queryString = queryParams.ToString();
            return string.IsNullOrEmpty(queryString) ? string.Empty : $"?{queryString}";
        }

        // --- CREATE PAYMENT ---
        // Assuming InstantPaymentCreateResponse, InstantPaymentCreateRequest DTOs exist
        public async Task<InstantPaymentCreateResponse?> CreatePaymentAsync(string token, InstantPaymentCreateRequest createRequest)
        {
            if (string.IsNullOrEmpty(BaseUrl)) { _log.LogError("CreatePaymentAsync: API base URL missing."); return null; }
            if (createRequest == null) { _log.LogWarning("CreatePaymentAsync: createRequest parameter was null."); return null; }

            var url = $"{BaseUrl}/InstantPayment";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            InstantPaymentCreateResponse? responseDto = null;

            try
            {
                _log.LogInformation("Creating instant payment from {FromCustomer} to {ToCustomer} for {Amount} {Currency}", createRequest.FromCustomer, createRequest.ToCustomer, createRequest.Amount, createRequest.CurrencyCode);
                var response = await _httpClient.PostAsJsonAsync(url, createRequest, _jsonOptions);

                if (response.Content?.Headers?.ContentLength > 0)
                {
                    try { responseDto = await response.Content.ReadFromJsonAsync<InstantPaymentCreateResponse>(_jsonOptions); }
                    catch (JsonException jsonEx)
                    {
                        _log.LogError(jsonEx, "JSON error deserializing create payment response from {Url}, Status: {StatusCode}.", url, response.StatusCode);
                        // *** CORRECTED: Use Problem constructor ***
                        return new InstantPaymentCreateResponse { Problems = new List<Problem> { new Problem(9001, ProblemType.Error, "Response deserialization failed.", jsonEx.Message) } };
                    }
                }

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content?.ReadAsStringAsync() ?? "No Content";
                    _log.LogError("Failed to create instant payment. Status: {StatusCode}, URL: {Url}, Request: {RequestBody}, Response: {ResponseContent}", response.StatusCode, url, JsonSerializer.Serialize(createRequest), errorContent);
                    // *** CORRECTED: Use Problem constructor ***
                    return responseDto ?? new InstantPaymentCreateResponse { Problems = new List<Problem> { new Problem((int)response.StatusCode, ProblemType.Error, "HTTP request failed.", errorContent) } };
                }

                if (responseDto?.Problems != null) { _log.LogWarning("Create successful (Status {StatusCode}), but API reported problems: {Problems}", response.StatusCode, JsonSerializer.Serialize(responseDto.Problems)); }
                else if (responseDto?.Payment == null) { _log.LogWarning("Create successful but 'Payment' object was null."); }
                else { _log.LogInformation("Successfully created payment ID: {PaymentId}, Reference: {Reference}", responseDto.Payment.PaymentId, responseDto.Payment.PaymentReference); }
                return responseDto;
            }
            // *** CORRECTED: Use Problem constructor in catch blocks ***
            catch (HttpRequestException httpEx) { _log.LogError(httpEx, "HTTP error creating instant payment at {Url}.", url); return new InstantPaymentCreateResponse { Problems = new List<Problem> { new Problem(9002, ProblemType.Error, "HTTP connection error.", httpEx.Message) } }; }
            catch (JsonException jsonEx) { _log.LogError(jsonEx, "JSON error during create payment processing at {Url}.", url); return new InstantPaymentCreateResponse { Problems = new List<Problem> { new Problem(9003, ProblemType.Error, "JSON processing error.", jsonEx.Message) } }; }
            catch (Exception ex) { _log.LogError(ex, "Unexpected error creating instant payment at {Url}.", url); return new InstantPaymentCreateResponse { Problems = new List<Problem> { new Problem(9999, ProblemType.Error, "Unexpected system error.", ex.Message) } }; }
        }

        // --- GET PAYMENT BY ID ---
        // Assuming InstantPaymentGetResponse, InstantPaymentGetData DTOs exist
        public async Task<InstantPaymentGetData?> GetPaymentAsync(string token, Guid paymentId)
        {
            // This method wasn't returning the full response, so no catch block changes needed here unless we change its return type too.
            // Keeping original return type for now.
            if (string.IsNullOrEmpty(BaseUrl)) { _log.LogError("GetPaymentAsync: API base URL missing."); return null; }
            if (paymentId == Guid.Empty) { _log.LogWarning("GetPaymentAsync: paymentId was Guid.Empty."); return null; }

            var url = $"{BaseUrl}/InstantPayment/{paymentId}";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                _log.LogInformation("Getting instant payment details for ID: {PaymentId}", paymentId);
                var responseDto = await _httpClient.GetFromJsonAsync<InstantPaymentGetResponse>(url, _jsonOptions);

                if (responseDto?.Problems != null) { _log.LogWarning("Get successful for ID {PaymentId}, but API reported problems: {Problems}", paymentId, JsonSerializer.Serialize(responseDto.Problems)); }
                if (responseDto?.Payment == null) { _log.LogWarning("Get successful for ID {PaymentId} but 'Payment' object was null.", paymentId); }
                return responseDto?.Payment;
            }
            catch (HttpRequestException httpEx) when (httpEx.StatusCode == System.Net.HttpStatusCode.NotFound) { _log.LogWarning("Instant payment with ID {PaymentId} not found (404) at {Url}.", paymentId, url); return null; }
            catch (HttpRequestException httpEx) { _log.LogError(httpEx, "HTTP error getting instant payment {PaymentId} at {Url}.", paymentId, url); return null; }
            catch (JsonException jsonEx) { _log.LogError(jsonEx, "Error deserializing get instant payment response for {PaymentId} from {Url}.", paymentId, url); return null; }
            catch (Exception ex) { _log.LogError(ex, "Unexpected error getting instant payment {PaymentId} at {Url}.", paymentId, url); return null; }
        }


        // --- POST (CONFIRM) PAYMENT ---
        // Assuming InstantPaymentPostResponse, InstantPaymentPostRequest DTOs exist
        public async Task<InstantPaymentPostResponse?> PostPaymentAsync(string token, Guid paymentId, string timestamp)
        {
            if (string.IsNullOrEmpty(BaseUrl)) { _log.LogError("PostPaymentAsync: API base URL missing."); return null; }
            if (paymentId == Guid.Empty) { _log.LogWarning("PostPaymentAsync: paymentId was Guid.Empty."); return null; }
            if (string.IsNullOrEmpty(timestamp)) { _log.LogWarning("PostPaymentAsync: timestamp was null or empty for Payment ID {PaymentId}.", paymentId); return null; }

            var url = $"{BaseUrl}/InstantPayment/Post";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var requestBody = new InstantPaymentPostRequest { InstantPaymentId = paymentId, Timestamp = timestamp };
            InstantPaymentPostResponse? responseDto = null;

            try
            {
                _log.LogInformation("Posting instant payment with ID: {PaymentId} using PATCH to {Url}", paymentId, url);
                var requestMessage = new HttpRequestMessage(HttpMethod.Patch, url)
                { Content = JsonContent.Create(requestBody, mediaType: null, _jsonOptions) };
                var response = await _httpClient.SendAsync(requestMessage);

                if (response.Content?.Headers?.ContentLength > 0)
                {
                    try { responseDto = await response.Content.ReadFromJsonAsync<InstantPaymentPostResponse>(_jsonOptions); }
                    catch (JsonException jsonEx)
                    {
                        _log.LogError(jsonEx, "JSON error deserializing post payment response from {Url}, Status: {StatusCode}.", url, response.StatusCode);
                        // *** CORRECTED: Use Problem constructor ***
                        return new InstantPaymentPostResponse { Problems = new List<Problem> { new Problem(9001, ProblemType.Error, "Response deserialization failed.", jsonEx.Message) } };
                    }
                }

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content?.ReadAsStringAsync() ?? "No Content";
                    _log.LogError("Failed to post payment {PaymentId}. Status: {StatusCode}, URL: {Url}, Request: {RequestBody}, Response: {ResponseContent}",
                                     paymentId, response.StatusCode, url, JsonSerializer.Serialize(requestBody), errorContent);
                    // *** CORRECTED: Use Problem constructor ***
                    return responseDto ?? new InstantPaymentPostResponse { Problems = new List<Problem> { new Problem((int)response.StatusCode, ProblemType.Error, "HTTP request failed.", errorContent) } };
                }

                if (responseDto?.Problems != null) { _log.LogWarning("Post successful (Status {StatusCode}) for ID {PaymentId}, but API reported problems: {Problems}", response.StatusCode, paymentId, JsonSerializer.Serialize(responseDto.Problems)); }
                else if (responseDto?.Payment == null) { _log.LogWarning("Post successful for ID {PaymentId} but 'Payment' object was null.", paymentId); }
                else { _log.LogInformation("Successfully posted payment ID {PaymentId}, resulting reference: {Reference}", responseDto.Payment.PaymentId, responseDto.Payment.PaymentReference); }

                return responseDto;
            }
            // *** CORRECTED: Use Problem constructor in catch blocks ***
            catch (HttpRequestException httpEx) { _log.LogError(httpEx, "HTTP error posting instant payment {PaymentId} at {Url}.", paymentId, url); return new InstantPaymentPostResponse { Problems = new List<Problem> { new Problem(9002, ProblemType.Error, "HTTP connection error.", httpEx.Message) } }; }
            catch (JsonException jsonEx) { _log.LogError(jsonEx, "JSON error during post payment processing for {PaymentId} at {Url}.", paymentId, url); return new InstantPaymentPostResponse { Problems = new List<Problem> { new Problem(9003, ProblemType.Error, "JSON processing error.", jsonEx.Message) } }; }
            catch (Exception ex) { _log.LogError(ex, "Unexpected error posting instant payment {PaymentId} at {Url}.", paymentId, url); return new InstantPaymentPostResponse { Problems = new List<Problem> { new Problem(9999, ProblemType.Error, "Unexpected system error.", ex.Message) } }; }
        }
    }
}