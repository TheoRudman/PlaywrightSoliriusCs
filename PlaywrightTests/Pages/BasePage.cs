using static Microsoft.Playwright.Assertions;
using Microsoft.Playwright;

namespace PlaywrightTests.Pages;

public abstract class BasePage(IPage page)
{
    protected readonly IPage Page;
    protected readonly string BaseUrl;
    
    protected BasePage(IPage page, string baseUrl) : this(page)
    {
        Page = page;
        BaseUrl = baseUrl;
    }
    
    protected enum LocatorType
    {
        Id,
        Xpath,
        Css,
        ClassName
    }
    
    protected async Task Navigate(string url)
    {
        if (url.StartsWith("/"))
        {
            await page.GotoAsync(BaseUrl + url);
        }
        else
        {
            url = "/" + url;
            await page.GotoAsync(BaseUrl+ url);
        }
        await WaitForIdle();
    }
    
    private static string GetSelector(string element, LocatorType locator)
    {
        element = element.Trim();
        
        switch(locator)
        {
            case LocatorType.Id:
                element = $"#{element}";
                break;
            case LocatorType.Xpath:
                element = $"xpath={element}";
                break;
            case LocatorType.Css:
                element = $"{element}";
                break;
            case LocatorType.ClassName:
                element = element.Replace(" ", ".");
                element = $".{element}";
                break;
        }
        
        return element;
    }
    protected async Task WaitForIdle()
        => await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    
    protected async Task Click(string element, LocatorType locator = LocatorType.Id)
    {
        element = GetSelector(element, locator);
        await Click(page.Locator(element));
    }

    protected async Task Click(ILocator locator)
    {
        await locator.ClickAsync();
        await WaitForIdle();
    }

    protected async Task IsVisible(string element, bool isVisible = true, LocatorType locator = LocatorType.Id)
    {
        element = GetSelector(element, locator);
        if (isVisible)
        {
            await Expect(page.Locator(element)).ToBeVisibleAsync();
        }
        else
        {
            await Expect(page.Locator(element)).ToBeHiddenAsync();
        }
    }

    protected async Task IsChecked(string element, LocatorType locator = LocatorType.Id)
    {
        element = GetSelector(element, locator);
        await IsChecked(page.Locator(element));
    }
    
    protected async Task IsChecked(ILocator locator)
    {
        await Expect(locator).ToBeCheckedAsync();
    }
    
    protected async Task InputText(string element, string text, LocatorType locator = LocatorType.Id)
    {
        var locatorElement = page.Locator(GetSelector(element, locator));
        await InputText(locatorElement, text);
    }

    protected async Task InputText(ILocator locator, string text)
    {
        await locator.FillAsync(text);
    }
    
    protected async Task AcceptCookies(bool accept)
    {
        string xpathVariable;
        if (accept)
            xpathVariable = "accept";
        else
            xpathVariable = "reject";

        var button = Page.Locator($"//button[@data-{xpathVariable}-cookies='true']");
        bool isPresent = await button.IsVisibleAsync();
        if (isPresent)
        {
            await Click(button);
            await IsVisible("//button[@data-hide-cookie-banner='true']", true, LocatorType.Xpath);
        }
        else
        {
            Console.WriteLine($"Cannot find element: {button}");
        }
        
    }
}
