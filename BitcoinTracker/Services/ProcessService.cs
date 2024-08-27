using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using BitcoinTracker.Interfaces;
using BitcoinTracker.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BitcoinTracker.Services;

[SuppressMessage("ReSharper", "ConvertToPrimaryConstructor")]
public class ProcessService : IProcessService
{
    private readonly ILogger<ProcessService> _logger;
    private readonly AppSettings _appSettings;

    public ProcessService(ILogger<ProcessService> logger, IOptions<AppSettings> appSettings)
    {
        _logger = logger;
        _appSettings = appSettings.Value;
    }

    public decimal GetBitcoinPriceInVariousCurrencies(string rawData)
    {
        _logger.LogInformation("Parse json raw data.");
        using var doc = JsonDocument.Parse(rawData);
        var bitcoinElement = doc.RootElement.GetProperty("bitcoin");

        var currency = _appSettings.OutputCurrency;
        _logger.LogInformation($"Output currency is set to {currency.ToUpper()}");

        var currencyValue =
            bitcoinElement.TryGetProperty(currency, out var currencyValueJsonElement)
            ? currencyValueJsonElement.GetDecimal()
            : 00;

        return currencyValue;

    }
}