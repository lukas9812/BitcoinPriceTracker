namespace BitcoinTracker.Models;

public class AppSettings
{
    public bool IsSoundAlertEnabled { get; init; }
    
    public string OutputCurrency { get; init; } = string.Empty;
}