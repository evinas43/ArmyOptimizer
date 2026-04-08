using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace ArmyOptimizer.Services
{
    public class SubscriptionsService
    {
        public async Task BuyTokens(int tokens)
        {
            var json = new StringContent(
                tokens.ToString(),
                Encoding.UTF8,
                "application/json"
            );

            var response = await HttpService.Client.PostAsync(
                "api/payments/create-session",
                json
            );

            if (!response.IsSuccessStatusCode)
                throw new Exception("Error al crear sesión de pago");

            var content = await response.Content.ReadAsStringAsync();

            var url = JsonDocument.Parse(content)
                        .RootElement
                        .GetProperty("url")
                        .GetString();

            // 🔥 abrir navegador
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
    }
}