using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using BitcoinTracker.Interfaces;

namespace BitcoinTracker.Services;

[SuppressMessage("ReSharper", "ConvertToPrimaryConstructor")]
public class GetApiService : IGetApiService
{
    private string ApiString { get; set; } =
        "https://api.coingecko.com/api/v3/simple/price?ids=bitcoin&vs_currencies=usd";

    private readonly HttpClient _httpClient;

    public GetApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> CallBitcoinApi()
    {
        // Send GET request to the API
        var response = await _httpClient.GetAsync(ApiString);
        response.EnsureSuccessStatusCode();

        // Read the response content as a string
        var responseBody = await response.Content.ReadAsStringAsync();

        return responseBody;
    }
}