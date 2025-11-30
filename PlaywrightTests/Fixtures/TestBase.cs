using Microsoft.Playwright;
using PlaywrightTests.Fixtures;
using Xunit.Abstractions;

namespace PlaywrightTests.Fixtures;

public abstract class TestBase : IClassFixture<PlaywrightFixture>
{
    protected IPage Page { get; }
    protected IBrowserContext Context { get; }
    protected string BaseUrl { get; }

    protected TestBase(PlaywrightFixture fixture)
    {
        Page = fixture.CreateIsolatedPageAsync().GetAwaiter().GetResult();
        BaseUrl = fixture.Settings.BaseUrl;   // ← MUST HAVE THIS!
    }

}