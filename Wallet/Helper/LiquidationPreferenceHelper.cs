using GPWebApi.DTO;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Wallet.Helper
{
    public class LiquidationPreferenceHelper
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LiquidationPreferenceHelper> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public LiquidationPreferenceHelper(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<LiquidationPreferenceHelper> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            // Configure JSON options once
            _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        /// <summary>
        /// Gets the customer's liquidation preferences from the API.
        /// </summary>
        /// <param name="token">The authentication token.</param>
        /// <returns>A list of LiquidationPreferenceData or an empty list if an error occurs.</returns>
        public async Task<List<LiquidationPreferenceData>> GetPreferencesAsync(string token)
        {
            var preferences = new List<LiquidationPreferenceData>();
            string baseUrl = _configuration["Win:Production:Url"]; // Assuming the same config key structure
            string customerId = _httpContextAccessor.HttpContext?.User?.FindFirst("CustomerId")?.Value;

            if (string.IsNullOrEmpty(baseUrl))
            {
                _logger.LogError("API base URL ('Win:Production:Url') is missing in configuration.");
                return preferences; // Return empty list
            }
            if (string.IsNullOrEmpty(customerId))
            {
                _logger.LogError("Customer ID could not be retrieved from HttpContext.");
                return preferences; // Return empty list
            }

            // Construct the URL using the customerId
            string url = $"{baseUrl}/CustomerLiquidationPreference/{customerId}";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                _logger.LogInformation("Fetching liquidation preferences from {Url}", url);
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Failed to fetch liquidation preferences. Status Code: {StatusCode}, Response: {ResponseContent}",
                                     response.StatusCode, errorContent);
                    return preferences; // Return empty list on failure
                }

                var content = await response.Content.ReadAsStringAsync();
                var responseDto = JsonSerializer.Deserialize<CustomerLiquidationPreferenceGetAllResponse>(content, _jsonSerializerOptions);

                // Check the base DTO for problems (assuming DTOResponseBase has a 'Problems' property or similar)
                if (responseDto != null && responseDto.Problems == null) // Adapt this check based on DTOResponseBase actual structure
                {
                    preferences = responseDto.Preferences ?? new List<LiquidationPreferenceData>(); // Handle null Preferences list
                    _logger.LogInformation("Successfully fetched {Count} liquidation preferences.", preferences.Count);
                }
                else
                {
                    // Log problems if they exist in the response
                    _logger.LogError("API call successful, but response indicates problems: {Problems}", JsonSerializer.Serialize(responseDto?.Problems));
                }
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "Error deserializing liquidation preferences response from API.");
                // Optionally log response content here if needed for debugging
                return preferences; // Return empty list on exception
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching liquidation preferences from API at {Url}.", url);
                return preferences; // Return empty list on exception
            }

            return preferences;
        }

        /// <summary>
        /// Updates the customer's liquidation preferences via the API.
        /// </summary>
        /// <param name="token">The authentication token.</param>
        /// <param name="orderedCurrencyCodes">A list of currency codes in the desired liquidation order.</param>
        /// <returns>True if the update was successful, false otherwise.</returns>
        public async Task<bool> UpdatePreferencesAsync(string token, List<string> orderedCurrencyCodes)
        {
            string baseUrl = _configuration["Win:Production:Url"]; // Assuming the same config key structure
            string customerIdString = _httpContextAccessor.HttpContext?.User?.FindFirst("CustomerId")?.Value;

            if (string.IsNullOrEmpty(baseUrl))
            {
                _logger.LogError("API base URL ('Win:Production:Url') is missing in configuration.");
                return false;
            }
            if (string.IsNullOrEmpty(customerIdString))
            {
                _logger.LogError("Customer ID could not be retrieved from HttpContext.");
                return false;
            }
            if (!Guid.TryParse(customerIdString, out Guid customerId))
            {
                _logger.LogError("Failed to parse Customer ID '{CustomerIdString}' to Guid.", customerIdString);
                return false;
            }
            if (orderedCurrencyCodes == null) // Allow empty list to clear preferences if API supports it
            {
                _logger.LogWarning("UpdatePreferencesAsync called with null currency list.");
                orderedCurrencyCodes = new List<string>(); // Treat null as empty
            }


            string url = $"{baseUrl}/CustomerLiquidationPreference";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Prepare the request body
            var requestBody = new CustomerLiquidationPreferenceUpdateAllRequest
            {
                CustomerId = customerId,
                CurrencyList = string.Join(",", orderedCurrencyCodes) // Join the list into a comma-separated string
            };

            try
            {
                _logger.LogInformation("Updating liquidation preferences at {Url} with currencies: {Currencies}", url, requestBody.CurrencyList);

                // Serialize the request body to JSON
                var jsonRequestBody = JsonSerializer.Serialize(requestBody);
                var httpContent = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

                // Send PATCH request
                var response = await _httpClient.PatchAsync(url, httpContent);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Failed to update liquidation preferences. Status Code: {StatusCode}, Request: {RequestBody}, Response: {ResponseContent}",
                                     response.StatusCode, jsonRequestBody, errorContent);
                    return false; // Update failed
                }

                // Check response body for potential errors even on 200 OK
                var content = await response.Content.ReadAsStringAsync();
                // Avoid exceptions if content is empty which might be valid for PATCH success
                if (!string.IsNullOrWhiteSpace(content))
                {
                    try
                    {
                        var responseDto = JsonSerializer.Deserialize<CustomerLiquidationPreferenceUpdateAllResponse>(content, _jsonSerializerOptions);
                        if (responseDto?.Problems != null)
                        {
                            _logger.LogError("Liquidation preference update API call returned status {StatusCode}, but response indicates problems: {Problems}", response.StatusCode, JsonSerializer.Serialize(responseDto.Problems));
                            return false; // Indicate failure if problems are reported
                        }
                    }
                    catch (JsonException jsonEx)
                    {
                        _logger.LogWarning(jsonEx, "Could not deserialize successful PATCH response body. Assuming success based on status code. Body: {Content}", content);
                        // Continue as success based on status code, but log the issue.
                    }
                }

                _logger.LogInformation("Successfully updated liquidation preferences for customer {CustomerId}.", customerId);
                return true; // Update successful
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "Error serializing update request for liquidation preferences.");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating liquidation preferences via API at {Url}.", url);
                return false; // Update failed on exception
            }
        }
    }
}

