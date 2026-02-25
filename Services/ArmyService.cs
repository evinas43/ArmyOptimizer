using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Windows;
using ArmyOptimizer.Models;

namespace ArmyOptimizer.Services
{
    public class ArmyService
    {
        private readonly HttpClient _client;

        public ArmyService(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<ArmySummary>?> GetUserArmiesAsync()
        {
            var response = await _client.GetAsync("api/users/userArmys");

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<List<ArmySummary>>();
        }

        public async Task<Army>? GetUserArmiesByIDAsync(int id)
        {
            var response = await _client.GetAsync($"api/users/userArmys/{id}");

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<Army>();
        }
    }
}