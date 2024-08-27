using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using BitcoinTracker.Interfaces;
using Microsoft.Extensions.Logging;

namespace BitcoinTracker.Services;

[SuppressMessage("ReSharper", "ConvertToPrimaryConstructor")]
public class GetApiService : IGetApiService
{
    private string ApiString { get; set; } =
        "https://api.coingecko.com/api/v3/simple/price?ids=bitcoin&vs_currencies=usd";

    private readonly HttpClient _httpClient;
    private readonly ILogger<GetApiService> _logger;

    public GetApiService(HttpClient httpClient, ILogger<GetApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<string> CallBitcoinApi()
    {
        // Send GET request to the API
        _logger.LogInformation("Getting API data.");
        var response = await _httpClient.GetAsync(ApiString);
        response.EnsureSuccessStatusCode();

        // Read the response content as a string
        var responseBody = await response.Content.ReadAsStringAsync();

        return responseBody;
    }
}