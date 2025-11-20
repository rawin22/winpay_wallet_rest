using System;
using System.Collections.Generic;
using System.Linq; // Added for LINQ operations like Any()
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json; // Required for ReadFromJsonAsync and PostAsJsonAsync
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using GPWebApi.DTO; // Assuming DTOs are in this namespace
using Microsoft.Extensions.Configuration; // Added for IConfiguration

namespace Wallet.Helper
{
    public class CurrencySettingsHelper
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CurrencySettingsHelper> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _baseUrl;

        public CurrencySettingsHelper(HttpClient httpClient, IConfiguration configuration, ILogger<CurrencySettingsHelper> logger, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _baseUrl = _configuration["Win:Production:Url"]; // Assuming the URL is stored here
            if (string.IsNullOrEmpty(_baseUrl))
            {
                _logger.LogError("API base URL 'Win:Production:Url' is missing in configuration.");
                // Fallback or throw exception - Using provided fallback for now
                _baseUrl = "https://www.bizcurrency.com:20200/api/v1";
            }
        }

        private string GetBearerToken()
        {
            var token = _httpContextAccessor.HttpContext?.User?.FindFirst("Token")?.Value;
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Bearer token claim ('Token') not found for the user.");
            }
            return token;
        }


        public async Task<List<PaymentCountryData>> GetPaymentCountriesAsync()
        {
            var token = GetBearerToken();
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogError("Cannot fetch payment countries without a token.");
                return new List<PaymentCountryData>();
            }

            string url = $"{_baseUrl}/PaymentCountryList";
            _logger.LogInformation("Fetching payment countries from {Url}", url);

            HttpResponseMessage response = null; // Declare outside the try block

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await _httpClient.GetAsync(url); // Assign inside the try block

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Failed to fetch payment country list. Status Code: {StatusCode}, Response: {ResponseContent}",
                                      response.StatusCode, errorContent);
                    return new List<PaymentCountryData>();
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                _logger.LogTrace("PaymentCountryList API Raw Response: {JsonResponse}", jsonResponse);

                var countryResponse = JsonSerializer.Deserialize<PaymentCountryListGetResponse>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (countryResponse == null)
                {
                    _logger.LogWarning("Deserialization of PaymentCountryList response resulted in null.");
                    return new List<PaymentCountryData>();
                }

                // Check for problems reported by the API
                if (countryResponse.Problems != null && countryResponse.Problems.Any())
                {
                    _logger.LogWarning("PaymentCountryList API returned problems: {Problems}", JsonSerializer.Serialize(countryResponse.Problems));
                }

                var activeCountries = countryResponse.PaymentCountries?
                    .Where(c => c.IsEnabled && !c.IsBlocked)
                    .OrderBy(c => c.SortOrder)
                    .ThenBy(c => c.CountryName)
                    .ToList() ?? new List<PaymentCountryData>();

                _logger.LogInformation("Successfully fetched {Count} active payment countries.", activeCountries.Count);
                return activeCountries;

            }
            catch (JsonException jsonEx)
            {
                // Now 'response' is accessible here
                string rawResponse = response != null ? await response.Content.ReadAsStringAsync() : "N/A";
                _logger.LogError(jsonEx, "JSON Deserialization Error fetching payment countries from API. Raw response: {RawResponse}", rawResponse);
                return new List<PaymentCountryData>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching payment countries from API.");
                return new List<PaymentCountryData>();
            }
        }


        public async Task<List<CurrencyData>> GetAvailableCurrenciesAsync()
        {
            var token = GetBearerToken();
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogError("Cannot fetch currencies without a token.");
                return new List<CurrencyData>();
            }

            string url = $"{_baseUrl}/PaymentCurrencyList";
            _logger.LogInformation("Fetching available currencies from {Url}", url);

            HttpResponseMessage response = null; // Declare outside the try block

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await _httpClient.GetAsync(url); // Assign inside the try block

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Failed to fetch currency list. Status Code: {StatusCode}, Response: {ResponseContent}",
                                      response.StatusCode, errorContent);
                    return new List<CurrencyData>();
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                _logger.LogTrace("PaymentCurrencyList API Raw Response: {JsonResponse}", jsonResponse);

                var currencyResponse = JsonSerializer.Deserialize<PaymentCurrencyListGetResponse>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });


                if (currencyResponse == null)
                {
                    _logger.LogWarning("Deserialization of PaymentCurrencyList response resulted in null.");
                    return new List<CurrencyData>();
                }

                if (currencyResponse.Problems != null && currencyResponse.Problems.Any())
                {
                    _logger.LogWarning("PaymentCurrencyList API returned problems: {Problems}", JsonSerializer.Serialize(currencyResponse.Problems));
                }

                // Assuming CurrencyData has properties like CurrencyCode and CurrencyName
                // Ensure CurrencyData definition exists in GPWebApi.DTO
                var currencies = currencyResponse.Currencies?
                                   .OrderBy(c => c.CurrencyCode) // Assuming CurrencyData has CurrencyCode
                                   .ToList() ?? new List<CurrencyData>();

                _logger.LogInformation("Successfully fetched {Count} currencies.", currencies.Count);

                return currencies;
            }
            catch (JsonException jsonEx)
            {
                // Now 'response' is accessible here
                string rawResponse = response != null ? await response.Content.ReadAsStringAsync() : "N/A";
                _logger.LogError(jsonEx, "JSON Deserialization Error fetching currencies from API. Raw response: {RawResponse}", rawResponse);
                return new List<CurrencyData>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching currencies from API.");
                return new List<CurrencyData>();
            }
        }

        public async Task<List<string>> GetUserFavoriteCurrenciesAsync()
        {
            var token = GetBearerToken();
            if (string.IsNullOrEmpty(token)) return new List<string>();

            string url = $"{_baseUrl}/User/FavoriteCurrencies";
            _logger.LogInformation("Fetching favorite currencies from {Url}", url);

            HttpResponseMessage response = null; // Declare outside the try block

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await _httpClient.GetAsync(url); // Assign inside the try block

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Failed to fetch favorite currencies. Status Code: {StatusCode}, Response: {ResponseContent}",
                                      response.StatusCode, errorContent);
                    return new List<string>();
                }

                var result = await response.Content.ReadFromJsonAsync<List<string>>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                _logger.LogInformation("Successfully fetched {Count} favorite currencies.", result?.Count ?? 0);
                return result ?? new List<string>();
            }
            catch (JsonException jsonEx)
            {
                // Now 'response' is accessible here
                string rawResponse = response != null ? await response.Content.ReadAsStringAsync() : "N/A";
                _logger.LogError(jsonEx, "JSON Deserialization Error fetching favorite currencies from API. Raw response: {RawResponse}", rawResponse);
                return new List<string>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching favorite currencies: {Message}", ex.Message);
                return new List<string>();
            }
        }

        public async Task<bool> ToggleFavoriteCurrencyAsync(string currencyCode)
        {
            var token = GetBearerToken();
            if (string.IsNullOrEmpty(token)) return false;

            _logger.LogInformation("Toggling favorite status for currency code: {CurrencyCode}", currencyCode);

            HttpResponseMessage response = null; // Declare outside the try block

            try
            {
                var favorites = await GetUserFavoriteCurrenciesAsync();
                bool isFavorite = favorites.Contains(currencyCode);

                _logger.LogInformation("Currency {CurrencyCode} is currently favorite: {IsFavorite}", currencyCode, isFavorite);

                string url = isFavorite
                    ? $"{_baseUrl}/User/FavoriteCurrencies/{currencyCode}"
                    : $"{_baseUrl}/User/FavoriteCurrencies";

                // Assign inside the try block
                if (isFavorite)
                {
                    _logger.LogInformation("Sending DELETE request to {Url}", url);
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    response = await _httpClient.DeleteAsync(url);
                }
                else
                {
                    _logger.LogInformation("Sending POST request to {Url}", url);
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    response = await _httpClient.PostAsJsonAsync(url, new { currencyCode });
                }


                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Failed to toggle favorite currency {CurrencyCode}. Status Code: {StatusCode}, Response: {ResponseContent}",
                                     currencyCode, response.StatusCode, errorContent);
                    return false;
                }

                _logger.LogInformation("Successfully toggled favorite status for currency code: {CurrencyCode}", currencyCode);
                return true;
            }
            catch (JsonException jsonEx) // Catch specific exception if PostAsJsonAsync fails serialization
            {
                // 'response' might be null if PostAsJsonAsync failed before sending
                string rawResponse = response != null ? await response.Content.ReadAsStringAsync() : "N/A";
                _logger.LogError(jsonEx, "JSON Error toggling favorite currency {CurrencyCode}. Raw response: {RawResponse}", currencyCode, rawResponse);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling favorite currency {CurrencyCode}: {Message}", currencyCode, ex.Message);
                return false;
            }
        }
    }
}