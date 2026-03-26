using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using ArmyOptimizer.Models;

namespace ArmyOptimizer.Services
{
    public class UserService
    {
        private readonly HttpClient _client;

        public UserService(HttpClient client)
        {
            _client = client;
        }

        public async Task<int> GetTokensAsync()
        {
            var response = await _client.GetAsync("api/users/MyTokens");

            if (!response.IsSuccessStatusCode)
                return 0;

            var result = await response.Content.ReadFromJsonAsync<Tokens>();

            return result?.tokens ?? 0;
        }

        public async Task<object?> ME() { 
            var response = await _client.GetAsync("api/users/me");
            if (!response.IsSuccessStatusCode)
                return null;
            return await response.Content.ReadFromJsonAsync<object>();
        }
    }
}
