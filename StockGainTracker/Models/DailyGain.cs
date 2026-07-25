namespace StockGainTracker.Models;

public class DailyGain
{
    public DateTime Date { get; set; }
    public decimal Close { get; set; }
    public decimal PercentGain { get; set; }
}