using System.Diagnostics.CodeAnalysis;
using BitcoinTracker.Interfaces;
using BitcoinTracker.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BitcoinTracker.Services;

[SuppressMessage("ReSharper", "ConvertToPrimaryConstructor")]
public class GetApiService : IGetApiService
{
    private string ApiString(string currency) =>
        $"https://api.coingecko.com/api/v3/simple/price?ids=bitcoin&vs_currencies={currency}";
        
    private readonly HttpClient _httpClient;
    private readonly ILogger<GetApiService> _logger;
    private readonly AppSettings _appSettings;

    public GetApiService(HttpClient httpClient, ILogger<GetApiService> logger, IOptions<AppSettings> settings)
    {
        _httpClient = httpClient;
        _logger = logger;
        _appSettings = settings.Value;
    }

    public async Task<string> CallBitcoinApi()
    {
        // Send GET request to the API
        _logger.LogInformation("Getting API data.");
        var response = await _httpClient.GetAsync(ApiString(_appSettings.OutputCurrency));
        response.EnsureSuccessStatusCode();

        // Read the response content as a string
        var responseBody = await response.Content.ReadAsStringAsync();

        return responseBody;
    }
}