using System.Net.Http;
using System.Net.Http.Json;
using ArmyOptimizer.Models;

namespace ArmyOptimizer.Services
{
    public class AuthService
    {
        private readonly HttpClient _client;

        public AuthService(HttpClient client)
        {
            _client = client;
        }

        public async Task<string?> LoginAsync(string username, string password)
        {
            var response = await _client.PostAsJsonAsync(
                "api/users/login",
                new { username, password });

            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            return result?.token;
        }

        public async Task<bool> RegisterAsync(string username, string password)
        {
            var response = await _client.PostAsJsonAsync(
                "api/users/register",
                new { username, password });

            return response.IsSuccessStatusCode;
        }
    }
}