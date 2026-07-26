namespace StockGainTracker.Shared.Services;

public interface ICsvExportService
{
    Task ExportAsync(string suggestedFileName, string csvContent);
}
