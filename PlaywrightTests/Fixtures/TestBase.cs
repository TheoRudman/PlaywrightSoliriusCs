using Microsoft.Playwright;
using PlaywrightTests.Fixtures;
using Xunit.Abstractions;

public abstract class TestBase : IClassFixture<PlaywrightFixture>, IAsyncLifetime
{
    protected IPage Page { get; }
    protected IBrowserContext Context { get; }
    protected string BaseUrl { get; }

    private readonly PlaywrightFixture _fixture;
    private readonly ITestOutputHelper _output;

    protected TestBase(PlaywrightFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;

        Page = fixture.CreateIsolatedPageAsync().GetAwaiter().GetResult();
        Context = Page.Context;

        BaseUrl = fixture.Settings.BaseUrl;
    }

    public async Task InitializeAsync()
    {
        await Context.Tracing.StartAsync(new()
        {
            Screenshots = true,
            Snapshots = true,
            Sources = true
        });
    }

    public async Task DisposeAsync()
    {
        string testName = GetTestName();
        string tracePath = Path.Combine("traces", $"{testName}.zip");

        Directory.CreateDirectory("traces");
        await Context.Tracing.StopAsync(new() { Path = tracePath });

        await Context.CloseAsync();
    }

    private string GetTestName()
    {
        // xUnit hides this internally → we read the private field via reflection
        var type = _output.GetType();
        var testMember = type.GetField("test", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (testMember?.GetValue(_output) is { } testObj)
        {
            var testNameProp = testObj.GetType().GetProperty("DisplayName");
            var testName = (string)testNameProp?.GetValue(testObj) ?? "UnknownTest";

            return testName.Replace(" ", "_").Replace(":", "_");
        }

        return $"UnknownTest_{Guid.NewGuid()}";
    }
}