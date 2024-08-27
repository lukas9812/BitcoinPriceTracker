using System.Diagnostics.CodeAnalysis;
using BitcoinTracker.Interfaces;
using BitcoinTracker.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BitcoinTracker.Services;

[SuppressMessage("ReSharper", "ConvertToPrimaryConstructor")]
public class ProcessService : IProcessService
{
    private readonly ILogger<ProcessService> _logger;

    public ProcessService(ILogger<ProcessService> logger)
    {
        _logger = logger;
    }

    public decimal ProcessBitcoinPrice(string rawData)
    {
        _logger.LogInformation("Deserializing raw API data.");
        var bitcoinPrice = JsonConvert.DeserializeObject<BitcoinPrice>(rawData);
        return bitcoinPrice == null ? 00 : bitcoinPrice.Bitcoin.Usd;
    }
}