using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using GPWebApi.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Wallet.Helper
{
    public class AccountStatementHelper
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountStatementHelper> _logger;

        public AccountStatementHelper(HttpClient httpClient, IConfiguration configuration, ILogger<AccountStatementHelper> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<CustomerAccountStatementGetResponse> GetAccountStatementAsync(string token, Guid accountId, DateTime startDate, DateTime endDate)
        {
            var statementResponse = new CustomerAccountStatementGetResponse
            {
                AccountInfo = new CustomerAccountStatementData(),
                Entries = new List<CustomerAccountStatementEntryData>()
            };

            string mode = _configuration["Win:Mode"] ?? "Production";
            string baseUrl = _configuration[$"Win:{mode}:Url"];
            if (string.IsNullOrEmpty(baseUrl))
            {
                _logger.LogError("API base URL is missing.");
                return statementResponse;
            }

            // Format dates as required by the API (yyyy-MM-dd)
            string strStartDate = startDate.ToString("yyyy-MM-dd");
            string strEndDate = endDate.ToString("yyyy-MM-dd");

            // Construct the API URL with query parameters
            string url = $"{baseUrl}/CustomerAccountStatement?accountId={accountId}&strStartDate={strStartDate}&strEndDate={strEndDate}";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));

            try
            {
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Failed to fetch account statement. Status Code: {StatusCode}, Response: {ResponseContent}",
                                      response.StatusCode, errorContent);
                    return statementResponse; // Return default response on failure
                }

                var content = await response.Content.ReadAsStringAsync();
                statementResponse = JsonSerializer.Deserialize<CustomerAccountStatementGetResponse>(
                    content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                if (statementResponse == null)
                {
                    _logger.LogWarning("Deserialized account statement response is null.");
                    return new CustomerAccountStatementGetResponse
                    {
                        AccountInfo = new CustomerAccountStatementData(),
                        Entries = new List<CustomerAccountStatementEntryData>()
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching account statement from API for AccountId: {AccountId}", accountId);
            }

            return statementResponse;
        }
    }
}