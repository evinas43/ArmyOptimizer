using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using ArmyOptimizer.Models;

namespace ArmyOptimizer.Services
{
    public class AiOptimizerService
    {
        private readonly HttpClient _client;

        public AiOptimizerService(HttpClient client)
        {
            _client = client;
        }

        public async Task<AiArmyResponse?> OptimizeArmy(object army)
        {
            var response = await _client.PostAsJsonAsync(
                "api/ArmyOptimize/Optimize",
                army);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<AiArmyResponse>();
        }
    }
}