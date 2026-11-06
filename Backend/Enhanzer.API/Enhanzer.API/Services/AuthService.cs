using System.Net.Http.Json;
using Enhanzer.API.DTOs;

namespace Enhanzer.API.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AuthService> _logger;

        private const string LoginUrl =
            "https://ez-staging-api.azurewebsites.net/api/External_Api/POS_Api/Invoke";

        public AuthService(HttpClient httpClient, ILogger<AuthService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<HttpResponseMessage> LoginAsync(
            string email,
            string password)
        {
            _logger.LogInformation("Sending external login request to {LoginUrl} for company: {CompanyCode}", LoginUrl, email);

            var request = new EnhanzerLoginRequest
            {
                Company_Code = email,

                API_Body = new EnhanzerLoginBody
                {
                    Username = email,
                    Pw = password
                }
            };

            var response = await _httpClient.PostAsJsonAsync(
                LoginUrl,
                request
            );

            _logger.LogInformation("Received external login response with HTTP status code: {StatusCode}", response.StatusCode);

            return response;
        }
    }
}
