using StockGainTracker.Shared.Services;

namespace StockGainTracker.MauiApp.Services;

public class ShareCsvExportService : ICsvExportService
{
    public async Task ExportAsync(string suggestedFileName, string csvContent)
    {
        var path = Path.Combine(FileSystem.CacheDirectory, suggestedFileName);
        await File.WriteAllTextAsync(path, csvContent);
        await Share.Default.RequestAsync(new ShareFileRequest
        {
            Title = "Export CSV",
            File = new ShareFile(path)
        });
    }
}
