using Microsoft.Playwright;
using PlaywrightTests.Config;

namespace PlaywrightTests.Fixtures;

// Global test fixture - executed once per test class.
// Centralises browser setup so tests do not manually configure Playwright.
public class PlaywrightFixture : IAsyncLifetime
{
    private IPlaywright _pw = null!;
    private IBrowser _browser = null!;

    // Loads config via appsettings.json (loose coupling to environment)
    public TestSettings Settings {get; }

    public PlaywrightFixture()
    {
        Settings = SettingsLoader.Load(); // Pull configuration on startup
    }

    public async Task InitializeAsync()
    {
        _pw = await Playwright.CreateAsync();

        // Launch browser using injected config values
        _browser = await _pw.Chromium.LaunchAsync(new()
        {
            Headless = Settings.Headless,
            Channel = Settings.Channel
        });
    }

    // Called after test run completes
    public async Task DisposeAsync()
    {
        await _browser.CloseAsync(); // Clean shutdown to prevent resource locking
        _pw?.Dispose();
    }

    // Creates a fresh page instance per test to ensure isolation
    // (cookies/sessions/state do not leak across tests)
    public async Task<IPage> CreateIsolatedPageAsync()
    {
        var context = await _browser.NewContextAsync();
        return await context.NewPageAsync();
    }
}