// BalanceHelper.cs - Helper class to fetch balance data
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using GPWebApi.DTO;
using Wallet.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace Wallet.Helper
{
    public class BalanceHelper
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<BalanceHelper> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BalanceHelper(HttpClient httpClient, IConfiguration configuration, ILogger<BalanceHelper> logger, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<CustomerBalanceData>> GetBalancesAsync(string token)
        {
            var balances = new List<CustomerBalanceData>();
            string mode = _configuration["Win:Mode"] ?? "Production";
            string baseUrl = _configuration[$"Win:{mode}:Url"];
            string customerId = _httpContextAccessor.HttpContext?.User?.FindFirst("CustomerId")?.Value;

            if (string.IsNullOrEmpty(baseUrl) || string.IsNullOrEmpty(customerId))
            {
                _logger.LogError("API base URL or Customer ID is missing.");
                return balances;
            }

            string url = $"{baseUrl}/CustomerAccountBalance/{customerId}";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Failed to fetch balances. Status Code: {StatusCode}, Response: {ResponseContent}",
                                      response.StatusCode, errorContent);
                    return balances; // Return empty list on failure
                }

                var content = await response.Content.ReadAsStringAsync();
                var balanceResponse = JsonSerializer.Deserialize<CustomerAccountBalanceGetResponse>(
                    content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                if (balanceResponse != null)
                {
                    balances = balanceResponse.Balances;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching balances from API.");
            }

            return balances;
        }
    }
}
