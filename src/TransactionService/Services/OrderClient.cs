using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TransactionService.Models;

namespace TransactionService.Services
{
    public class OrderClient : IOrderClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public OrderClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["OrderServiceUrl"] ?? "http://localhost:63054/api";
        }

        public async Task<OrderDto?> GetOrderAsync(int orderId)
        {
            var url = $"{_baseUrl}/orders/{orderId}";
            var resp = await _httpClient.GetAsync(url);
            if (!resp.IsSuccessStatusCode)
            {
                return null;
            }

            return await resp.Content.ReadFromJsonAsync<OrderDto>();
        }
    }
}
