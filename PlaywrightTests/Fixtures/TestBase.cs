using Microsoft.Playwright;

namespace PlaywrightTests.Fixtures;

public abstract class TestBase : IClassFixture<PlaywrightFixture>
{
    protected IPage Page { get; }
    protected string BaseUrl { get; }

    protected TestBase(PlaywrightFixture fixture)
    {
        Page = fixture.CreateIsolatedPageAsync().GetAwaiter().GetResult();
        BaseUrl = fixture.Settings.BaseUrl;   // ← MUST HAVE THIS!
    }
}