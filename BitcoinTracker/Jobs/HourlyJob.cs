using System.Diagnostics.CodeAnalysis;
using BitcoinTracker.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Toolkit.Uwp.Notifications;

namespace BitcoinTracker.Jobs;

[SuppressMessage("ReSharper", "ConvertToPrimaryConstructor")]
public class HourlyJob : BackgroundService
{
    private readonly IGetApiService _apiService;
    private readonly IProcessService _processService;

    public HourlyJob(IGetApiService apiService, IProcessService processService)
    {
        _apiService = apiService;
        _processService = processService;
    }

    public async Task Trigger()
    {
        //Only for testing purpose!
        //var sec = TimeSpan.FromSeconds(5);
        var hour = TimeSpan.FromHours(1);
        
        while (true)
        {
            await ExecuteAsync(default);
            await Task.Delay(hour);
        }
        // ReSharper disable once FunctionNeverReturns
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var rawData = await _apiService.CallBitcoinApi();
        var price = _processService.ProcessBitcoinPrice(rawData);
        
        var imageUri = Path.GetFullPath(@"Images\btc_logo.png");
        var audioUri = Path.GetFullPath(@"Sounds\sound_money.mp3");
        
        if(!File.Exists(audioUri))
            Console.WriteLine("File for audio do not exists!");
        if(!File.Exists(imageUri))
            Console.WriteLine("File for image do not exists!");
        if(price == 00)
            Console.WriteLine("Cannot get data from API!");
            
        new ToastContentBuilder()
            .AddAppLogoOverride(new Uri(imageUri))
            .AddAudio(new Uri(audioUri))
            .AddText("BITCOIN PRICE NOTIFICATION")
            .AddText($"Current Bitcoin price is: {price} USD.")
            .Show();
    }
}
