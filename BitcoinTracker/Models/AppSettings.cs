namespace BitcoinTracker.Models;

public class AppSettings
{
    public bool IsSoundAlertEnabled { get; init; }
    
    public string ToCurrency { get; init; } = string.Empty;
}