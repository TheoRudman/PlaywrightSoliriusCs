using Microsoft.Playwright;
using PlaywrightTests.Config;

namespace PlaywrightTests.Fixtures;

public class PlaywrightFixture : IAsyncLifetime
{
    private IPlaywright _pw = null!;     // Playwright engine instance
    private IBrowser _browser = null!;   // Shared browser instance
    private readonly TestSettings _settings;

    public PlaywrightFixture()
    {
        _settings = SettingsLoader.Load();   // Load settings from appsettings.json
    }

    public async Task InitializeAsync()
    {
        _pw = await Playwright.CreateAsync();   // Start Playwright

        // Launch browser using settings (headless/channel)
        _browser = await _pw.Chromium.LaunchAsync(new()
        {
            Headless = _settings.Headless,
            Channel = _settings.Channel
        });
    }

    public async Task DisposeAsync()
    {
        await _browser.CloseAsync();   // Close browser when tests finish
        _pw?.Dispose();                // Dispose Playwright engine
    }

    // Creates an isolated browser context and page per test (parallel-safe)
    public async Task<IPage> CreateIsolatedPageAsync()
    {
        var context = await _browser.NewContextAsync();   // New browser profile
        return await context.NewPageAsync();              // New page for the test
    }
}
