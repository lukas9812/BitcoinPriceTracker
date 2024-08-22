namespace BitcoinTracker.Interfaces;

public interface IGetApiService
{
    Task<string> CallBitcoinApi();
}