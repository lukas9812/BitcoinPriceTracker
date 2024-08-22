using BitcoinTracker.Interfaces;
using Microsoft.Toolkit.Uwp.Notifications;

namespace BitcoinTracker.Services;
using Microsoft.Extensions.Hosting;


public class TriggerService : BackgroundService
{
    private readonly IProcessService _processService;
    private readonly IGetApiService _apiService;

    public TriggerService(IProcessService processService, IGetApiService apiService)
    {
        _processService = processService;
        _apiService = apiService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var rawData = await _apiService.CallBitcoinApi();
        var price = _processService.ProcessBitcoinPrice(rawData);
        
        var imageUri = Path.GetFullPath(@"Images\btc_logo.png");

        new ToastContentBuilder()
            .AddAppLogoOverride(new Uri(imageUri))
            .AddText("BITCOIN PRICE NOTIFICATION")
            .AddText($"Current Bitcoin price is: {price} USD.")
            .Show();
    }
}