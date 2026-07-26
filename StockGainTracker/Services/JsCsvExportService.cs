using Microsoft.JSInterop;
using StockGainTracker.Shared.Services;

namespace StockGainTracker.Services;

public class JsCsvExportService(IJSRuntime js) : ICsvExportService
{
    public Task ExportAsync(string suggestedFileName, string csvContent)
        => js.InvokeVoidAsync("downloadCsv", suggestedFileName, csvContent).AsTask();
}
