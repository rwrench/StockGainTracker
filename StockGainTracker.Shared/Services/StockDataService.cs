using System.Text.Json;
using StockGainTracker.Shared.Models;

namespace StockGainTracker.Shared.Services;

public class StockDataService
{
    private readonly HttpClient _http;

    public StockDataService(HttpClient http)
    {
        _http = http;
        // Yahoo blocks requests with no User-Agent
        if (!_http.DefaultRequestHeaders.Contains("User-Agent"))
        {
            _http.DefaultRequestHeaders.Add("User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
        }
    }

    public async Task<List<DailyGain>> GetGainsForRangeAsync(string ticker, DateTime from, DateTime to)
    {
        // Pull one extra day before 'from' so the first day in range has a prior close to compare against
        var period1 = new DateTimeOffset(from.AddDays(-7)).ToUnixTimeSeconds();
        var period2 = new DateTimeOffset(to.AddDays(1)).ToUnixTimeSeconds();

        var url = $"https://query1.finance.yahoo.com/v8/finance/chart/{ticker}" +
                   $"?period1={period1}&period2={period2}&interval=1d";

        var json = await _http.GetStringAsync(url);

        using var doc = JsonDocument.Parse(json);
        var result = doc.RootElement.GetProperty("chart").GetProperty("result")[0];

        var timestamps = result.GetProperty("timestamp")
            .EnumerateArray()
            .Select(t => DateTimeOffset.FromUnixTimeSeconds(t.GetInt64()).UtcDateTime)
            .ToList();

        var closes = result.GetProperty("indicators").GetProperty("quote")[0]
            .GetProperty("close")
            .EnumerateArray()
            .Select(c => c.ValueKind == JsonValueKind.Null ? (decimal?)null : c.GetDecimal())
            .ToList();

        var rows = timestamps.Zip(closes, (date, close) => (Date: date, Close: close))
            .Where(r => r.Close.HasValue)
            .OrderBy(r => r.Date)
            .ToList();

        var gains = new List<DailyGain>();
        for (int i = 1; i < rows.Count; i++)
        {
            if (rows[i].Date.Date < from.Date || rows[i].Date.Date > to.Date)
                continue;

            var prevClose = rows[i - 1].Close!.Value;
            var close = rows[i].Close!.Value;
            var pct = prevClose == 0 ? 0 : (close - prevClose) / prevClose * 100m;

            gains.Add(new DailyGain
            {
                Date = rows[i].Date,
                Close = close,
                PercentGain = Math.Round(pct, 2)
            });
        }

        return gains;
    }
}

 