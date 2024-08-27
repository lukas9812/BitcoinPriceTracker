namespace BitcoinTracker.Interfaces;

public interface IProcessService
{
    decimal GetBitcoinPriceInVariousCurrencies(string rawData);
}