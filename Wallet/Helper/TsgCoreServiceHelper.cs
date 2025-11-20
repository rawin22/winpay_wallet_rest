using GPWebApi.DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Wallet.Interfaces;
using Wallet.Models;

namespace Wallet.Helper
{
    public class TsgCoreServiceHelper : ITsgCoreServiceHelper
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TsgCoreServiceHelper> _logger;
        private readonly string _baseUrl;
        private AuthenticationStateProvider _authenticationStateProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJSRuntime _jsRuntime;

        public TsgCoreServiceHelper(IHttpContextAccessor httpContextAccessor, ILogger<TsgCoreServiceHelper> logger, AuthenticationStateProvider authenticationStateProvider, HttpClient http, IConfiguration configuration, IJSRuntime jsRuntime)
        {
            _logger = logger;
            _http = http;
            _configuration = configuration;
            _authenticationStateProvider = authenticationStateProvider;
            _baseUrl = _configuration["Win:Production:Url"] ?? "NO PROD URL"; // Null-coalescing for safety
            _httpContextAccessor = httpContextAccessor;
            _jsRuntime = jsRuntime;


        }

        public async Task<AuthenticateResponse> AuthenticateAsync(string username, string password)
        {
            string serviceCallerId = _configuration["Win:Production:CallerId"] ?? "NO CALLERID CONFIGURAED"; // Null-coalescing for safety
            _logger.LogInformation("AuthenticateAsync:\n<{serviceCallerId}>\n<{username}>\n<{password}>", serviceCallerId, username, password);

            var authRequest = new AuthenticateRequest
            {
                LoginId = username,
                Password = password,
                CallerId = serviceCallerId,
                IncludeUserSettingsInResponse = true,
                IncludeAccessRightsWithUserSettings = false,
            };


            HttpResponseMessage httpResponse = await _http.PostAsJsonAsync($"{_baseUrl}/authenticate", authRequest);
            _logger.LogInformation("AuthenticateAsync, httpResponse StatusCode: {StatusCode}", JsonSerializer.Serialize(httpResponse.StatusCode));
            var statusCode = httpResponse.StatusCode;

            // Extract and serialize the headers
            // Convert the headers to a dictionary for easier serialization
            var headers = httpResponse.Headers.ToDictionary(
                header => header.Key,
                header => string.Join(", ", header.Value) // Combine multiple values into a single string
            );

            // Serialize the headers dictionary to JSON
            var serializedHeaders = JsonSerializer.Serialize(headers);

            // Log both status code and headers
            _logger.LogInformation(
                "AuthenticateAsync, httpResponse StatusCode: {StatusCode}, Headers: {Headers}",
                statusCode,
                serializedHeaders
            );
            // Deserialize the response content to AuthenticateResponse type
            AuthenticateResponse response = await httpResponse.Content!.ReadFromJsonAsync<AuthenticateResponse>()
                ?? throw new InvalidOperationException("The response content is null or cannot be deserialized.");

            // Check if the deserialized response is not null
            if (response == null)
            {
                throw new InvalidOperationException("The response content could not be deserialized.");
            }

            // Return the deserialized AuthenticateResponse object
            return response;
        } // end of AuthenticateSync

