
using System.Net.Http.Json;
using Enhanzer.API.DTOs;

namespace Enhanzer.API.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;

        private const string LoginUrl =
            "https://ez-staging-api.azurewebsites.net/api/External_Api/POS_Api/Invoke";

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> LoginAsync(
            string email,
            string password)
        {
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

            return response;
        }
    }
}

