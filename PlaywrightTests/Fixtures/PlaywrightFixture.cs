using Microsoft.Playwright;
using PlaywrightTests.Config;

namespace PlaywrightTests.Fixtures;

public class PlaywrightFixture : IAsyncLifetime
{
    private IPlaywright _pw = null!;
    private IBrowser _browser = null!;
    public TestSettings Settings {get; }

    public PlaywrightFixture()
    {
        Settings = SettingsLoader.Load();
    }

    public async Task InitializeAsync()
    {
        _pw = await Playwright.CreateAsync();
        _browser = await _pw.Chromium.LaunchAsync(new()
        {
            Headless = Settings.Headless,
            Channel = Settings.Channel
        });
    }

    public async Task DisposeAsync()
    {
        await _browser.CloseAsync();
        _pw?.Dispose();
    }

    public async Task<IPage> CreateIsolatedPageAsync()
    {
        var context = await _browser.NewContextAsync();
        return await context.NewPageAsync();
    }
}
