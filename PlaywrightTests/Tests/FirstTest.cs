using Microsoft.Playwright;
using PlaywrightTests.Fixtures;
using PlaywrightTests.Pages;
using Xunit;

namespace PlaywrightTests.Tests;

public class FirstTest : TestBase
{
    public FirstTest(PlaywrightFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task ValidatePage()
    {
        var firstPage = new FirstPage(Page, BaseUrl);
        await firstPage.NavigateToStart();
        await firstPage.VerifyContent();
    } 
}