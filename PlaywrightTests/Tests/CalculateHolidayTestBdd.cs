using Microsoft.Playwright;
using PlaywrightTests.HooksBdd;
using PlaywrightTests.Pages;
using TechTalk.SpecFlow;
using Xunit.Abstractions;

namespace PlaywrightTests.Tests;
[Binding]
public class CalculateHolidayTestBdd
{
    private readonly ScenarioContext _scenarioContext;
    private readonly CalculateHolidayPage _page;

    public CalculateHolidayTestBdd(ScenarioContext scenarioContext, ITestOutputHelper output)
    {
        _scenarioContext = scenarioContext;
        var page = _scenarioContext.Get<IPage>("Page");
        _page = new CalculateHolidayPage(page, TestSetup.Fixture.Settings.BaseUrl, output);
    }


    [Given(@"I am on the Calculate Holiday page")]
    public async Task GivenIAmOnTheCalculateHolidayPage()
    {
        await _page.SetupInitialPage(false);
    }

    [Given(@"I select Start Now")]
    public async Task GivenISelectStartNow()
    {
        await _page.ClickStartNow();
    }

    [Given(@"I set Irregular Hours to (.*)")]
    public async Task GivenISetIrregularHoursTo(bool irregularHours)
    {
        await _page.SetIrregularHours(irregularHours);
    }

    [When(@"I click change on row (.*)")]
    public async Task WhenIClickChangeOnRow(int row)
    {
        await _page.ClickChange(row);
    }

    [Then(@"I set Irregular Hours to (.*)")]
    public async Task ThenISetIrregularHoursTo(bool irregularHours)
    {
        await _page.SetIrregularHours(irregularHours);
    }
}