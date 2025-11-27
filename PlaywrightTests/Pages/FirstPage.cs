using Microsoft.Playwright;

namespace PlaywrightTests.Pages;

public class FirstPage : BasePage
{
    public FirstPage(IPage page, string baseUrl) : base(page, baseUrl)
    {
    }

    public async Task NavigateToStart()
    {
        await Navigate("/");
    }

    public async Task VerifyContent()
    {
        await IsVisible("content");
    }
}