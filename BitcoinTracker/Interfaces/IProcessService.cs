namespace BitcoinTracker.Interfaces;

public interface IProcessService
{
    decimal ProcessBitcoinPrice(string rawData);
}