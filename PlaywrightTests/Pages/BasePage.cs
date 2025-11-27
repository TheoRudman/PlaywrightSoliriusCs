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
        //await page.GotoAsync(url);
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
            await Assertions.Expect(page.Locator(element)).ToBeVisibleAsync();
        }
        else
        {
            await Assertions.Expect(page.Locator(element)).ToBeHiddenAsync();
        }
    }

}
