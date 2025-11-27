using Microsoft.Playwright;

namespace PlaywrightTests.Fixtures;

public abstract class TestBase : IClassFixture<PlaywrightFixture>
{
    protected IPage Page { get; }

    protected TestBase(PlaywrightFixture fixture)
    {
        // Each test gets its own fresh page/context via the fixture
        Page = fixture.CreateIsolatedPageAsync().GetAwaiter().GetResult();
    }
}
