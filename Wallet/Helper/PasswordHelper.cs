using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using GPWebApi.DTO;

namespace Wallet.Helper
{
    public class PasswordHelper
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PasswordHelper> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly string _baseUrl;

        public PasswordHelper(HttpClient httpClient, ILogger<PasswordHelper> logger, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _baseUrl = _configuration["Win:Production:Url"] ?? "NO PROD URL"; // Null-coalescing for safety
        }
        public async Task<UserPasswordChangeResponse> ChangePasswordAsync(UserPasswordChangeRequest request)
        {
            var token = _httpContextAccessor.HttpContext?.User.FindFirst("Token")?.Value;
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogError("User token is missing.");
                throw new InvalidOperationException("User is not authenticated.");
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            // Log details
            _logger.LogInformation(
                "ChangePasswordAsync:\n<{UserId}>\n<{OldPassword}>\n<{NewPassword}>",
                request.UserId, request.OldPassword, request.NewPassword
            );
            _logger.LogInformation(
                "ChangePasswordAsync:Request\n {request}",
                request
            );

            // Send PATCH request
            HttpResponseMessage httpResponse = await _httpClient.PatchAsJsonAsync(
                $"{_baseUrl}/User/PasswordChange",
                request
            );
            _logger.LogInformation(
                "ChangePasswordAsync, httpResponse StatusCode: {StatusCode}",
                JsonSerializer.Serialize(httpResponse.StatusCode)
            );

            // Log headers
            var statusCode = httpResponse.StatusCode;
            var headers = httpResponse.Headers.ToDictionary(
                header => header.Key,
                header => string.Join(", ", header.Value)
            );
            var serializedHeaders = JsonSerializer.Serialize(headers);
            _logger.LogInformation(
                "ChangePasswordAsync, httpResponse StatusCode: {StatusCode}, Headers: {Headers}",
                statusCode,
                serializedHeaders
            );

            // Read the response content
            string responseContent = await httpResponse.Content.ReadAsStringAsync();
            _logger.LogInformation("Password change API response: {ResponseContent}", responseContent);

            // Build a response object
            var userPasswordChangeResponse = new UserPasswordChangeResponse();

            // 1. If there's no body, fallback to "Old password is incorrect" or similar
            if (string.IsNullOrWhiteSpace(responseContent))
            {
                _logger.LogError("API returned {Status} with NO JSON body.", statusCode);
                // Return a Problems list with your fallback message
                userPasswordChangeResponse.Problems = new List<Problem>
        {
            new Problem(
                1000211,
                ProblemType.Error,
                "Cannot change password",
                "Old password is incorrect",
                "",
                ""
            )
        };
                return userPasswordChangeResponse;
            }

            // 2. If there is content, parse the "problems" array manually
            try
            {
                using JsonDocument doc = JsonDocument.Parse(responseContent);
                if (doc.RootElement.TryGetProperty("problems", out var problemsElement)
                    && problemsElement.ValueKind == JsonValueKind.Array)
                {
                    var problemsList = new List<Problem>();
                    foreach (var problemElement in problemsElement.EnumerateArray())
                    {
                        int code = problemElement.GetProperty("problemCode").GetInt32();
                        var problemType = (ProblemType)Enum.Parse(
                            typeof(ProblemType),
                            problemElement.GetProperty("problemType").GetString() ?? "Error",
                            ignoreCase: true
                        );
                        string message = problemElement.GetProperty("message").GetString() ?? "";
                        string messageDetails = problemElement.GetProperty("messageDetails").GetString() ?? "";
                        string fieldName = problemElement.GetProperty("fieldName").GetString() ?? "";
                        string fieldValue = problemElement.GetProperty("fieldValue").GetString() ?? "";

                        problemsList.Add(new Problem(code, problemType, message, messageDetails, fieldName, fieldValue));
                    }
                    userPasswordChangeResponse.Problems = problemsList.Count > 0 ? problemsList : null;
                }
                else
                {
                    // If "problems" isn't found or is null, 
                    // we assume success or no problems
                    userPasswordChangeResponse.Problems = null;
                }
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "Failed to parse 'problems' from API response.");
                // If the body is some other JSON format, fallback or handle as needed
                userPasswordChangeResponse.Problems = null;
            }

            // Return the object with any parsed "problems"
            return userPasswordChangeResponse;
        }

    }
}
