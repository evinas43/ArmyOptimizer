using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using ArmyOptimizer.Models;

namespace ArmyOptimizer.Services
{
    public class SubscriptionsService
    {
        private readonly HttpClient _client;


        public SubscriptionsService(HttpClient client)
        {
            _client = client;
        }

        public async Task BuyTokens(int tokens)
        {
            var json = new StringContent(
                tokens.ToString(),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _client.PostAsync(
                "api/Payments/create-session",
                json
            );

            if (!response.IsSuccessStatusCode)
                throw new Exception("Error al crear sesión de pago");

            var content = await response.Content.ReadAsStringAsync();

            var url = JsonDocument.Parse(content)
                        .RootElement
                        .GetProperty("url")
                        .GetString();

            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
        public async Task <List<PaymentHistory?>> GetPayments() 
        {
            var response = await _client.GetAsync("api/Payments/history");

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<List<PaymentHistory>>();
        }
    }
}