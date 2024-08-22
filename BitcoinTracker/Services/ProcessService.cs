using System.Diagnostics.CodeAnalysis;
using BitcoinTracker.Interfaces;
using BitcoinTracker.Models;
using Newtonsoft.Json;

namespace BitcoinTracker.Services;

[SuppressMessage("ReSharper", "ConvertToPrimaryConstructor")]
public class ProcessService : IProcessService
{
    public decimal ProcessBitcoinPrice(string rawData)
    {
        var bitcoinPrice = JsonConvert.DeserializeObject<BitcoinPrice>(rawData);
        return bitcoinPrice == null ? 00 : bitcoinPrice.Bitcoin.Usd;
    }
}