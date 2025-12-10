using Microsoft.Playwright;
using PlaywrightTests.Fixtures;
using TechTalk.SpecFlow;

namespace PlaywrightTests.HooksBdd;

[Binding]
public class TestSetup
{
    public static PlaywrightFixture Fixture { get; private set; } = null!;

    private readonly ScenarioContext _scenarioContext;

    public TestSetup(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [BeforeTestRun]
    public static void BeforeTestRun()
    {
        Fixture = new PlaywrightFixture();
        Fixture.InitializeAsync().GetAwaiter().GetResult();
    }

    [BeforeScenario]
    public async Task BeforeScenario()
    {
        var page = await Fixture.CreateIsolatedPageAsync();
        _scenarioContext.Set(page, "Page");
    }

    [AfterScenario]
    public async Task AfterScenario()
    {
        if (_scenarioContext.TryGetValue<IPage>("Page", out var page))
        {
            await page.CloseAsync();
        }
    }
}