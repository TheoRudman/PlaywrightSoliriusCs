using PlaywrightTests.Fixtures;
using PlaywrightTests.Pages;
using Xunit.Abstractions;

namespace PlaywrightTests.Tests;

public class FirstTest(PlaywrightFixture fixture, ITestOutputHelper output) : TestBase(fixture, output)
{
    [Fact]
    public async Task RejectCookiesBanner()
    {
        var firstPage = new FirstPage(Page, BaseUrl);
        await firstPage.SetupInitialPage(false);
    }
    
    [Fact]
    public async Task ChangeIrregularHours()
    {
        var firstPage = new FirstPage(Page, BaseUrl);
        await firstPage.SetupInitialPage();
        await firstPage.ClickStartNow();
        await firstPage.SetIrregularHours(true);
        await firstPage.ClickChange(0);
        await firstPage.SetIrregularHours(false);
    }

    [Fact]
    public async Task VerifyStartAgain()
    {
        var firstPage = new FirstPage(Page, BaseUrl);
        await firstPage.SetupInitialPage();
        await firstPage.ClickStartNow();
        await firstPage.SetIrregularHours(false);
        await firstPage.StartAgain();
    }

    [Fact]
    public async Task InvalidLeaveYear()
    {
        var firstPage = new FirstPage(Page, BaseUrl);
        await firstPage.SetupInitialPage();
        await firstPage.ClickStartNow();
        await firstPage.SetIrregularHours(true);
        await firstPage.SetLeaveYear("99", "99", "9999", false);
        await firstPage.ValidateErrorMessage();
    }

    [Fact]
    public async Task IrregularHoursWithDaysPerWeekLeavingPartWayThroughLeaveYear()
    {
        var firstPage = new FirstPage(Page, BaseUrl);
        await firstPage.SetupInitialPage();
        await firstPage.ClickStartNow();
        await firstPage.SetIrregularHours(true);
        await firstPage.SetLeaveYear("9", "9", "2009", true);
        await firstPage.SetHolidayEntitlement(0);
        await firstPage.SetWorkOutHoliday(2);
        await firstPage.SetEmploymentDate("3", "3", "2010", true);
        await firstPage.SetNumberOfDaysWorked("5.0", true);
        await firstPage.VerifyCompleted("13.6");
    }

    [Fact]
    public async Task InvalidEmploymentDate()
    {
        var firstPage = new FirstPage(Page, BaseUrl);
        await firstPage.SetupInitialPage();
        await firstPage.ClickStartNow();
        await firstPage.SetIrregularHours(true);
        await firstPage.SetLeaveYear("9", "9", "2009", true);
        await firstPage.SetHolidayEntitlement(0);
        await firstPage.SetWorkOutHoliday(2);
        await firstPage.SetEmploymentDate("39", "39", "9999", false);
    }

    [Fact]
    public async Task InvalidNumberOfDaysWorked()
    {
        var firstPage = new FirstPage(Page, BaseUrl);
        await firstPage.SetupInitialPage();
        await firstPage.ClickStartNow();
        await firstPage.SetIrregularHours(true);
        await firstPage.SetLeaveYear("9", "9", "2009", true);
        await firstPage.SetHolidayEntitlement(0);
        await firstPage.SetWorkOutHoliday(2);
        await firstPage.SetEmploymentDate("3", "3", "2010", true);
        await firstPage.SetNumberOfDaysWorked("99", false);
    }
}