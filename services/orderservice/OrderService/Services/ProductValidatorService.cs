using System;

namespace OrderService.Services;

public class ProductValidatorService
{
    private readonly HttpClient _http;

    public ProductValidatorService(IHttpClientFactory http)
    {
        _http = http.CreateClient("productservice");
    }

    public async Task<bool> ProductExists(int productId)
    {
        var response = await _http.GetAsync($"/api/products/{productId}");

        return response.IsSuccessStatusCode;
    }
}
