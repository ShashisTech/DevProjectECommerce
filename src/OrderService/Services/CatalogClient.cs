using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using OrderService.Models;

namespace OrderService.Services
{
    public interface ICatalogClient
    {
        Task<ProductDto?> GetProductAsync(int id);
        Task<CategoryDto?> GetCategoryAsync(int id);
        Task<(ProductDto? Product, bool Reserved, int StatusCode)> ReserveProductAsync(int id, int qty);
    }

    public class CatalogClient : ICatalogClient
    {
        private readonly IHttpClientFactory _factory;

        public CatalogClient(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public async Task<ProductDto?> GetProductAsync(int id)
        {
            var client = _factory.CreateClient("catalog");
            try
            {
                return await client.GetFromJsonAsync<ProductDto?>("api/products/" + id);
            }
            catch
            {
                return null;
            }
        }

        public async Task<(ProductDto? Product, bool Reserved, int StatusCode)> ReserveProductAsync(int id, int qty)
        {
            var client = _factory.CreateClient("catalog");
            try
            {
                var resp = await client.PostAsJsonAsync($"api/products/{id}/reserve", new { Quantity = qty });
                if (resp.IsSuccessStatusCode)
                {
                    var prod = await resp.Content.ReadFromJsonAsync<ProductDto?>();
                    return (prod, true, (int)resp.StatusCode);
                }
                else
                {
                    return (null, false, (int)resp.StatusCode);
                }
            }
            catch
            {
                return (null, false, 0);
            }
        }

        public async Task<CategoryDto?> GetCategoryAsync(int id)
        {
            var client = _factory.CreateClient("catalog");
            try
            {
                return await client.GetFromJsonAsync<CategoryDto?>("api/categories/" + id);
            }
            catch
            {
                return null;
            }
        }
    }
}