        private async Task<AuthenticateResponse> RefreshAccessTokenAsync(string accessToken, string refreshToken)
        {

            var url = $"{_baseUrl}/Authenticate/Refresh";
            var payload = new
            {
                accessToken = accessToken,
                refreshToken = refreshToken,
            };
            var jsonPayload = JsonSerializer.Serialize(payload);
            var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _http.PostAsync(url, httpContent);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var authResponse = JsonSerializer.Deserialize<AuthenticateResponse>(responseContent);
                if (authResponse == null)
                {
                    _logger.LogError("Failed to deserialize the refresh token response.");
                    throw new InvalidOperationException("Failed to deserialize the refresh token response.");
                }

                return authResponse;
            }
            else
            {
                _logger.LogError($"Failed to refresh access token. Status: {response.StatusCode}. Response: {await response.Content.ReadAsStringAsync()}");
                return null;
                //throw new HttpRequestException($"Failed to refresh access token. Status: {response.StatusCode}");
            }
        } // end of RefreshAccessTokenAsync

        public async Task SetAuthenticationCookie(Tokens tokens, UserSettingsData settings)
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                _logger.LogError("HttpContext is null in SetAuthenticationCookie.");
                throw new InvalidOperationException("HttpContext is not available.");
            }
            // Calculate the expiration DateTime from the expiresIn value
            var accessTokenExpiration = DateTime.UtcNow.AddMinutes(tokens.AccessTokenExpiresInMinutes);
            var refreshTokenExpiration = DateTime.UtcNow.AddHours(tokens.RefreshTokenExpiresInHours); // Assuming you have a similar property

            // Creating the security context
            var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, settings.UserName),
                    new Claim(ClaimTypes.Email, settings.EmailAddress),
                    new Claim("Token", tokens.AccessToken),
                    new Claim("RefreshToken", tokens.RefreshToken),
                    new Claim("AccessTokenExpiration", accessTokenExpiration.ToString("o")), // ISO 8601 format
					new Claim("RefreshTokenExpiration", refreshTokenExpiration.ToString("o")),
                    new Claim("WKYCId", settings.WKYCId ?? "NO WKYCID"),
                    new Claim("BaseCountryCode", settings.BaseCountryCode),
                    new Claim("BaseCurrencyCode",settings.BaseCurrencyCode),
                    new Claim("PreferredLanguage", "en_US"),
                    new Claim("FirstName", settings.FirstName),
                    new Claim("lastName", settings.LastName),
                    new Claim("CultureCode", settings.CultureCode),
                    new Claim("Regex", settings.PasswordRegEx),
                    new Claim("UserId", settings.UserId.ToString()),
                    new Claim("CustomerId", settings.OrganizationId.ToString())
                };
            var identity = new ClaimsIdentity(claims, "AuthToken");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };

            await _httpContextAccessor.HttpContext.SignInAsync("AuthToken", claimsPrincipal, authProperties);

        } // end of SetAuthenticationCookie



        public async Task<bool> RefreshAccessTokenAndRebuildCookie(Boolean doPageRefresh)
        {
            var httpContext = _httpContextAccessor.HttpContext ?? throw new InvalidOperationException("HttpContext is not available.");
            var user = httpContext.User;
            _logger.LogInformation("Refresh token probiong....");
            // check if refresh necessary
            var bufferTime = TimeSpan.FromMinutes(1);
            var expirationClaim = user.Claims.FirstOrDefault(c => c.Type == "AccessTokenExpiration");
            if (expirationClaim != null)
            {
                if (DateTime.TryParse(expirationClaim.Value, out var expirationTime))
                {
                    // Check if the current time is before the expiration time
                    if (DateTime.UtcNow.Add(bufferTime) < expirationTime)
                    {
                        _logger.LogInformation("Refresh token still valid....");
                        return true; // Token is still valid long enough
                    }
                }
            }

            _logger.LogInformation("Refresh token Refreshing....");

            // Extract refreshToken and accessToken from the claims
            var refreshTokenClaim = user.Claims.FirstOrDefault(c => c.Type == "RefreshToken");
            var accessTokenClaim = user.Claims.FirstOrDefault(c => c.Type == "Token");
            if (refreshTokenClaim == null || accessTokenClaim == null)
            {
                _logger.LogError("Refresh token or access token not found in user claims.");
                return false;
            }
            string refreshToken = refreshTokenClaim.Value;
            string accessToken = accessTokenClaim.Value;

            // Refresh the token using both access and refresh tokens
            var authResponse = await RefreshAccessTokenAsync(accessToken, refreshToken);
            if (authResponse == null || string.IsNullOrEmpty(authResponse.Tokens.AccessToken))
            {
                _logger.LogError("Token refresh failed or new access token is null.");
                return false;
            }

            // Prepare to update claims with new tokens and expiration
            var updatedClaims = user.Claims
                .Where(c => c.Type != "Token" && c.Type != "RefreshToken" && c.Type != "AccessTokenExpiration" && c.Type != "RefreshTokenExpiration")
                .ToList();

            // Add refreshed tokens and their expiration times
            updatedClaims.Add(new Claim("Token", authResponse.Tokens.AccessToken));
            updatedClaims.Add(new Claim("RefreshToken", authResponse.Tokens.RefreshToken));

            var accessTokenExpiration = DateTime.UtcNow.AddMinutes(authResponse.Tokens.AccessTokenExpiresInMinutes);
            updatedClaims.Add(new Claim("AccessTokenExpiration", accessTokenExpiration.ToString("o")));

            var refreshTokenExpiration = DateTime.UtcNow.AddHours(authResponse.Tokens.RefreshTokenExpiresInHours);
            updatedClaims.Add(new Claim("RefreshTokenExpiration", refreshTokenExpiration.ToString("o")));

            // Re-authenticate to update the cookie with the refreshed information
            await ReAuthenticateUser(httpContext, updatedClaims);

            return true;
        }

        private async Task ReAuthenticateUser(HttpContext httpContext, List<Claim> updatedClaims)
        {
            var identity = new ClaimsIdentity(updatedClaims, "AuthToken");
            var principal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true // Ensures the login session persists across browser sessions as needed
            };

            await httpContext.SignOutAsync("AuthToken");
            await httpContext.SignInAsync("AuthToken", principal, authProperties);
        }

        #region Customer

        public async Task<WkycGetCustomerResponse> CustomerGetSingleAsync(string? customerId, string token)
        {
            // Ensure customerId is not null or empty before attempting to parse
            if (string.IsNullOrWhiteSpace(customerId))
            {
                // Handle the case where customerId is null or empty
                throw new ArgumentException("customerId cannot be null or empty.");
            }

            Guid customerGuid;
            bool isValidGuid = Guid.TryParse(customerId, out customerGuid);

            if (!isValidGuid)
            {
                // Handle the case where customerId is not a valid GUID
                throw new ArgumentException("customerId is not a valid GUID.");
            }

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var httpResponse = await _http.GetAsync($"{_baseUrl}/Customer/{customerGuid}");

            var rawContent = await httpResponse.Content.ReadAsStringAsync();

            // You can log this rawContent to a file or the console for debugging purposes
            Console.WriteLine(rawContent);

            if (httpResponse.IsSuccessStatusCode)
            {
                try
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true, // Adjusts for case sensitivity issues
                                                            // Add more options as needed
                    };

                    var responseContent = await httpResponse.Content.ReadFromJsonAsync<CustomerGetResponse>(options);

                    if (responseContent == null)
                    {
                        return new WkycGetCustomerResponse()
                        {
                            IsSuccessful = false,
                            ErrorMessages = new List<string> { "Response content is null." },
                            Data = new CustomerGetData()
                        };
                    }
                    else
                    {
                        return new WkycGetCustomerResponse()
                        {
                            IsSuccessful = true,
                            ErrorMessages = new List<string>(), // No need to add an empty string
                            Data = responseContent.Customer
                        };
                    }
                }
                catch (JsonException jsonException)
                {
                    // Log the exception details
                    return new WkycGetCustomerResponse()
                    {
                        IsSuccessful = false,
                        ErrorMessages = new List<string> { jsonException.Message },
                        Data = new CustomerGetData()
                    };
                }
            }

            else
            {
                // Handle the case where the HTTP request was not successful
                var myData = new CustomerGetData();

                return new WkycGetCustomerResponse()
                {
                    IsSuccessful = false,
                    ErrorMessages = new List<string> { "Request failed with status: " + httpResponse.StatusCode },

                    Data = myData
                };
            }
        }  // end of CustomerGetSingleAsync

        public async Task<WkycGetCustomerResponse> CustomerGetSingleAsync(string? customerId)
        {
            string token = "";
            // Ensure customerId is not null or empty before attempting to parse
            if (string.IsNullOrWhiteSpace(customerId))
            {
                // Handle the case where customerId is null or empty
                throw new ArgumentException("customerId cannot be null or empty.");
            }

            Guid customerGuid;
            bool isValidGuid = Guid.TryParse(customerId, out customerGuid);

            if (!isValidGuid)
            {
                // Handle the case where customerId is not a valid GUID
                throw new ArgumentException("customerId is not a valid GUID.");
            }

            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userClaims = user.Claims;
                token = userClaims?.FirstOrDefault(c => c.Type == "Token")?.Value ?? "Unknown";
            }
            return await CustomerGetSingleAsync(customerId, token);
        }  // end of CustomerGetSingleAsync

        public async Task<WkycUpdateCustomerResponse> CustomerUpdateAsync(CustomerUpdateRequest request, string token)
        {
            Console.WriteLine($"token: {token}");
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            Console.WriteLine($"request: {JsonSerializer.Serialize(request)}");
            var jsonPayload = JsonSerializer.Serialize(request);
            Console.WriteLine($"jsonPayload: {JsonSerializer.Serialize(jsonPayload)}");
            // Create HttpContent from JSON payload
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            Console.WriteLine($"content: {JsonSerializer.Serialize(content)}");
            var httpResponse = await _http.PatchAsync($"{_baseUrl}/Customer", content);

            var rawContent = await httpResponse.Content.ReadAsStringAsync();

            // You can log this rawContent to a file or the console for debugging purposes
            Console.WriteLine($"rawContent: {JsonSerializer.Serialize(rawContent)}");

            if (httpResponse.IsSuccessStatusCode)
            {
                try
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true, // Adjusts for case sensitivity issues
                                                            // Add more options as needed
                    };

                    var responseContent = await httpResponse.Content.ReadFromJsonAsync<CustomerUpdateResponse>(options);
                    Console.WriteLine($"responseContent: {JsonSerializer.Serialize(responseContent)}");
                    Console.WriteLine($"problems: {JsonSerializer.Serialize(responseContent?.Problems)}");

                    if (responseContent == null)
                    {
                        return new WkycUpdateCustomerResponse()
                        {
                            IsSuccessful = false,
                            ErrorMessages = new List<string> { "Response content is null." },
                        };
                    }
                    else
                    {
                        return new WkycUpdateCustomerResponse()
                        {
                            IsSuccessful = true,
                            ErrorMessages = new List<string>(), // No need to add an empty string
                            Customer = responseContent.Customer
                        };
                    }
                }
                catch (JsonException jsonException)
                {
                    // Log the exception details
                    return new WkycUpdateCustomerResponse()
                    {
                        IsSuccessful = false,
                        ErrorMessages = new List<string> { jsonException.Message },
                    };
                }
            }

            else
            {
                // Handle the case where the HTTP request was not successful
                var myData = new CustomerGetData();

                return new WkycUpdateCustomerResponse()
                {
                    IsSuccessful = false,
                    ErrorMessages = new List<string> { "Request failed with status: " + httpResponse.StatusCode },
                };
            }
        }  // end of CustomerUpdateAsync

        public async Task<WkycUpdateCustomerResponse> CustomerUpdateAsync(CustomerUpdateRequest request)
        {
            Console.WriteLine($"CustomerUpdateAsync without token");
            Console.WriteLine($"CustomerUpdateAsync without token, retrieve logging in token");
            string token = "";
            // Ensure customerId is not null or empty before attempting to parse
            if (request is null)
            {
                // Handle the case where customerId is null or empty
                throw new ArgumentException("Customer details cannot be null or empty.");
            }

            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userClaims = user.Claims;
                token = userClaims?.FirstOrDefault(c => c.Type == "Token")?.Value ?? "Unknown";
            }
            else
            {
                return new WkycUpdateCustomerResponse()
                {
                    IsSuccessful = false,
                    ErrorMessages = new List<string> { "No Token User is not authenticated." },
                };
            }

            Console.WriteLine($"token: {token}");
            return await CustomerUpdateAsync(request, token);
        }  // end of CustomerUpdateAsync

        public async Task<WkycCustomerCreateResponse> CreateCustomerAsync(CustomerCreateRequest request, string token)
        {
            // Ensure customerId is not null or empty before attempting to parse
            if (request is null)
            {
                // Handle the case where customerId is null or empty
                throw new ArgumentException("Customer details cannot be null or empty.");
            }

            Console.WriteLine($"token: {token}");
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            Console.WriteLine($"request: {JsonSerializer.Serialize(request)}");
            var jsonPayload = JsonSerializer.Serialize(request);
            Console.WriteLine($"jsonPayload: {JsonSerializer.Serialize(jsonPayload)}");
            // Create HttpContent from JSON payload
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            Console.WriteLine($"content: {JsonSerializer.Serialize(content)}");
            var httpResponse = await _http.PostAsync($"{_baseUrl}/Customer", content);

            var rawContent = await httpResponse.Content.ReadAsStringAsync();

            // You can log this rawContent to a file or the console for debugging purposes
            Console.WriteLine($"rawContent: {rawContent}");

            if (httpResponse.IsSuccessStatusCode)
            {
                try
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true, // Adjusts for case sensitivity issues
                                                            // Add more options as needed
                    };

                    var responseContent = await httpResponse.Content.ReadFromJsonAsync<CustomerCreateResponse>(options);
                    Console.WriteLine($"responseContent: {JsonSerializer.Serialize(responseContent)}");
                    Console.WriteLine($"problems: {JsonSerializer.Serialize(responseContent?.Problems)}");

                    if (responseContent == null)
                    {
                        return new WkycCustomerCreateResponse()
                        {
                            IsSuccessful = false,
                            ErrorMessages = new List<string> { "Response content is null." },
                        };
                    }
                    else
                    {
                        return new WkycCustomerCreateResponse()
                        {
                            IsSuccessful = true,
                            ErrorMessages = new List<string>(), // No need to add an empty string
                            Customer = responseContent.Customer
                        };
                    }
                }
                catch (JsonException jsonException)
                {
                    // Log the exception details
                    return new WkycCustomerCreateResponse()
                    {
                        IsSuccessful = false,
                        ErrorMessages = new List<string> { jsonException.Message },
                    };
                }
            }
            else
            {
                // Handle the case where the HTTP request was not successful
                return new WkycCustomerCreateResponse()
                {
                    IsSuccessful = false,
                    ErrorMessages = new List<string> { "Request failed with status: " + httpResponse.StatusCode },
                };
            }
        }  // end of CreateCustomerAsync

        public async Task<WkycCustomerCreateResponse> CreateCustomerAsync(CustomerCreateRequest request)
        {
            string token = "";
            // Ensure customerId is not null or empty before attempting to parse
            if (request is null)
            {
                // Handle the case where customerId is null or empty
                throw new ArgumentException("Customer details cannot be null or empty.");
            }

            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userClaims = user.Claims;
                token = userClaims?.FirstOrDefault(c => c.Type == "Token")?.Value ?? "Unknown";
            }
            else
            {
                return new WkycCustomerCreateResponse()
                {
                    IsSuccessful = false,
                    ErrorMessages = new List<string> { "No Token User is not authenticated." },
                };
            }

            Console.WriteLine($"token: {token}");

            return await CreateCustomerAsync(request, token);
        }  // end of CreateCustomerAsync

        public async Task<WkycCustomerCreateFromTemplateResponse> CreateCustomerFromTemplateAsync(CustomerCreateFromTemplateRequest request, string token)
        {
            // Ensure customerId is not null or empty before attempting to parse
            if (request is null)
            {
                // Handle the case where customerId is null or empty
                throw new ArgumentException("Customer details cannot be null or empty.");
            }

            Console.WriteLine($"token: {token}");
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            Console.WriteLine($"request: {JsonSerializer.Serialize(request)}");
            var jsonPayload = JsonSerializer.Serialize(request);
            Console.WriteLine($"jsonPayload: {JsonSerializer.Serialize(jsonPayload)}");
            // Create HttpContent from JSON payload
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            Console.WriteLine($"content: {JsonSerializer.Serialize(content)}");
            var httpResponse = await _http.PostAsync($"{_baseUrl}/Customer/FromTemplate", content);

            var rawContent = await httpResponse.Content.ReadAsStringAsync();

            // You can log this rawContent to a file or the console for debugging purposes
            Console.WriteLine($"rawContent: {rawContent}");

            if (httpResponse.IsSuccessStatusCode)
            {
                try
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true, // Adjusts for case sensitivity issues
                                                            // Add more options as needed
                    };

                    var responseContent = await httpResponse.Content.ReadFromJsonAsync<CustomerCreateFromTemplateResponse>(options);
                    Console.WriteLine($"responseContent: {JsonSerializer.Serialize(responseContent)}");
                    Console.WriteLine($"problems: {JsonSerializer.Serialize(responseContent?.Problems)}");

                    if (responseContent == null)
                    {
                        return new WkycCustomerCreateFromTemplateResponse()
                        {
                            IsSuccessful = false,
                            ErrorMessages = new List<string> { "Response content is null." },
                        };
                    }
                    else
                    {
                        return new WkycCustomerCreateFromTemplateResponse()
                        {
                            IsSuccessful = true,
                            ErrorMessages = new List<string>(), // No need to add an empty string
                            Customer = responseContent.Customer
                        };
                    }
                }
                catch (JsonException jsonException)
                {
                    // Log the exception details
                    return new WkycCustomerCreateFromTemplateResponse()
                    {
                        IsSuccessful = false,
                        ErrorMessages = new List<string> { jsonException.Message },
                    };
                }
            }

            else
            {
                // Handle the case where the HTTP request was not successful
                return new WkycCustomerCreateFromTemplateResponse()
                {
                    IsSuccessful = false,
                    ErrorMessages = new List<string> { "Request failed with status: " + httpResponse.StatusCode },
                };
            }
        }  // end of CreateCustomerFromTemplateAsync

        public async Task<WkycCustomerCreateFromTemplateResponse> CreateCustomerFromTemplateAsync(CustomerCreateFromTemplateRequest request)
        {
            string token = "";
            // Ensure customerId is not null or empty before attempting to parse
            if (request is null)
            {
                // Handle the case where customerId is null or empty
                throw new ArgumentException("Customer details cannot be null or empty.");
            }

            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userClaims = user.Claims;
                token = userClaims?.FirstOrDefault(c => c.Type == "Token")?.Value ?? "Unknown";
            }
            else
            {
                return new WkycCustomerCreateFromTemplateResponse()
                {
                    IsSuccessful = false,
                    ErrorMessages = new List<string> { "No Token User is not authenticated." },
                };
            }

            Console.WriteLine($"token: {token}");

            return await CreateCustomerFromTemplateAsync(request, token);
        }  // end of CreateCustomerFromTemplateAsync

        public async Task<WkycCustomerAccountAliasListResponse> GetCustomerAccountAliasListAsync(string customerId, string token)
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var httpResponse = await _http.GetAsync($"{_baseUrl}/CustomerAccountAliasList/{customerId}");

            var rawContent = await httpResponse.Content.ReadAsStringAsync();

            // You can log this rawContent to a file or the console for debugging purposes
            Console.WriteLine(rawContent);

            if (httpResponse.IsSuccessStatusCode)
            {
                try
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true, // Adjusts for case sensitivity issues
                                                            // Add more options as needed
                    };

                    var responseContent = await httpResponse.Content.ReadFromJsonAsync<CustomerAccountAliasListGetResponse>(options);

                    if (responseContent == null)
                    {
                        return new WkycCustomerAccountAliasListResponse()
                        {
                            IsSuccessful = false,
                            ErrorMessages = new List<string> { "Response content is null." }
                        };
                    }
                    else
                    {
                        return new WkycCustomerAccountAliasListResponse()
                        {
                            IsSuccessful = true,
                            ErrorMessages = new List<string>(),
                            Aliases = responseContent.Aliases
                        };
                    }
                }
                catch (JsonException jsonException)
                {
                    // Log the exception details
                    return new WkycCustomerAccountAliasListResponse()
                    {
                        IsSuccessful = false,
                        ErrorMessages = new List<string> { jsonException.Message },
                    };
                }
            }

            else
            {
                return new WkycCustomerAccountAliasListResponse()
                {
                    IsSuccessful = false,
                    ErrorMessages = new List<string> { "Request failed with status: " + httpResponse.StatusCode },
                };
            }
        }  // end of GetCustomerAccountAliasListAsync

        public async Task<WkycCustomerAccountAliasListResponse> GetCustomerAccountAliasListAsync(string customerId)
        {
            string token = "";
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userClaims = user.Claims;
                token = userClaims?.FirstOrDefault(c => c.Type == "Token")?.Value ?? "Unknown";
            }

            return await GetCustomerAccountAliasListAsync(customerId, token);
        }  // end of GetCustomerAccountAliasListAsync

        public async Task<WkycCustomerAccountAliasCreateResponse> CreateCustomerAccountAliasAsync(CustomerAccountAliasCreateRequest request, string token)
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            _logger.LogInformation("request: {Request}", JsonSerializer.Serialize(request));
            var jsonPayload = JsonSerializer.Serialize(request);
            _logger.LogInformation("jsonPayload: {JsonPayload}", JsonSerializer.Serialize(jsonPayload));
            // Create HttpContent from JSON payload
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            _logger.LogInformation("content: {Content}", JsonSerializer.Serialize(content));
            var httpResponse = await _http.PostAsync($"{_baseUrl}/CustomerAccountAliasList", content);

            var rawContent = await httpResponse.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true, // Adjusts for case sensitivity issues
                                                    // Add more options as needed
            };

            _logger.LogInformation("rawContent: {RawContent}", JsonSerializer.Serialize(rawContent));
            _logger.LogInformation("httpResponse.IsSuccessStatusCode: {IsSuccessStatusCode}", httpResponse.IsSuccessStatusCode);

            if (httpResponse.IsSuccessStatusCode)
            {
                try
                {
                    var problemElement = GetProblems(rawContent);
                    if (problemElement.ValueKind != JsonValueKind.Null)
                    {
                        _logger.LogInformation("problemElement: {ProblemElement}", JsonSerializer.Serialize(problemElement));
                        var errorMessageDetails = GetErrorMessageDetails(problemElement);
                        _logger.LogInformation("errorMessageDetails: {ErrorMessageDetails}", JsonSerializer.Serialize(errorMessageDetails));
                        return new WkycCustomerAccountAliasCreateResponse()
                        {
                            IsSuccessful = false,
                            ErrorMessages = new List<string> { errorMessageDetails },
                        };
                    }

                    var responseContent = await httpResponse.Content.ReadFromJsonAsync<CustomerAccountAliasCreateResponse>(options);
                    _logger.LogInformation("responseContent: {ResponseContent}", JsonSerializer.Serialize(responseContent));
                    _logger.LogInformation("Problems: {Problems}", JsonSerializer.Serialize(responseContent?.Problems));

                    if (responseContent == null)
                    {
                        return new WkycCustomerAccountAliasCreateResponse()
                        {
                            IsSuccessful = false,
                            ErrorMessages = new List<string> { "Response content is null." },
                        };
                    }
                    else
                    {
                        return new WkycCustomerAccountAliasCreateResponse()
                        {
                            IsSuccessful = true,
                            ErrorMessages = new List<string>(), // No need to add an empty string
                        };
                    }
                }
                catch (JsonException jsonException)
                {
                    // Log the exception details
                    return new WkycCustomerAccountAliasCreateResponse()
                    {
                        IsSuccessful = false,
                        ErrorMessages = new List<string> { jsonException.Message },
                    };
                }
            }
            else
            {
                var messageDetails = GetErrorMessageDetails(rawContent);

                //if (errorResponseContent is not null && errorResponseContent.Problems is not null && errorResponseContent.Problems.Any())
                if (!string.IsNullOrEmpty(messageDetails))
                {
                    return new WkycCustomerAccountAliasCreateResponse()
                    {
                        IsSuccessful = false,
                        ErrorMessages = new List<string> { messageDetails },
                    };
                }
                else
                {
                    return new WkycCustomerAccountAliasCreateResponse()
                    {
                        IsSuccessful = false,
                        ErrorMessages = new List<string> { "Request failed with status: " + httpResponse.StatusCode },
                    };
                }
            }
        }  // end of CreateCustomerAccountAliasAsync

        public async Task<WkycCustomerAccountAliasCreateResponse> CreateCustomerAccountAliasAsync(CustomerAccountAliasCreateRequest request)
        {
            string token = "";
            // Ensure customerId is not null or empty before attempting to parse
            if (request is null)
            {
                // Handle the case where customerId is null or empty
                throw new ArgumentException("Customer details cannot be null or empty.");
            }

            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userClaims = user.Claims;
                token = userClaims?.FirstOrDefault(c => c.Type == "Token")?.Value ?? "Unknown";
            }
            else
            {
                return new WkycCustomerAccountAliasCreateResponse()
                {
                    IsSuccessful = false,
                    ErrorMessages = new List<string> { "No Token User is not authenticated." },
                };
            }

            return await CreateCustomerAccountAliasAsync(request, token);
        }  // end of CreateCustomerAccountAliasAsync

        public async Task<WkycCustomerAccountAliasSetDefaultResponse> SetDefaultCustomerAccountAliasAsync(string customerId, string alias, string token)
        {
            var postResponse = new WkycCustomerAccountAliasSetDefaultResponse()
            {
                IsSuccessful = false,
                ErrorMessages = [""],
            };

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var httpResponse = await _http.PostAsync($"{_baseUrl}/CustomerAccountAliasList/SetDefault?customerId={customerId}&alias={alias}", null);

            Console.WriteLine($"httpResponse: {JsonSerializer.Serialize(httpResponse)}");

            if (httpResponse.IsSuccessStatusCode)
            {
                // Deserialize JSON response into your data object
                var responseContent = await httpResponse.Content.ReadFromJsonAsync<WkycCustomerAccountAliasSetDefaultResponse>();

                if (responseContent == null)
                {
                    postResponse.ErrorMessages = new List<string> { "Response content is null." };
                }
                else
                {
                    postResponse.IsSuccessful = true;
                    postResponse.ErrorMessages = new List<string>(); // Clear any placeholder error message
                }
            }
            else
            {
                var error = await httpResponse.Content.ReadAsStringAsync();
                postResponse.ErrorMessages = new List<string> { $"Request failed with status: {httpResponse.StatusCode}, Error: {error}" };
            }

            return postResponse;

        } // end of SetDefaultCustomerAccountAliasAsync

        public async Task<WkycCustomerAccountAliasSetDefaultResponse> SetDefaultCustomerAccountAliasAsync(string customerId, string alias)
        {
            string token = "";

            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userClaims = user.Claims;
                token = userClaims?.FirstOrDefault(c => c.Type == "Token")?.Value ?? "Unknown";
            }

            return await SetDefaultCustomerAccountAliasAsync(customerId, alias, token);
        } // end of SetDefaultCustomerAccountAliasAsync

        public async Task<WkycCustomerAccountAliasDeleteResponse> DeleteCustomerAccountAliasAsync(string customerId, string alias, string token)
        {
            var deleteResponse = new WkycCustomerAccountAliasDeleteResponse()
            {
                IsSuccessful = false,
                ErrorMessages = [""],
            };

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var httpResponse = await _http.DeleteAsync($"{_baseUrl}/CustomerAccountAliasList?customerId={customerId}&alias={alias}");

            Console.WriteLine($"httpResponse: {JsonSerializer.Serialize(httpResponse)}");

            if (httpResponse.IsSuccessStatusCode)
            {
                // Deserialize JSON response into your data object
                var responseContent = await httpResponse.Content.ReadFromJsonAsync<WkycCustomerAccountAliasDeleteResponse>();

                if (responseContent == null)
                {
                    deleteResponse.ErrorMessages = new List<string> { "Response content is null." };
                }
                else
                {
                    deleteResponse.IsSuccessful = true;
                    deleteResponse.ErrorMessages = new List<string>(); // Clear any placeholder error message
                }
            }
            else
            {
                var error = await httpResponse.Content.ReadAsStringAsync();
                deleteResponse.ErrorMessages = new List<string> { $"Request failed with status: {httpResponse.StatusCode}, Error: {error}" };
            }

            return deleteResponse;

        } // end of DeleteCustomerAccountAliasAsync

        public async Task<WkycCustomerAccountAliasDeleteResponse> DeleteCustomerAccountAliasAsync(string customerId, string alias)
        {
            string token = "";

            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userClaims = user.Claims;
                token = userClaims?.FirstOrDefault(c => c.Type == "Token")?.Value ?? "Unknown";
            }

            return await DeleteCustomerAccountAliasAsync(customerId, alias, token);
        } // end of DeleteCustomerAccountAliasAsync

        #endregion

        #region FileAttachment

        public async Task<WkycFileAttachemenmtListResponse> FileAttachmentGetFileListForObjectAsync(string customerId)
        {
            string token = "";

            var fileAttchmentListResponse = new WkycFileAttachemenmtListResponse()
            {
                IsSuccessful = false,
                ErrorMessages = [""],
                Data = new List<FileAttachmentGetInfo>()
            };

            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userClaims = user.Claims;
                token = userClaims?.FirstOrDefault(c => c.Type == "Token")?.Value ?? "Unknown";
            }
            else
            {
                fileAttchmentListResponse.ErrorMessages = ["No Token User is not authenticated."];
                return fileAttchmentListResponse;
            }

            // Use the injected _http HttpClient instance
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var url = $"{_baseUrl}/FileAttachmentInfoList/{customerId}";
            var httpResponse = await _http.GetAsync(url);

            if (httpResponse.IsSuccessStatusCode)
            {
                // Deserialize JSON response into your data object
                var responseContent = await httpResponse.Content.ReadFromJsonAsync<List<FileAttachmentGetInfo>>();
                if (responseContent == null)
                {
                    fileAttchmentListResponse.ErrorMessages = new List<string> { "Response content is null." };
                }
                else
                {
                    fileAttchmentListResponse.IsSuccessful = true;
                    fileAttchmentListResponse.ErrorMessages = new List<string>(); // Clear any placeholder error message

                    fileAttchmentListResponse.Data = responseContent;

                }
            }
            else
            {
                var error = await httpResponse.Content.ReadAsStringAsync();
                fileAttchmentListResponse.ErrorMessages = new List<string> { $"Request failed with status: {httpResponse.StatusCode}, Error: {error}" };
            }

            return fileAttchmentListResponse;

        } // end of FileAttachmentGetFileListForObjectAsync


        public async Task<WkycFileAttachmentGetDataResponse> FileAttachmentGetDataAsync(string fileAttachmentId)
        {
            string token = "";
            var fileAttachemenmtGetDataResponse = new WkycFileAttachmentGetDataResponse()
            {
                IsSuccessful = false,
                ErrorMessages = [""],
                Data = new FileAttachmentGetData()
            };


            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userClaims = user.Claims;
                token = userClaims?.FirstOrDefault(c => c.Type == "Token")?.Value ?? "Unknown";
            }
            else
            {
                fileAttachemenmtGetDataResponse.ErrorMessages = ["No Token User is not authenticated."];
                return fileAttachemenmtGetDataResponse;
            }


            // Use the injected _http HttpClient instance
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var url = $"{_baseUrl}/FileAttachment/{fileAttachmentId}";
            var httpResponse = await _http.GetAsync(url);

            if (httpResponse.IsSuccessStatusCode)
            {
                // Deserialize JSON response into your data object
                var responseContent = await httpResponse.Content.ReadFromJsonAsync<FileAttachmentGetData>();
                if (responseContent == null)
                {
                    fileAttachemenmtGetDataResponse.ErrorMessages = new List<string> { "Response content is null." };
                }
                else
                {
                    fileAttachemenmtGetDataResponse.IsSuccessful = true;
                    fileAttachemenmtGetDataResponse.ErrorMessages = new List<string>(); // Clear any placeholder error message

                    fileAttachemenmtGetDataResponse.Data = responseContent;

                }
            }
            else
            {
                var error = await httpResponse.Content.ReadAsStringAsync();
                fileAttachemenmtGetDataResponse.ErrorMessages = new List<string> { $"Request failed with status: {httpResponse.StatusCode}, Error: {error}" };
            }

            return fileAttachemenmtGetDataResponse;

        } // end of FileAttachmentGetDataAsync

        public async Task<WkycFileAttachmentGetDataResponse> FileAttachmentAddAsync(FileAttachmentAddFileRequest request)
        {
            string token = "";
            var fileAttachemenmtGetDataResponse = new WkycFileAttachmentGetDataResponse()
            {
                IsSuccessful = false,
                ErrorMessages = [""],
                Data = new FileAttachmentGetData()
            };

            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userClaims = user.Claims;
                token = userClaims?.FirstOrDefault(c => c.Type == "Token")?.Value ?? "Unknown";
            }
            else
            {
                fileAttachemenmtGetDataResponse.ErrorMessages = ["No Token User is not authenticated."];
                return fileAttachemenmtGetDataResponse;
            }

            // Use the injected _http HttpClient instance
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var jsonPayload = JsonSerializer.Serialize(request);
            // Create HttpContent from JSON payload
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            var httpResponse = await _http.PostAsync($"{_baseUrl}/FileAttachment", content);
            Console.WriteLine($"httpResponse: {JsonSerializer.Serialize(httpResponse)}");

            if (httpResponse.IsSuccessStatusCode)
            {
                // Deserialize JSON response into your data object
                var responseContent = await httpResponse.Content.ReadFromJsonAsync<FileAttachmentAddFileResponse>();

                if (responseContent == null)
                {
                    fileAttachemenmtGetDataResponse.ErrorMessages = new List<string> { "Response content is null." };
                }
                else
                {
                    var httpFileDataResponse = await _http.GetAsync($"{_baseUrl}/FileAttachment/{responseContent.FileAttachment.FileAttachmentId}");
                    if (httpFileDataResponse.IsSuccessStatusCode)
                    {
                        // Deserialize JSON response into your data object
                        var fileDataresponseContent = await httpFileDataResponse.Content.ReadFromJsonAsync<FileAttachmentGetResponse>();
                        if (fileDataresponseContent == null)
                        {
                            fileAttachemenmtGetDataResponse.ErrorMessages = new List<string> { "Response content is null." };
                        }
                        else
                        {
                            fileAttachemenmtGetDataResponse.IsSuccessful = true;
                            fileAttachemenmtGetDataResponse.ErrorMessages = new List<string>(); // Clear any placeholder error message

                            fileAttachemenmtGetDataResponse.Data = fileDataresponseContent.FileAttachment;

                        }
                    }
                    else
                    {
                        var error = await httpResponse.Content.ReadAsStringAsync();
                        fileAttachemenmtGetDataResponse.ErrorMessages = new List<string> { $"Request failed with status: {httpResponse.StatusCode}, Error: {error}" };
                    }
                }
            }
            else
            {
                var error = await httpResponse.Content.ReadAsStringAsync();
                fileAttachemenmtGetDataResponse.ErrorMessages = new List<string> { $"Request failed with status: {httpResponse.StatusCode}, Error: {error}" };
            }

            return fileAttachemenmtGetDataResponse;

        } // end of FileAttachmentAddAsync

        public async Task<WkycFileAttachmentUpdateFileInfoResponse> FileAttachmentUpdateAsync(FileAttachmentUpdateFileInfoRequest request)
        {
            string token = "";
            var fileAttachmentUpdateResponse = new WkycFileAttachmentUpdateFileInfoResponse()
            {
                IsSuccessful = false,
                ErrorMessages = [""],
            };

            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userClaims = user.Claims;
                token = userClaims?.FirstOrDefault(c => c.Type == "Token")?.Value ?? "Unknown";
            }
            else
            {
                fileAttachmentUpdateResponse.ErrorMessages = ["No Token User is not authenticated."];
                return fileAttachmentUpdateResponse;
            }

            // Use the injected _http HttpClient instance
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var jsonPayload = JsonSerializer.Serialize(request);
            Console.WriteLine($"jsonPayload: {JsonSerializer.Serialize(jsonPayload)}");
            // Create HttpContent from JSON payload
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            Console.WriteLine($"content: {JsonSerializer.Serialize(content)}");
            var httpResponse = await _http.PatchAsync($"{_baseUrl}/FileAttachment", content);
            Console.WriteLine($"httpResponse: {JsonSerializer.Serialize(httpResponse)}");

            if (httpResponse.IsSuccessStatusCode)
            {
                // Deserialize JSON response into your data object
                var responseContent = await httpResponse.Content.ReadFromJsonAsync<FileAttachmentUpdateFileInfoResponse>();

                if (responseContent == null)
                {
                    fileAttachmentUpdateResponse.ErrorMessages = new List<string> { "Response content is null." };
                }
                else
                {
                    fileAttachmentUpdateResponse.IsSuccessful = true;
                    fileAttachmentUpdateResponse.ErrorMessages = new List<string>(); // Clear any placeholder error message
                }
            }
            else
            {
                var error = await httpResponse.Content.ReadAsStringAsync();
                fileAttachmentUpdateResponse.ErrorMessages = new List<string> { $"Request failed with status: {httpResponse.StatusCode}, Error: {error}" };
            }

            return fileAttachmentUpdateResponse;

        } // end of FileAttachmentUpdateAsync

        public async Task<WkycFileAttachmentDeleteFileResponse> FileAttachmentDeleteAsync(Guid fileAttachmentId)
        {
            string token = "";

            var fileAttchmentDeleteResponse = new WkycFileAttachmentDeleteFileResponse()
            {
                IsSuccessful = false,
                ErrorMessages = [""],
            };

            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userClaims = user.Claims;
                token = userClaims?.FirstOrDefault(c => c.Type == "Token")?.Value ?? "Unknown";
            }
            else
            {
                fileAttchmentDeleteResponse.ErrorMessages = ["No Token User is not authenticated."];
                return fileAttchmentDeleteResponse;
            }

            // Use the injected _http HttpClient instance
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var url = $"{_baseUrl}/FileAttachment/{fileAttachmentId}";
            var httpResponse = await _http.GetAsync(url);

            if (httpResponse.IsSuccessStatusCode)
            {
                // Deserialize JSON response into your data object
                var responseContent = await httpResponse.Content.ReadFromJsonAsync<FileAttachmentDeleteFileResponse>();
                if (responseContent == null)
                {
                    fileAttchmentDeleteResponse.ErrorMessages = new List<string> { "Response content is null." };
                }
                else
                {
                    fileAttchmentDeleteResponse.IsSuccessful = true;
                    fileAttchmentDeleteResponse.ErrorMessages = new List<string>(); // Clear any placeholder error message
                }
            }
            else
            {
                var error = await httpResponse.Content.ReadAsStringAsync();
                fileAttchmentDeleteResponse.ErrorMessages = new List<string> { $"Request failed with status: {httpResponse.StatusCode}, Error: {error}" };
            }

            return fileAttchmentDeleteResponse;

        } // end of FileAttachmentDeleteAsync

        #endregion

        #region VerifiedLink

        public async Task<VerifiedLinkSearchResponse> VerifiedLinkSearchAsync(string verifiedLinkTypeId, string reference, int pageIndex = 0, int pageSize = 25, int amountMin = 0)
        {
            string token = "";

            // Retrieve the current user's token
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userClaims = user.Claims;
                token = userClaims?.FirstOrDefault(c => c.Type == "Token")?.Value ?? "Unknown";
            }
            else
            {
                _logger.LogError("User is not authenticated.");
                throw new InvalidOperationException("User is not authenticated.");
            }

            // Prepare the request with authorization header
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Construct the URL with query parameters
            var url = $"{_baseUrl}/VerifiedLink/Search?PageIndex={pageIndex}&PageSize={pageSize}&AmountMin={amountMin}";
            if (!string.IsNullOrEmpty(verifiedLinkTypeId))
            {
                url = $"{url}&VerifiedLinkTypeId={verifiedLinkTypeId}";
            }

            if (!string.IsNullOrEmpty(reference))
            {
                url = $"{url}&VerifiedLinkReference={reference}";
            }
            Console.WriteLine($"url: {JsonSerializer.Serialize(url)}");

            // Execute the GET request
            var httpResponse = await _http.GetAsync(url);

            // Ensure successful response status
            httpResponse.EnsureSuccessStatusCode();

            if (httpResponse.Content == null)
            {
                _logger.LogError("Response content is null.");
                throw new InvalidOperationException("Response content is null.");
            }

            try
            {
                // Deserialize the response
                var responseContent = await httpResponse.Content.ReadFromJsonAsync<VerifiedLinkSearchResponse>();

                if (responseContent == null)
                {
                    _logger.LogError("Failed to deserialize the VerifiedLink search response.");
                    throw new InvalidOperationException("Failed to deserialize the VerifiedLink search response.");
                }

                return responseContent;
            }
            catch (JsonException jsonException)
            {
                _logger.LogError($"JSON error: {jsonException.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching VerifiedLink data: {ex.Message}");
                throw;
            }
        } // end of VerifiedLinkSearchAsync

        public async Task<VerifiedLinkData> GetVerifiedLinkDetailsAsync(string verifiedLinkId, string token)
        {
            // Prepare the request with authorization header
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Construct the URL with the Verified Link ID
            var url = $"{_baseUrl}/VerifiedLink/{verifiedLinkId}";

            // Execute the GET request
            var httpResponse = await _http.GetAsync(url);

            // Ensure successful response status
            httpResponse.EnsureSuccessStatusCode();

            if (httpResponse.Content == null)
            {
                _logger.LogError("Response content is null.");
                throw new InvalidOperationException("Response content is null.");
            }

            try
            {
                // Deserialize the response
                var responseContent = await httpResponse.Content.ReadFromJsonAsync<VerifiedLinkGetResponse>();

                if (responseContent == null)
                {
                    _logger.LogError("Failed to deserialize the VerifiedLink details response.");
                    throw new InvalidOperationException("Failed to deserialize the VerifiedLink details response.");
                }

                return responseContent.VerifiedLink;
            }
            catch (JsonException jsonException)
            {
                _logger.LogError($"JSON error: {jsonException.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching VerifiedLink details: {ex.Message}");
                throw;
            }
        } // end of GetVerifiedLinkDetailsAsync

        public async Task<VerifiedLinkData> GetVerifiedLinkDetailsAsync(string verifiedLinkId)
        {
            string token = "";

            // Retrieve the current user's token
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userClaims = user.Claims;
                token = userClaims?.FirstOrDefault(c => c.Type == "Token")?.Value ?? "Unknown";
            }
            else
            {
                _logger.LogError("User is not authenticated.");
                throw new InvalidOperationException("User is not authenticated.");
            }


            return await GetVerifiedLinkDetailsAsync(verifiedLinkId, token);
        } // end of GetVerifiedLinkDetailsAsync

        public async Task<WkycVerifiedLinkUpdateResponse> UpdateVerifiedLinkAsync(VerifiedLinkUpdateRequest request)
        {
            string token = "";
            var updateResponse = new WkycVerifiedLinkUpdateResponse()
            {
                IsSuccessful = false,
                ErrorMessages = [""],
            };

            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userClaims = user.Claims;
                token = userClaims?.FirstOrDefault(c => c.Type == "Token")?.Value ?? "Unknown";
            }
            else
            {

                return updateResponse;
            }

            // Use the injected _http HttpClient instance
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var jsonPayload = JsonSerializer.Serialize(request);
            Console.WriteLine($"jsonPayload: {JsonSerializer.Serialize(jsonPayload)}");
            // Create HttpContent from JSON payload
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            Console.WriteLine($"content: {JsonSerializer.Serialize(content)}");
            var httpResponse = await _http.PutAsync($"{_baseUrl}/VerifiedLink", content);
            Console.WriteLine($"httpResponse: {JsonSerializer.Serialize(httpResponse)}");

            if (httpResponse.IsSuccessStatusCode)
            {
                // Deserialize JSON response into your data object
                var responseContent = await httpResponse.Content.ReadFromJsonAsync<VerifiedLinkUpdateResponse>();

                if (responseContent == null)
                {
                    updateResponse.ErrorMessages = new List<string> { "Response content is null." };
                }
                else
                {
                    updateResponse.IsSuccessful = true;
                    updateResponse.ErrorMessages = new List<string>(); // Clear any placeholder error message
                }
            }
            else
            {
                var error = await httpResponse.Content.ReadAsStringAsync();
                updateResponse.ErrorMessages = new List<string> { $"Request failed with status: {httpResponse.StatusCode}, Error: {error}" };
            }

            return updateResponse;

        } // end of UpdateVerifiedLinkAsync

        public async Task<WkycVerifiedLinkCreateResponse> CreateVerifiedLinkAsync(VerifiedLinkCreateRequest request)
        {
            string token = "";
            var createResponse = new WkycVerifiedLinkCreateResponse()
            {
                IsSuccessful = false,
                ErrorMessages = [""],
            };

            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userClaims = user.Claims;
                token = userClaims?.FirstOrDefault(c => c.Type == "Token")?.Value ?? "Unknown";
            }
            else
            {
                createResponse.IsSuccessful = true;
                return createResponse;
            }

            // Use the injected _http HttpClient instance
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var jsonPayload = JsonSerializer.Serialize(request);
            Console.WriteLine($"jsonPayload: {JsonSerializer.Serialize(jsonPayload)}");
            // Create HttpContent from JSON payload
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            Console.WriteLine($"content: {JsonSerializer.Serialize(content)}");
            var httpResponse = await _http.PostAsync($"{_baseUrl}/VerifiedLink", content);
            Console.WriteLine($"httpResponse: {JsonSerializer.Serialize(httpResponse)}");

            if (httpResponse.IsSuccessStatusCode)
            {
                // Deserialize JSON response into your data object
                var responseContent = await httpResponse.Content.ReadFromJsonAsync<VerifiedLinkCreateResponse>();

                if (responseContent == null)
                {
                    createResponse.ErrorMessages = new List<string> { "Response content is null." };
                }
                else
                {
                    createResponse.IsSuccessful = true;
                    createResponse.ErrorMessages = new List<string>(); // Clear any placeholder error message
                    createResponse.VerifiedLink = responseContent.VerifiedLink;
                }
            }
            else
            {
                var error = await httpResponse.Content.ReadAsStringAsync();
                createResponse.ErrorMessages = new List<string> { $"Request failed with status: {httpResponse.StatusCode}, Error: {error}" };
            }

            return createResponse;

        } // end of CreateVerifiedLinkAsync

        public async Task<WkycVerifiedLinkDeleteResponse> DeleteVerifiedLinkAsync(VerifiedLinkDeleteRequest request)
        {
            string token = "";
            var createResponse = new WkycVerifiedLinkDeleteResponse()
            {
                IsSuccessful = false,
                ErrorMessages = [""],
            };

            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userClaims = user.Claims;
                token = userClaims?.FirstOrDefault(c => c.Type == "Token")?.Value ?? "Unknown";
            }
            else
            {
                createResponse.IsSuccessful = true;
                return createResponse;
            }

            // Use the injected _http HttpClient instance
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            JsonSerializerOptions options = new()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
            };
            var jsonPayload = JsonSerializer.Serialize(request, options);
            Console.WriteLine($"jsonPayload: {JsonSerializer.Serialize(jsonPayload)}");
            // Create HttpContent from JSON payload
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            Console.WriteLine($"content: {JsonSerializer.Serialize(content)}");
            var httpResponse = await _http.PostAsync($"{_baseUrl}/VerifiedLink/Delete", content);
            Console.WriteLine($"httpResponse: {JsonSerializer.Serialize(httpResponse)}");

            if (httpResponse.IsSuccessStatusCode)
            {
                // Deserialize JSON response into your data object
                var responseContent = await httpResponse.Content.ReadFromJsonAsync<VerifiedLinkDeleteResponse>();

                if (responseContent == null)
                {
                    createResponse.ErrorMessages = new List<string> { "Response content is null." };
                }
                else
                {
                    createResponse.IsSuccessful = true;
                    createResponse.ErrorMessages = new List<string>(); // Clear any placeholder error message
                }
            }
            else
            {
                var error = await httpResponse.Content.ReadAsStringAsync();
                createResponse.ErrorMessages = new List<string> { $"Request failed with status: {httpResponse.StatusCode}, Error: {error}" };
            }

            return createResponse;

        } // end of DeleteVerifiedLinkAsync

        #endregion

        #region User

        public async Task<WkycUserPasswordChangeResponse> ChangePasswordAsync(UserPasswordChangeRequest request)
        {
            string token = "";
            var createResponse = new WkycUserPasswordChangeResponse()
            {
                IsSuccessful = false,
                ErrorMessages = [""],
            };

            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userClaims = user.Claims;
                token = userClaims?.FirstOrDefault(c => c.Type == "Token")?.Value ?? "Unknown";
            }
            else
            {
                createResponse.IsSuccessful = false;
                return createResponse;
            }

            // Use the injected _http HttpClient instance
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            JsonSerializerOptions options = new()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
            };

            var jsonPayload = JsonSerializer.Serialize(request);
            Console.WriteLine($"jsonPayload: {JsonSerializer.Serialize(jsonPayload)}");
            // Create HttpContent from JSON payload
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            Console.WriteLine($"content: {JsonSerializer.Serialize(content)}");
            var httpResponse = await _http.PatchAsync($"{_baseUrl}/User/PasswordChange", content);
            Console.WriteLine($"httpResponse: {JsonSerializer.Serialize(httpResponse)}");

            if (httpResponse.IsSuccessStatusCode)
            {
                // Deserialize JSON response into your data object
                var responseContent = await httpResponse.Content.ReadFromJsonAsync<VerifiedLinkDeleteResponse>();

                if (responseContent == null)
                {
                    createResponse.ErrorMessages = new List<string> { "Response content is null." };
                }
                else
                {
                    createResponse.IsSuccessful = true;
                    createResponse.ErrorMessages = new List<string>(); // Clear any placeholder error message
                }
            }
            else
            {
                _logger.LogInformation("Change password failed.");
                var rawContent = await httpResponse.Content.ReadAsStringAsync();
                _logger.LogInformation("rawContent: {RawContent}", rawContent);

                var messageDetails = GetErrorMessageDetails(rawContent);
                _logger.LogInformation("MessageDetails: {MessageDetails}", messageDetails);
                //if (errorResponseContent is not null && errorResponseContent.Problems is not null && errorResponseContent.Problems.Any())
                if (!string.IsNullOrEmpty(messageDetails))
                {
                    return new WkycUserPasswordChangeResponse()
                    {
                        IsSuccessful = false,
                        ErrorMessages = new List<string> { messageDetails },
                    };
                }
                else
                {
                    return new WkycUserPasswordChangeResponse()
                    {
                        IsSuccessful = false,
                        ErrorMessages = new List<string> { "Request failed with status: " + httpResponse.StatusCode },
                    };
                }
            }

            return createResponse;

        } // end of ChangePasswordAsync

        public async Task<WkycUserDoesUsernameExistResponse> IsUsernameExistAsync(string username, string token)
        {
            var wkycResponse = new WkycUserDoesUsernameExistResponse()
            {
                IsSuccessful = false,
                ErrorMessages = [""],
            };

            // Use the injected _http HttpClient instance
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            JsonSerializerOptions options = new()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
            };

            var httpResponse = await _http.GetAsync($"{_baseUrl}/User/DoesUsernameExist/{username}");
            Console.WriteLine($"httpResponse: {JsonSerializer.Serialize(httpResponse)}");

            if (httpResponse.IsSuccessStatusCode)
            {
                // Deserialize JSON response into your data object
                var responseContent = await httpResponse.Content.ReadFromJsonAsync<UserDoesUsernameExistResponse>();

                if (responseContent == null)
                {
                    wkycResponse.ErrorMessages = new List<string> { "Response content is null." };
                }
                else
                {
                    wkycResponse.IsSuccessful = true;
                    wkycResponse.ErrorMessages = new List<string>(); // Clear any placeholder error message
                    wkycResponse.Exists = responseContent.Exists;
                }
            }
            else
            {
                var error = await httpResponse.Content.ReadAsStringAsync();
                wkycResponse.ErrorMessages = new List<string> { $"Request failed with status: {httpResponse.StatusCode}, Error: {error}" };
            }

            return wkycResponse;

        } // end of IsUsernameExistAsync

        public async Task<WkycUserDoesUsernameExistResponse> IsUsernameExistAsync(string username)
        {
            string token = "";
            var createResponse = new WkycUserDoesUsernameExistResponse()
            {
                IsSuccessful = false,
                ErrorMessages = [""],
            };

            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userClaims = user.Claims;
                token = userClaims?.FirstOrDefault(c => c.Type == "Token")?.Value ?? "Unknown";
            }
            else
            {
                createResponse.IsSuccessful = false;
                return createResponse;
            }

            return await IsUsernameExistAsync(username, token);

        } // end of IsUsernameExistAsync

        public async Task<WkycCustomerUserCreateResponse> CreateCustomerUserAsync(CustomerUserCreateRequest request, string token)
        {
            var createResponse = new WkycCustomerUserCreateResponse()
            {
                IsSuccessful = false,
                ErrorMessages = [""],
            };

            // Use the injected _http HttpClient instance
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            JsonSerializerOptions options = new()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
            };

            var jsonPayload = JsonSerializer.Serialize(request);
            Console.WriteLine($"jsonPayload: {JsonSerializer.Serialize(jsonPayload)}");
            // Create HttpContent from JSON payload
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            Console.WriteLine($"content: {JsonSerializer.Serialize(content)}");
            var httpResponse = await _http.PostAsync($"{_baseUrl}/CustomerUser", content);
            Console.WriteLine($"httpResponse: {JsonSerializer.Serialize(httpResponse)}");

            if (httpResponse.IsSuccessStatusCode)
            {
                // Deserialize JSON response into your data object
                var responseContent = await httpResponse.Content.ReadFromJsonAsync<CustomerUserCreateResponse>();

                if (responseContent == null)
                {
                    createResponse.ErrorMessages = new List<string> { "Response content is null." };
                }
                else
                {
                    createResponse.IsSuccessful = true;
                    createResponse.ErrorMessages = new List<string>(); // Clear any placeholder error message
                    createResponse.User = responseContent.User;
                }
            }
            else
            {
                var error = await httpResponse.Content.ReadAsStringAsync();
                createResponse.ErrorMessages = new List<string> { $"Request failed with status: {httpResponse.StatusCode}, Error: {error}" };
            }

            return createResponse;

        } // end of CreateCustomerUserAsync

        public async Task<WkycCustomerUserCreateResponse> CreateCustomerUserAsync(CustomerUserCreateRequest request)
        {
            string token = "";
            var createResponse = new WkycCustomerUserCreateResponse()
            {
                IsSuccessful = false,
                ErrorMessages = [""],
            };

            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userClaims = user.Claims;
                token = userClaims?.FirstOrDefault(c => c.Type == "Token")?.Value ?? "Unknown";
            }
            else
            {
                createResponse.IsSuccessful = false;
                return createResponse;
            }

            return await CreateCustomerUserAsync(request, token);

        } // end of CreateCustomerUserAsync

        public async Task<WkycUserAccessRightTemplateApplyResponse> ApplyUserAccessRightTemplateAsync(UserAccessRightTemplateApplyRequest request, string token)
        {
            var wkycResponse = new WkycUserAccessRightTemplateApplyResponse()
            {
                IsSuccessful = false,
                ErrorMessages = [""],
            };

            // Use the injected _http HttpClient instance
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            JsonSerializerOptions options = new()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
            };

            var jsonPayload = JsonSerializer.Serialize(request);
            Console.WriteLine($"jsonPayload: {JsonSerializer.Serialize(jsonPayload)}");
            // Create HttpContent from JSON payload
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            Console.WriteLine($"content: {JsonSerializer.Serialize(content)}");
            var httpResponse = await _http.PatchAsync($"{_baseUrl}/User/ApplyAccessRightTemplate", content);
            Console.WriteLine($"httpResponse: {JsonSerializer.Serialize(httpResponse)}");

            if (httpResponse.IsSuccessStatusCode)
            {
                // Deserialize JSON response into your data object
                var responseContent = await httpResponse.Content.ReadFromJsonAsync<UserAccessRightTemplateApplyResponse>();

                if (responseContent == null)
                {
                    wkycResponse.ErrorMessages = new List<string> { "Response content is null." };
                }
                else
                {
                    wkycResponse.IsSuccessful = true;
                    wkycResponse.ErrorMessages = new List<string>(); // Clear any placeholder error message
                }
            }
            else
            {
                var error = await httpResponse.Content.ReadAsStringAsync();
                wkycResponse.ErrorMessages = new List<string> { $"Request failed with status: {httpResponse.StatusCode}, Error: {error}" };
            }

            return wkycResponse;

        } // end of ApplyUserAccessRightTemplateAsync

        public async Task<WkycUserAccessRightTemplateApplyResponse> ApplyUserAccessRightTemplateAsync(UserAccessRightTemplateApplyRequest request)
        {
            string token = "";
            var wkycResponse = new WkycUserAccessRightTemplateApplyResponse()
            {
                IsSuccessful = false,
                ErrorMessages = [""],
            };

            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userClaims = user.Claims;
                token = userClaims?.FirstOrDefault(c => c.Type == "Token")?.Value ?? "Unknown";
            }
            else
            {
                wkycResponse.IsSuccessful = false;
                return wkycResponse;
            }

            return await ApplyUserAccessRightTemplateAsync(request, token);

        } // end of ApplyUserAccessRightTemplateAsync

        public async Task<WkycUserAccessRightTemplateLinkResponse> LinkUserAccessRightTemplateAsync(UserAccessRightTemplateLinkRequest request, string token)
        {
            var wkycResponse = new WkycUserAccessRightTemplateLinkResponse()
            {
                IsSuccessful = false,
                ErrorMessages = [""],
            };

            // Use the injected _http HttpClient instance
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            JsonSerializerOptions options = new()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
            };

            var jsonPayload = JsonSerializer.Serialize(request);
            Console.WriteLine($"jsonPayload: {JsonSerializer.Serialize(jsonPayload)}");
            // Create HttpContent from JSON payload
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            Console.WriteLine($"content: {JsonSerializer.Serialize(content)}");
            var httpResponse = await _http.PatchAsync($"{_baseUrl}/User/LinkAccessRightTemplate", content);
            Console.WriteLine($"httpResponse: {JsonSerializer.Serialize(httpResponse)}");

            if (httpResponse.IsSuccessStatusCode)
            {
                // Deserialize JSON response into your data object
                var responseContent = await httpResponse.Content.ReadFromJsonAsync<UserAccessRightTemplateLinkResponse>();

                if (responseContent == null)
                {
                    wkycResponse.ErrorMessages = new List<string> { "Response content is null." };
                }
                else
                {
                    wkycResponse.IsSuccessful = true;
                    wkycResponse.ErrorMessages = new List<string>(); // Clear any placeholder error message
                }
            }
            else
            {
                var error = await httpResponse.Content.ReadAsStringAsync();
                wkycResponse.ErrorMessages = new List<string> { $"Request failed with status: {httpResponse.StatusCode}, Error: {error}" };
            }

            return wkycResponse;

        } // end of LinkUserAccessRightTemplateAsync

        public async Task<WkycUserAccessRightTemplateLinkResponse> LinkUserAccessRightTemplateAsync(UserAccessRightTemplateLinkRequest request)
        {
            string token = "";
            var wkycResponse = new WkycUserAccessRightTemplateLinkResponse()
            {
                IsSuccessful = false,
                ErrorMessages = [""],
            };

            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userClaims = user.Claims;
                token = userClaims?.FirstOrDefault(c => c.Type == "Token")?.Value ?? "Unknown";
            }
            else
            {
                wkycResponse.IsSuccessful = false;
                return wkycResponse;
            }

            return await LinkUserAccessRightTemplateAsync(request, token);

        } // end of LinkUserAccessRightTemplateAsync

        #endregion

        public async Task<WkycCountryListResponse> GetCountryListAsync(string token)
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var httpResponse = await _http.GetAsync($"{_baseUrl}/CountryList");

            var rawContent = await httpResponse.Content.ReadAsStringAsync();

            // You can log this rawContent to a file or the console for debugging purposes
            Console.WriteLine(rawContent);

            if (httpResponse.IsSuccessStatusCode)
            {
                try
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true, // Adjusts for case sensitivity issues
                                                            // Add more options as needed
                    };

                    var responseContent = await httpResponse.Content.ReadFromJsonAsync<CountryListGetResponse>(options);

                    if (responseContent == null)
                    {
                        return new WkycCountryListResponse()
                        {
                            IsSuccessful = false,
                            ErrorMessages = new List<string> { "Response content is null." }
                        };
                    }
                    else
                    {
                        return new WkycCountryListResponse()
                        {
                            IsSuccessful = true,
                            ErrorMessages = new List<string>(),
                            Countries = responseContent.Countries
                        };
                    }
                }
                catch (JsonException jsonException)
                {
                    // Log the exception details
                    return new WkycCountryListResponse()
                    {
                        IsSuccessful = false,
                        ErrorMessages = new List<string> { jsonException.Message },
                    };
                }
            }

            else
            {
                // Handle the case where the HTTP request was not successful
                var myData = new CustomerGetData();

                return new WkycCountryListResponse()
                {
                    IsSuccessful = false,
                    ErrorMessages = new List<string> { "Request failed with status: " + httpResponse.StatusCode },
                };
            }
        }  // end of GetCountryListAsync

        public async Task<WkycCountryListResponse> GetCountryListAsync()
        {
            string token = "";
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userClaims = user.Claims;
                token = userClaims?.FirstOrDefault(c => c.Type == "Token")?.Value ?? "Unknown";
            }

            return await GetCountryListAsync(token);
        }  // end of GetCountryListAsync

        public async Task<WkycPaymentCurrencyListResponse> GetPaymentCurrencyListAsync()
        {
            string token = "";
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userClaims = user.Claims;
                token = userClaims?.FirstOrDefault(c => c.Type == "Token")?.Value ?? "Unknown";
            }
            else
            {
                return new WkycPaymentCurrencyListResponse()
                {
                    IsSuccessful = false,
                    ErrorMessages = new List<string> { "No Token User is not authenticated." },
                };
            }


            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var httpResponse = await _http.GetAsync($"{_baseUrl}/PaymentCurrencyList");

            var rawContent = await httpResponse.Content.ReadAsStringAsync();

            // You can log this rawContent to a file or the console for debugging purposes
            Console.WriteLine(rawContent);

            if (httpResponse.IsSuccessStatusCode)
            {
                try
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true, // Adjusts for case sensitivity issues
                                                            // Add more options as needed
                    };

                    var responseContent = await httpResponse.Content.ReadFromJsonAsync<PaymentCurrencyListGetResponse>(options);

                    if (responseContent == null)
                    {
                        return new WkycPaymentCurrencyListResponse()
                        {
                            IsSuccessful = false,
                            ErrorMessages = new List<string> { "Response content is null." }
                        };
                    }
                    else
                    {
                        return new WkycPaymentCurrencyListResponse()
                        {
                            IsSuccessful = true,
                            ErrorMessages = new List<string>(),
                            Currencies = responseContent.Currencies
                        };
                    }
                }
                catch (JsonException jsonException)
                {
                    // Log the exception details
                    return new WkycPaymentCurrencyListResponse()
                    {
                        IsSuccessful = false,
                        ErrorMessages = new List<string> { jsonException.Message },
                    };
                }
            }

            else
            {
                // Handle the case where the HTTP request was not successful
                var myData = new CustomerGetData();

                return new WkycPaymentCurrencyListResponse()
                {
                    IsSuccessful = false,
                    ErrorMessages = new List<string> { "Request failed with status: " + httpResponse.StatusCode },
                };
            }
        }  // end of GetPaymentCurrencyListAsync

        public async Task<WkycCountryIdentificationTypeListResponse> GetCountryIdentificationTypeListAsync(string token, string countryCode)
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var httpResponse = await _http.GetAsync($"{_baseUrl}/CountryIdentificationTypeList/{countryCode}");

            var rawContent = await httpResponse.Content.ReadAsStringAsync();

            // You can log this rawContent to a file or the console for debugging purposes
            Console.WriteLine(rawContent);

            if (httpResponse.IsSuccessStatusCode)
            {
                try
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true, // Adjusts for case sensitivity issues
                                                            // Add more options as needed
                    };

                    var responseContent = await httpResponse.Content.ReadFromJsonAsync<CountryIdentificationTypeListGetResponse>(options);

                    if (responseContent == null)
                    {
                        return new WkycCountryIdentificationTypeListResponse()
                        {
                            IsSuccessful = false,
                            ErrorMessages = new List<string> { "Response content is null." }
                        };
                    }
                    else
                    {
                        return new WkycCountryIdentificationTypeListResponse()
                        {
                            IsSuccessful = true,
                            ErrorMessages = new List<string>(),
                            IdentificationTypes = responseContent.IdentificationTypes
                        };
                    }
                }
                catch (JsonException jsonException)
                {
                    // Log the exception details
                    return new WkycCountryIdentificationTypeListResponse()
                    {
                        IsSuccessful = false,
                        ErrorMessages = new List<string> { jsonException.Message },
                    };
                }
            }

            else
            {
                return new WkycCountryIdentificationTypeListResponse()
                {
                    IsSuccessful = false,
                    ErrorMessages = new List<string> { "Request failed with status: " + httpResponse.StatusCode },
                };
            }
        }  // end of GetCountryIdentificationTypeListAsync

        public async Task<WkycCountryIdentificationTypeListResponse> GetCountryIdentificationTypeListAsync(string countryCode)
        {
            string token = "";
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userClaims = user.Claims;
                token = userClaims?.FirstOrDefault(c => c.Type == "Token")?.Value ?? "Unknown";
            }

            return await GetCountryIdentificationTypeListAsync(token, countryCode);
        }  // end of GetCountryIdentificationTypeListAsync

        public async Task<WkycUserAccountAliasListResponse> GetUserAccountAliasListAsync(string userId, string token)
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var httpResponse = await _http.GetAsync($"{_baseUrl}/UserAccountAliasList?userId={userId}");

            var rawContent = await httpResponse.Content.ReadAsStringAsync();

            // You can log this rawContent to a file or the console for debugging purposes
            Console.WriteLine(rawContent);

            if (httpResponse.IsSuccessStatusCode)
            {
                try
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true, // Adjusts for case sensitivity issues
                                                            // Add more options as needed
                    };

                    var responseContent = await httpResponse.Content.ReadFromJsonAsync<UserAccountAliasListGetResponse>(options);

                    if (responseContent == null)
                    {
                        return new WkycUserAccountAliasListResponse()
                        {
                            IsSuccessful = false,
                            ErrorMessages = new List<string> { "Response content is null." }
                        };
                    }
                    else
                    {
                        return new WkycUserAccountAliasListResponse()
                        {
                            IsSuccessful = true,
                            ErrorMessages = new List<string>(),
                            AccountAliases = responseContent.AccountAliases
                        };
                    }
                }
                catch (JsonException jsonException)
                {
                    // Log the exception details
                    return new WkycUserAccountAliasListResponse()
                    {
                        IsSuccessful = false,
                        ErrorMessages = new List<string> { jsonException.Message },
                    };
                }
            }

            else
            {
                return new WkycUserAccountAliasListResponse()
                {
                    IsSuccessful = false,
                    ErrorMessages = new List<string> { "Request failed with status: " + httpResponse.StatusCode },
                };
            }
        }  // end of GetUserAccountAliasListAsync

        public async Task<WkycUserAccountAliasListResponse> GetUserAccountAliasListAsync(string userId)
        {
            string token = "";
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userClaims = user.Claims;
                token = userClaims?.FirstOrDefault(c => c.Type == "Token")?.Value ?? "Unknown";
            }

            return await GetUserAccountAliasListAsync(token);
        }  // end of GetUserAccountAliasListAsync

        #region Verify

        public async Task<WkycVerifyResponse> VerifyAsync(VerifyGetRequest request, string token)
        {
            var createResponse = new WkycVerifyResponse()
            {
                IsSuccessful = false,
                ErrorMessages = [""],
            };

            // Use the injected _http HttpClient instance
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            JsonSerializerOptions options = new()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
            };
            var jsonPayload = JsonSerializer.Serialize(request, options);
            Console.WriteLine($"jsonPayload: {JsonSerializer.Serialize(jsonPayload)}");
            // Create HttpContent from JSON payload
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            Console.WriteLine($"content: {JsonSerializer.Serialize(content)}");
            var httpResponse = await _http.PostAsync($"{_baseUrl}/Verify", content);
            Console.WriteLine($"httpResponse: {JsonSerializer.Serialize(httpResponse)}");

            if (httpResponse.IsSuccessStatusCode)
            {
                // Deserialize JSON response into your data object
                var responseContent = await httpResponse.Content.ReadFromJsonAsync<VerifyGetResponse>();

                if (responseContent == null)
                {
                    createResponse.ErrorMessages = new List<string> { "Response content is null." };
                }
                else
                {
                    createResponse.IsSuccessful = true;
                    createResponse.ErrorMessages = new List<string>(); // Clear any placeholder error message
                    createResponse.VLink = responseContent.VLink;
                }
            }
            else
            {
                var error = await httpResponse.Content.ReadAsStringAsync();
                _logger.LogInformation("error : {Error}", error);
                createResponse.ErrorMessages = new List<string> { $"Request failed with status: {httpResponse.StatusCode}, Error: {error}" };
                createResponse.Errors.Add(new WkycError { Code = (int)httpResponse.StatusCode, MessageDetail = error });
            }

            return createResponse;

        } // end of VerifyAsync

        public async Task<WkycVerifyResponse> VerifyAsync(VerifyGetRequest request)
        {
            string token = "";

            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userClaims = user.Claims;
                token = userClaims?.FirstOrDefault(c => c.Type == "Token")?.Value ?? "Unknown";
            }

            return await VerifyAsync(request, token);
        } // end of VerifyAsync

        public async Task<WkycVerifyResponse> VerifyAsync(Guid VLinkId, string token)
        {
            Console.WriteLine($"VerifyAsync, VLinkId: {VLinkId}");

            var createResponse = new WkycVerifyResponse()
            {
                IsSuccessful = false,
                ErrorMessages = [""],
            };

            // Use the injected _http HttpClient instance
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var httpResponse = await _http.PostAsync($"{_baseUrl}/Verify/{VLinkId}", new StringContent(string.Empty));
            Console.WriteLine($"httpResponse: {JsonSerializer.Serialize(httpResponse)}");

            if (httpResponse.IsSuccessStatusCode)
            {
                // Deserialize JSON response into your data object
                var responseContent = await httpResponse.Content.ReadFromJsonAsync<VerifyGetResponse>();

                if (responseContent == null)
                {
                    createResponse.ErrorMessages = new List<string> { "Response content is null." };
                }
                else
                {
                    createResponse.IsSuccessful = true;
                    createResponse.ErrorMessages = new List<string>(); // Clear any placeholder error message
                    createResponse.VLink = responseContent.VLink;
                }
            }
            else
            {
                var error = await httpResponse.Content.ReadAsStringAsync();
                _logger.LogInformation("error : {Error}", error);
                createResponse.ErrorMessages = new List<string> { $"Request failed with status: {httpResponse.StatusCode}, Error: {error}" };
                createResponse.Errors.Add(new WkycError { Code = (int)httpResponse.StatusCode, MessageDetail = error });
            }

            return createResponse;

        } // end of VerifyAsync

        public async Task<WkycVerifyResponse> VerifyAsync(Guid VLinkId)
        {
            Console.WriteLine($"VerifyAsync, VLinkId: {VLinkId}");
            string token = "";

            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userClaims = user.Claims;
                token = userClaims?.FirstOrDefault(c => c.Type == "Token")?.Value ?? "Unknown";
            }

            return await VerifyAsync(VLinkId, token);
        } // end of VerifyAsync

        public async Task<WkycVerifyMultipleResponse> VerifyAsync(VerifyGetMultipleRequest request)
        {
            string token = "";
            var createResponse = new WkycVerifyMultipleResponse()
            {
                IsSuccessful = false,
                ErrorMessages = [""],
            };

            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userClaims = user.Claims;
                token = userClaims?.FirstOrDefault(c => c.Type == "Token")?.Value ?? "Unknown";
            }
            else
            {
                createResponse.IsSuccessful = true;
                return createResponse;
            }

            // Use the injected _http HttpClient instance
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            JsonSerializerOptions options = new()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
            };
            var jsonPayload = JsonSerializer.Serialize(request, options);
            Console.WriteLine($"jsonPayload: {JsonSerializer.Serialize(jsonPayload)}");
            // Create HttpContent from JSON payload
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            Console.WriteLine($"content: {JsonSerializer.Serialize(content)}");
            var httpResponse = await _http.PostAsync($"{_baseUrl}/Verify", content);
            Console.WriteLine($"httpResponse: {JsonSerializer.Serialize(httpResponse)}");

            if (httpResponse.IsSuccessStatusCode)
            {
                // Deserialize JSON response into your data object
                var responseContent = await httpResponse.Content.ReadFromJsonAsync<VerifyGetMultipleResponse>();

                if (responseContent == null)
                {
                    createResponse.ErrorMessages = new List<string> { "Response content is null." };
                }
                else
                {
                    createResponse.IsSuccessful = true;
                    createResponse.ErrorMessages = new List<string>(); // Clear any placeholder error message
                    createResponse.VLinks = responseContent.VLinks;
                }
            }
            else
            {
                var error = await httpResponse.Content.ReadAsStringAsync();
                createResponse.ErrorMessages = new List<string> { $"Request failed with status: {httpResponse.StatusCode}, Error: {error}" };
            }

            return createResponse;

        } // end of VerifyMultipleAsync

        public async Task<WkycVerifyResponse> PublicVerifyAsync(VerifyGetRequest request)
        {
            //string token = "";
            var createResponse = new WkycVerifyResponse()
            {
                IsSuccessful = false,
                ErrorMessages = [""],
            };

            //var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            //var user = authState.User;
            //if (user?.Identity?.IsAuthenticated == true)
            //{
            //	var userClaims = user.Claims;
            //	token = userClaims?.FirstOrDefault(c => c.Type == "Token")?.Value ?? "Unknown";
            //}
            //else
            //{
            //	createResponse.IsSuccessful = true;
            //	return createResponse;
            //}

            // Use the injected _http HttpClient instance
            //_http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            JsonSerializerOptions options = new()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
            };
            var jsonPayload = JsonSerializer.Serialize(request, options);
            Console.WriteLine($"jsonPayload: {JsonSerializer.Serialize(jsonPayload)}");
            // Create HttpContent from JSON payload
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            Console.WriteLine($"content: {JsonSerializer.Serialize(content)}");
            var httpResponse = await _http.PostAsync($"{_baseUrl}/Verify", content);
            Console.WriteLine($"httpResponse: {JsonSerializer.Serialize(httpResponse)}");

            if (httpResponse.IsSuccessStatusCode)
            {
                // Deserialize JSON response into your data object
                var responseContent = await httpResponse.Content.ReadFromJsonAsync<VerifyGetResponse>();

                if (responseContent == null)
                {
                    createResponse.ErrorMessages = new List<string> { "Response content is null." };
                }
                else
                {
                    createResponse.IsSuccessful = true;
                    createResponse.ErrorMessages = new List<string>(); // Clear any placeholder error message
                    createResponse.VLink = responseContent.VLink;
                }
            }
            else
            {
                var error = await httpResponse.Content.ReadAsStringAsync();
                createResponse.ErrorMessages = new List<string> { $"Request failed with status: {httpResponse.StatusCode}, Error: {error}" };
            }

            return createResponse;

        } // end of PublicVerifyAsync

        public async Task<WkycVerifyResponse> PublicVerifyAsync(Guid VLinkId)
        {
            var createResponse = new WkycVerifyResponse()
            {
                IsSuccessful = false,
                ErrorMessages = [""],
            };

            var httpResponse = await _http.PostAsync($"{_baseUrl}/Verify/Public/{VLinkId}", new StringContent(string.Empty));

            if (httpResponse.IsSuccessStatusCode)
            {
                // Deserialize JSON response into your data object
                var responseContent = await httpResponse.Content.ReadFromJsonAsync<VerifyGetResponse>();

                if (responseContent == null)
                {
                    createResponse.ErrorMessages = new List<string> { "Response content is null." };
                }
                else
                {
                    createResponse.IsSuccessful = true;
                    createResponse.ErrorMessages = new List<string>(); // Clear any placeholder error message
                    createResponse.VLink = responseContent.VLink;
                }
            }
            else
            {
                var error = await httpResponse.Content.ReadAsStringAsync();
                createResponse.ErrorMessages = new List<string> { $"Request failed with status: {httpResponse.StatusCode}, Error: {error}" };
            }

            return createResponse;

        } // end of PublicVerifyAsync

        #endregion

        #region Item Note

        public async Task<WkycItemNoteGetAllForItemResponse> GetAllItemNotesForItemAsync(Guid itemId, string token)
        {
            var getResponse = new WkycItemNoteGetAllForItemResponse()
            {
                IsSuccessful = false,
                ErrorMessages = [""],
            };

            // Use the injected _http HttpClient instance
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var httpResponse = await _http.GetAsync($"{_baseUrl}/ItemNote");
            Console.WriteLine($"httpResponse: {JsonSerializer.Serialize(httpResponse)}");

            if (httpResponse.IsSuccessStatusCode)
            {
                // Deserialize JSON response into your data object
                var responseContent = await httpResponse.Content.ReadFromJsonAsync<ItemNoteGetAllForItemResponse>();

                if (responseContent == null)
                {
                    getResponse.ErrorMessages = new List<string> { "Response content is null." };
                }
                else
                {
                    getResponse.IsSuccessful = true;
                    getResponse.ErrorMessages = new List<string>(); // Clear any placeholder error message
                    getResponse.Notes = responseContent.Notes;
                }
            }
            else
            {
                var error = await httpResponse.Content.ReadAsStringAsync();
                getResponse.ErrorMessages = new List<string> { $"Request failed with status: {httpResponse.StatusCode}, Error: {error}" };
            }

            return getResponse;

        } // end of GetAllItemNoteForItemAsync

        public async Task<WkycItemNoteGetAllForItemResponse> GetAllItemNotesForItemAsync(Guid itemId)
        {
            string token = "";

            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userClaims = user.Claims;
                token = userClaims?.FirstOrDefault(c => c.Type == "Token")?.Value ?? "Unknown";
            }

            return await GetAllItemNotesForItemAsync(itemId, token);
        } // end of GetAllItemNoteForItemAsync

        public async Task<WkycItemNoteGetResponse> GetItemNoteAsync(Guid itemId, Guid itemNoteId, string token)
        {
            var getResponse = new WkycItemNoteGetResponse()
            {
                IsSuccessful = false,
                ErrorMessages = [""],
            };
            // Prepare the request with authorization header
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Construct the URL with the item ID and item note ID
            var url = $"{_baseUrl}/ItemNote/{itemId}/{itemNoteId}";

            // Execute the GET request
            var httpResponse = await _http.GetAsync(url);

            // Ensure successful response status
            httpResponse.EnsureSuccessStatusCode();

            if (httpResponse.Content == null)
            {
                _logger.LogError("Response content is null.");
                throw new InvalidOperationException("Response content is null.");
            }

            try
            {
                // Deserialize the response
                var responseContent = await httpResponse.Content.ReadFromJsonAsync<ItemNoteGetResponse>();

                if (httpResponse.IsSuccessStatusCode)
                {
                    if (responseContent == null)
                    {
                        getResponse.ErrorMessages = new List<string> { "Response content is null." };
                    }
                    else
                    {
                        getResponse.IsSuccessful = true;
                        getResponse.ErrorMessages = new List<string>(); // Clear any placeholder error message
                        getResponse.Note = responseContent.Note;
                    }
                }
                else
                {
                    var error = await httpResponse.Content.ReadAsStringAsync();
                    getResponse.ErrorMessages = new List<string> { $"Request failed with status: {httpResponse.StatusCode}, Error: {error}" };
                }

                return getResponse;
            }
            catch (JsonException jsonException)
            {
                _logger.LogError("JSON error: {JsonException}", jsonException);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching item note details: {Message}", ex.Message);
                throw;
            }
        } // end of GetVerifiedLinkDetailsAsync

        public async Task<WkycItemNoteGetResponse> GetItemNoteAsync(Guid itemId, Guid itemNoteId)
        {
            string token = "";

            // Retrieve the current user's token
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userClaims = user.Claims;
                token = userClaims?.FirstOrDefault(c => c.Type == "Token")?.Value ?? "Unknown";
            }
            else
            {
                _logger.LogError("User is not authenticated.");
                throw new InvalidOperationException("User is not authenticated.");
            }


            return await GetItemNoteAsync(itemId, itemNoteId, token);
        } // end of GetItemNoteAsync

        public async Task<WkycItemNoteUpdateResponse> UpdateItemNoteAsync(ItemNoteUpdateRequest request, string token)
        {
            var updateResponse = new WkycItemNoteUpdateResponse()
            {
                IsSuccessful = false,
                ErrorMessages = [""],
            };
            // Use the injected _http HttpClient instance
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var jsonPayload = JsonSerializer.Serialize(request);
            Console.WriteLine($"jsonPayload: {JsonSerializer.Serialize(jsonPayload)}");
            // Create HttpContent from JSON payload
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            Console.WriteLine($"content: {JsonSerializer.Serialize(content)}");
            var httpResponse = await _http.PatchAsync($"{_baseUrl}/ItemNote", content);
            Console.WriteLine($"httpResponse: {JsonSerializer.Serialize(httpResponse)}");

            if (httpResponse.IsSuccessStatusCode)
            {
                // Deserialize JSON response into your data object
                var responseContent = await httpResponse.Content.ReadFromJsonAsync<ItemNoteUpdateResponse>();

                if (responseContent == null)
                {
                    updateResponse.ErrorMessages = new List<string> { "Response content is null." };
                }
                else
                {
                    updateResponse.IsSuccessful = true;
                    updateResponse.ErrorMessages = new List<string>(); // Clear any placeholder error message
                }
            }
            else
            {
                var error = await httpResponse.Content.ReadAsStringAsync();
                updateResponse.ErrorMessages = new List<string> { $"Request failed with status: {httpResponse.StatusCode}, Error: {error}" };
            }

            return updateResponse;

        } // end of UpdateItemNoteAsync

        public async Task<WkycItemNoteUpdateResponse> UpdateItemNoteAsync(ItemNoteUpdateRequest request)
        {
            string token = "";
            var updateResponse = new WkycItemNoteUpdateResponse()
            {
                IsSuccessful = false,
                ErrorMessages = [""],
            };

            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userClaims = user.Claims;
                token = userClaims?.FirstOrDefault(c => c.Type == "Token")?.Value ?? "Unknown";
            }
            else
            {
                updateResponse.IsSuccessful = true;
                return updateResponse;
            }

            return await UpdateItemNoteAsync(request, token);
        } // end of UpdateItemNoteAsync



        public async Task<WkycItemNoteCreateResponse> CreateItemNoteAsync(ItemNoteCreateRequest request, string token)
        {
            var createResponse = new WkycItemNoteCreateResponse()
            {
                IsSuccessful = false,
                ErrorMessages = [""],
            };

            // Use the injected _http HttpClient instance
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            JsonSerializerOptions options = new()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
            };
            var jsonPayload = JsonSerializer.Serialize(request, options);
            Console.WriteLine($"jsonPayload: {JsonSerializer.Serialize(jsonPayload)}");
            // Create HttpContent from JSON payload
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            Console.WriteLine($"content: {JsonSerializer.Serialize(content)}");
            var httpResponse = await _http.PostAsync($"{_baseUrl}/ItemNote", content);
            Console.WriteLine($"httpResponse: {JsonSerializer.Serialize(httpResponse)}");

            if (httpResponse.IsSuccessStatusCode)
            {
                // Deserialize JSON response into your data object
                var responseContent = await httpResponse.Content.ReadFromJsonAsync<ItemNoteCreateResponse>();

                if (responseContent == null)
                {
                    createResponse.ErrorMessages = new List<string> { "Response content is null." };
                }
                else
                {
                    createResponse.IsSuccessful = true;
                    createResponse.ErrorMessages = new List<string>(); // Clear any placeholder error message
                                                                       //createResponse.Note = responseContent.Note;
                }
            }
            else
            {
                var error = await httpResponse.Content.ReadAsStringAsync();
                createResponse.ErrorMessages = new List<string> { $"Request failed with status: {httpResponse.StatusCode}, Error: {error}" };
            }

            return createResponse;

        } // end of CreateItemNoteAsync

        public async Task<WkycItemNoteCreateResponse> CreateItemNoteAsync(ItemNoteCreateRequest request)
        {
            string token = "";

            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userClaims = user.Claims;
                token = userClaims?.FirstOrDefault(c => c.Type == "Token")?.Value ?? "Unknown";
            }

            return await CreateItemNoteAsync(request, token);
        } // end of CreateItemNoteAsync

        public async Task<WkycItemNoteDeleteResponse> DeleteItemNoteAsync(ItemNoteDeleteRequest request, string token)
        {
            var deleteResponse = new WkycItemNoteDeleteResponse()
            {
                IsSuccessful = false,
                ErrorMessages = [""],
            };

            // Use the injected _http HttpClient instance
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            JsonSerializerOptions options = new()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
            };
            var jsonPayload = JsonSerializer.Serialize(request, options);
            Console.WriteLine($"jsonPayload: {JsonSerializer.Serialize(jsonPayload)}");
            // Create HttpContent from JSON payload
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            Console.WriteLine($"content: {JsonSerializer.Serialize(content)}");
            // Create the request message
            HttpRequestMessage requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{_baseUrl}/ItemNote"),
                Content = content
            };

            //var httpResponse = await _http.DeleteFromJsonAsync($"{_baseUrl}/VerifiedLink/Delete", content);
            var httpResponse = await _http.SendAsync(requestMessage);

            Console.WriteLine($"httpResponse: {JsonSerializer.Serialize(httpResponse)}");

            if (httpResponse.IsSuccessStatusCode)
            {
                // Deserialize JSON response into your data object
                var responseContent = await httpResponse.Content.ReadFromJsonAsync<ItemNoteDeleteResponse>();

                if (responseContent == null)
                {
                    deleteResponse.ErrorMessages = new List<string> { "Response content is null." };
                }
                else
                {
                    deleteResponse.IsSuccessful = true;
                    deleteResponse.ErrorMessages = new List<string>(); // Clear any placeholder error message
                }
            }
            else
            {
                var error = await httpResponse.Content.ReadAsStringAsync();
                deleteResponse.ErrorMessages = new List<string> { $"Request failed with status: {httpResponse.StatusCode}, Error: {error}" };
            }

            return deleteResponse;

        } // end of DeleteItemNoteAsync

        public async Task<WkycItemNoteDeleteResponse> DeleteItemNoteAsync(ItemNoteDeleteRequest request)
        {
            string token = "";

            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userClaims = user.Claims;
                token = userClaims?.FirstOrDefault(c => c.Type == "Token")?.Value ?? "Unknown";
            }

            return await DeleteItemNoteAsync(request, token);
        } // end of DeleteItemNoteAsync

        public async Task<WkycItemNoteDeleteResponse> DeleteItemNoteAsync(Guid itemId, Guid itemNoteId, string token)
        {
            var deleteResponse = new WkycItemNoteDeleteResponse()
            {
                IsSuccessful = false,
                ErrorMessages = [""],
            };

            var httpResponse = await _http.DeleteAsync($"{_baseUrl}/ItemNote/{itemId}/{itemNoteId}");

            Console.WriteLine($"httpResponse: {JsonSerializer.Serialize(httpResponse)}");

            if (httpResponse.IsSuccessStatusCode)
            {
                // Deserialize JSON response into your data object
                var responseContent = await httpResponse.Content.ReadFromJsonAsync<ItemNoteDeleteResponse>();

                if (responseContent == null)
                {
                    deleteResponse.ErrorMessages = new List<string> { "Response content is null." };
                }
                else
                {
                    deleteResponse.IsSuccessful = true;
                    deleteResponse.ErrorMessages = new List<string>(); // Clear any placeholder error message
                }
            }
            else
            {
                var error = await httpResponse.Content.ReadAsStringAsync();
                deleteResponse.ErrorMessages = new List<string> { $"Request failed with status: {httpResponse.StatusCode}, Error: {error}" };
            }

            return deleteResponse;

        } // end of DeleteItemNoteAsync

        public async Task<WkycItemNoteDeleteResponse> DeleteItemNoteAsync(Guid itemId, Guid itemNoteId)
        {
            string token = "";

            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userClaims = user.Claims;
                token = userClaims?.FirstOrDefault(c => c.Type == "Token")?.Value ?? "Unknown";
            }

            return await DeleteItemNoteAsync(itemId, itemNoteId, token);
        } // end of DeleteItemNoteAsync

        #endregion

        #region Utilities

        private string GetErrorMessageDetails(string errorResponse)
        {
            using JsonDocument doc = JsonDocument.Parse(errorResponse);

            // Access the messageDetails field
            JsonElement root = doc.RootElement;
            JsonElement problems = root.GetProperty("problems");
            JsonElement firstProblem = problems[0];
            string messageDetails = firstProblem.GetProperty("messageDetails").GetString() ?? string.Empty;
            _logger.LogInformation("messageDetails: {MessageDetails}", messageDetails);
            return messageDetails;
        }

        private string GetErrorMessageDetails(JsonElement problems)
        {
            string messageDetails = "";
            if (problems.ValueKind != JsonValueKind.Null && problems.GetArrayLength() > 0)
            {
                JsonElement firstProblem = problems[0];
                messageDetails = firstProblem.GetProperty("messageDetails").GetString() ?? string.Empty;
                _logger.LogInformation("messageDetails: {MessageDetails}", messageDetails);
            }
            return messageDetails;
        }

        private JsonElement GetProblems(string errorResponse)
        {
            JsonDocument doc = JsonDocument.Parse(errorResponse);

            // Access the messageDetails field
            JsonElement root = doc.RootElement;
            JsonElement problems = root.GetProperty("problems");
            _logger.LogInformation("GetProblems, problems: {Problems}", JsonSerializer.Serialize(problems));

            return problems;
        }

        #endregion

    } // end of Classs WKYCProdServiceHelper
} // end of namespace VO2Tech.Helper
