
# StockGainTracker

Tracks day-by-day percentage gain/loss for a stock ticker using Yahoo Finance's public chart API, displayed as a color-coded table with CSV export.

## Projects

- **`StockGainTracker`** — ASP.NET Core Blazor Server web app (the original host).
- **`StockGainTracker.Shared`** — Razor Class Library containing the shared UI page, the `StockDataService` (Yahoo Finance client), the `DailyGain` model, and the `ICsvExportService` abstraction used by both hosts below.
- **`StockGainTracker.MauiApp`** — .NET MAUI Blazor Hybrid app, installable as a native app on Android (and Windows) with no server dependency at runtime.

## Running the web app

```
dotnet run --project StockGainTracker\StockGainTracker.csproj
```

Opens on a `localhost` URL. CSV export triggers a normal browser download.

## Running the MAUI app

**Android** (phone connected via USB with Developer Options / USB debugging enabled):
```
dotnet build -t:Run -f net10.0-android
```
(run from `StockGainTracker.MauiApp\`, or use Visual Studio with the project set as startup and your device selected)

**Windows** (unpackaged desktop build):
```
dotnet build -t:Run -f net10.0-windows10.0.19041.0
```

CSV export triggers the OS's native share sheet on Android, or share flyout on Windows.

## CI

GitHub Actions (`.github/workflows/build.yml`) builds the web project, the MAUI Android target, and the MAUI Windows target on every push.

Let me know if you'd 