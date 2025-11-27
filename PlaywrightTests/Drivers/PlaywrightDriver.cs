using Microsoft.Playwright;
using PlaywrightTests.Config;

namespace PlaywrightTests.Drivers;

public static class PlaywrightDriver
{
    public static async Task<IPage> CreatePageAsync(TestSettings settings)
    {
        var pw = await Playwright.CreateAsync();

        var browser = await pw.Chromium.LaunchAsync(new()
        {
            Headless = settings.Headless,
            Channel = settings.Channel
        });

        var context = await browser.NewContextAsync();
        return await context.NewPageAsync();
    }
}