using System.Net.Http.Headers;
using System.Net.Http.Json;
using StockQuoteAlert.Models;
using StockQuoteAlert.Settings;

namespace StockQuoteAlert.Services;

public class BrApiService(ApplicationSettings appSettings)
{
    private readonly ApplicationSettings _appSettings = appSettings;
    private readonly HttpClient _httpClient = new()
    {
        Timeout = TimeSpan.FromSeconds(10)
    };

    public async Task<StockInfo?> GetStockQuoteAsync(string stock)
    {
        string url = $"{_appSettings.BRAPI.BaseUrl}/{stock}";
        var request = new HttpRequestMessage(HttpMethod.Get, url)
        {
            Headers = {Authorization = new AuthenticationHeaderValue("Bearer", _appSettings.BRAPI.Key)},
        };

        using var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            Console.Error.WriteLine($"API request error. Status: {response.StatusCode}; Content: {await response.Content.ReadAsStringAsync()}");
            return null;
        }

        BrApiResponseModel? result = await response.Content.ReadFromJsonAsync<BrApiResponseModel>();
        if (result == null || result.Results.Length == 0)
        {
            Console.Error.WriteLine("API response is empty or invalid.");
            return null;
        }
        
        return result.Results.FirstOrDefault();
    }
}
