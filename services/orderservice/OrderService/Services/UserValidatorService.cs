using System;

namespace OrderService.Services;

public class UserValidatorService
{

    private readonly HttpClient _http;

    public UserValidatorService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("userservice");
    }

    public async Task<bool> UserExists(int userId)
    {
        var response = await _http.GetAsync($"/api/users/{userId}");

        return response.IsSuccessStatusCode;
    }

}
