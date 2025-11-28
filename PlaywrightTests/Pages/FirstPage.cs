using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;


namespace PlaywrightTests.Pages;

public class FirstPage(IPage page, string baseUrl) : BasePage(page, baseUrl)
{
    public async Task NavigateToStart()
    {
        await Navigate("/");
    }
    
    public async Task SetupInitialPage( bool acceptCookies = true)
    {
        await NavigateToStart();
        await VerifyContent();
        await AcceptCookies(acceptCookies);
    }

    public async Task VerifyContent()
    {
        await IsVisible("content");
    }
    
    public async Task ClickStartNow()
    {
        await Click("//a[@href='/calculate-your-holiday-entitlement/y']", LocatorType.Xpath);
        await IsVisible("//h1[text()='Does the employee work irregular hours or for part of the year?']", true, LocatorType.Xpath);
    }

    public async Task SetIrregularHours(bool isIrregular)
    {
        string element;

        if (isIrregular)
            element = "response-0";
        else
            element = "response-1";
        
        await Click(element);
        await IsChecked(element);
    }

    public async Task SetLeaveYear(string day, string month, string year)
    {
        await Click("gem-c-button govuk-button gem-c-button--bottom-margin", LocatorType.ClassName);
        await IsVisible("//h1[text()='When does the leave year start?']", true, LocatorType.Xpath);
        
        await InputText("response-0", day);
        await InputText("response-1", month);
        await InputText("response-2", year);
        
        
    }
}