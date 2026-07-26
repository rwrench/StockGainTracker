using Microsoft.Extensions.Logging;
using StockGainTracker.MauiApp.Services;
using StockGainTracker.Shared.Services;

namespace StockGainTracker.MauiApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();

        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddHttpClient<StockDataService>();
        builder.Services.AddScoped<ICsvExportService, ShareCsvExportService>();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
