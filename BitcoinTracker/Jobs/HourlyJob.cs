using System.Diagnostics.CodeAnalysis;
using BitcoinTracker.Interfaces;
using BitcoinTracker.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Toolkit.Uwp.Notifications;

namespace BitcoinTracker.Jobs;

[SuppressMessage("ReSharper", "ConvertToPrimaryConstructor")]
public class HourlyJob : BackgroundService
{
    private readonly IGetApiService _apiService;
    private readonly IProcessService _processService;
    private readonly ILogger<HourlyJob> _logger;
    private readonly AppSettings _settings;

    public HourlyJob(IGetApiService apiService, 
    IProcessService processService, 
    ILogger<HourlyJob> logger,
    IOptions<AppSettings> settings)
    {
        _apiService = apiService;
        _processService = processService;
        _logger = logger;
        _settings = settings.Value;
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
            _logger.LogInformation("Wait for 1 hour.");
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
            _logger.LogInformation("File for audio do not exists!");
            
        if(!File.Exists(imageUri))
            _logger.LogInformation("File for image do not exists!");
        
        if (price == 00)
        {
            _logger.LogInformation("Cannot get data from API!");
            new ToastContentBuilder()
                .AddAppLogoOverride(new Uri(imageUri))
                .AddText("FAULT BITCOIN PRICE NOTIFICATION")
                .AddText("Cannot reach bitcoin price API!")
                .Show();
        }
        else
        {
            if (_settings.IsSoundAlertEnabled)
            {
                _logger.LogInformation("Displaying toast content notification WITH sound.");
                new ToastContentBuilder()
                    .AddAppLogoOverride(new Uri(imageUri))
                    .AddAudio(new Uri(audioUri))
                    .AddText("BITCOIN PRICE NOTIFICATION")
                    .AddText($"Current Bitcoin price is: {price} USD.")
                    .Show();
            }
            else
            {
                _logger.LogInformation("Displaying toast content notification WITHOUT a sound.");
                new ToastContentBuilder()
                    .AddAppLogoOverride(new Uri(imageUri))
                    .AddText("BITCOIN PRICE NOTIFICATION")
                    .AddText($"Current Bitcoin price is: {price} USD.")
                    .Show();
            }
            
        }
    }
}
