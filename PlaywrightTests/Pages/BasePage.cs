using Microsoft.Playwright;

namespace PlaywrightTests.Pages;

public abstract class BasePage(IPage page)
{
    protected enum LocatorType
    {
        Id,
        Xpath,
        Css,
        ClassName
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

    protected async Task Navigate(string url)
    {
        await page.GotoAsync(url);
        await WaitForIdle();
    }
    
    protected async Task WaitForIdle()
        => await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
}
