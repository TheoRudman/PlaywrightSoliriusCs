using Microsoft.Playwright;
using Xunit.Abstractions;
using static Microsoft.Playwright.Assertions;

namespace PlaywrightTests.Pages.BaseUiPage;

// Abstract base class used by all Page Objects.
// Contains common utilities such as navigation, logging & locator handling.
public abstract class BasePage(IPage page)
{
    protected readonly IPage Page;               // Playwright browser page instance
    protected readonly string BaseUrl;           // Global site URL reference
    protected ITestOutputHelper Output;          // Logs test actions in console/CI pipeline

    // Overloaded constructor to inject URL + test logger
    protected BasePage(IPage page, string baseUrl, ITestOutputHelper output) : this(page)
    {
        Page = page;
        BaseUrl = baseUrl;
        Output = output;
    }

    protected void Log(string message)
    {
        Output?.WriteLine(message);
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
        Log($"Navigated to url: {url}");
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
        Log($"Click called on: {locator}");
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
        Log($"IsVisible called on: {element}");
    }

    protected async Task IsChecked(string element, LocatorType locator = LocatorType.Id)
    {
        element = GetSelector(element, locator);
        await IsChecked(page.Locator(element));
    }
    
    protected async Task IsChecked(ILocator locator)
    {
        await Expect(locator).ToBeCheckedAsync();
        Log($"IsChecked called on: {locator}");
    }
    
    protected async Task InputText(string element, string text, LocatorType locator = LocatorType.Id)
    {
        var locatorElement = page.Locator(GetSelector(element, locator));
        await InputText(locatorElement, text);
    }

    protected async Task InputText(ILocator locator, string text)
    {
        await locator.FillAsync(text);
        Log($"InputText called on: {locator} with text: {text}");
    }

    protected async Task ContainsText(string element, string text, LocatorType locator = LocatorType.Id)
    {
        var locatorElement = page.Locator(GetSelector(element, locator));
        await ContainsText(locatorElement, text);
    }
    
    protected async Task ContainsText(ILocator locator, string text)
    {
        await Expect(locator).ToContainTextAsync(text);
        Log($"ContainsText called on: {locator} with text: {text}");
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
            Log($"Cookies selected with value: {xpathVariable}");
        }
        else
        {
            Log($"Cannot find Cookies element: {button}");
        }
        
    }
}
