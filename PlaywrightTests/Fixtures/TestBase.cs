using Microsoft.Playwright;

namespace PlaywrightTests.Fixtures;

// Base class inherited by all test files
// Keeps test code clean by handling page setup & URL injection
public abstract class TestBase : IClassFixture<PlaywrightFixture>
{
    protected IPage Page { get; }
    protected string BaseUrl { get; }

    protected TestBase(PlaywrightFixture fixture)
    {
        // Create new isolated browser page per test for reliability.
        Page = fixture.CreateIsolatedPageAsync().GetAwaiter().GetResult();

        // Store base URL for navigation inside POM classes
        BaseUrl = fixture.Settings.BaseUrl;
    }
}