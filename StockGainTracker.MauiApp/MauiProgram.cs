using Microsoft.Extensions.Logging;
using StockGainTracker.MauiApp.Services;
using StockGainTracker.Shared.Services;

namespace StockGainTracker.MauiApp;

public static class MauiProgram
{
    public static global::Microsoft.Maui.Hosting.MauiApp CreateMauiApp()
    {
        var builder = global::Microsoft.Maui.Hosting.MauiApp.CreateBuilder();
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