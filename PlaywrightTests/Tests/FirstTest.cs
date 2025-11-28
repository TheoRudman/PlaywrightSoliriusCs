using Microsoft.Playwright;
using PlaywrightTests.Fixtures;
using PlaywrightTests.Pages;
using Xunit;

namespace PlaywrightTests.Tests;

public class FirstTest(PlaywrightFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task RejectCookiesBanner()
    {
        var firstPage = new FirstPage(Page, BaseUrl);
        await firstPage.SetupInitialPage(false);
    } 
    
    [Fact]
    public async Task NoIrregularHours()
    {
        var firstPage = new FirstPage(Page, BaseUrl);
        await firstPage.SetupInitialPage();
        await firstPage.ClickStartNow();
        await firstPage.SetIrregularHours(false);
    }
    
    [Fact]
    public async Task YesIrregularHours()
    {
        var firstPage = new FirstPage(Page, BaseUrl);
        await firstPage.SetupInitialPage();
        await firstPage.ClickStartNow();
        await firstPage.SetIrregularHours(true);
    }
    
    [Fact]
    public async Task InvalidLeaveYear()
    {
        var firstPage = new FirstPage(Page, BaseUrl);
        await firstPage.SetupInitialPage();
        await firstPage.ClickStartNow();
        await firstPage.SetIrregularHours(true);
    }}