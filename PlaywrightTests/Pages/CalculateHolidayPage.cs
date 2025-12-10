using Microsoft.Playwright;
using PlaywrightTests.Pages.BaseUiPage;
using Xunit.Abstractions;

namespace PlaywrightTests.Pages;

public class CalculateHolidayPage(IPage page, string baseUrl, ITestOutputHelper? output = null) : BasePage(page, baseUrl, output)
{
    public async Task SetupInitialPage( bool acceptCookies = true)
    {
        await Navigate("/");
        await IsVisible("//h1[normalize-space()='Calculate holiday entitlement']", true, LocatorType.Xpath);
        await AcceptCookies(acceptCookies);
    }
    
    public async Task ClickStartNow()
    {
        await Click("//a[@href='/calculate-your-holiday-entitlement/y']", LocatorType.Xpath);
        await IsVisible("//h1[text()='Does the employee work irregular hours or for part of the year?']", true, LocatorType.Xpath);
    }

    public async Task SetIrregularHours(bool isIrregular)
    {
        string element;
        string answer;

        if (isIrregular)
        {
            element = "response-0";
            answer = "Yes";
        }
        else
        {
            element = "response-1";
            answer = "No";
        }

        await Click(element);
        await IsChecked(element);
        await Click("//button[text()='Continue']", LocatorType.Xpath);
        
        await ContainsText("govuk-summary-list__key",
            "Does the employee work irregular hours or for part of the year?", LocatorType.ClassName);
        await ContainsText("govuk-summary-list__value", answer, LocatorType.ClassName);
    }
    
    public async Task SetLeaveYear(string day, string month, string year, bool isValid)
    {
        await IsVisible("//h1[text()='When does the leave year start?']", true, LocatorType.Xpath);
        
        await InputText("response-0", day);
        await InputText("response-1", month);
        await InputText("response-2", year);

        await Click("//button[text()='Continue']", LocatorType.Xpath);

        if (isValid)
        {
            var element = page.Locator(".govuk-summary-list__key").Nth(1);
            await ContainsText(element, "When does the leave year start?");
        }
    }

    public async Task ValidateErrorMessage()
    {
        await IsVisible("govuk-error-summary__title", true, LocatorType.ClassName);
        await ContainsText("govuk-error-summary__title", "There is a problem", LocatorType.ClassName);
    }

    public async Task SetHolidayEntitlement(int selection)
    {
        await ContainsText("govuk-fieldset__heading gem-c-radio__heading-text", "Is the holiday entitlement based on:",
            LocatorType.ClassName);
        await Click($"response-{selection.ToString()}");
        string answer = await page.Locator($"//label[@for='response-{selection}']").InnerTextAsync();
        await Click("//button[text()='Continue']", LocatorType.Xpath);

        var question = page.Locator(".govuk-summary-list__key").Nth(2);
        var questionValue = page.Locator(".govuk-summary-list__value").Nth(2);
        await ContainsText(question, "Is the holiday entitlement based on:");
        await ContainsText(questionValue, answer);
    }

    public async Task SetWorkOutHoliday(int selection)
    {
        await ContainsText("govuk-fieldset__heading gem-c-radio__heading-text", "Do you want to work out holiday:",
            LocatorType.ClassName);
        await Click($"response-{selection.ToString()}");

        string answer = await page.Locator($"//label[@for='response-{selection}']").InnerTextAsync();
        
        await Click("//button[text()='Continue']", LocatorType.Xpath);
        
        if (selection == 0)
        {
            await ContainsText("gem-c-label govuk-label govuk-label--l", "Number of hours worked per week?",
                LocatorType.ClassName);
        }
        else if(selection == 2)
        {
            await ContainsText("govuk-fieldset__heading", "What was the employment end date?", LocatorType.ClassName);
        }
        else
        {
            await ContainsText("govuk-fieldset__heading", "What was the employment start date?", LocatorType.ClassName);
        }
        
        var question = page.Locator(".govuk-summary-list__key").Nth(3);
        var questionValue = page.Locator(".govuk-summary-list__value").Nth(3);
        await ContainsText(question, "Do you want to work out holiday:");
        await ContainsText(questionValue, answer);
    }

    public async Task SetEmploymentDate(string day, string month, string year, bool isValid)
    {
        string title = "What was the employment end date?";
        
        await ContainsText("govuk-fieldset__heading", title,
            LocatorType.ClassName);
        await InputText("response-0", day);
        await InputText("response-1", month);
        await InputText("response-2", year);
        await Click("//button[text()='Continue']", LocatorType.Xpath);
        
        if(isValid)
            await ContainsText(page.Locator(".govuk-summary-list__key").Nth(4), title);
        else
            await ValidateErrorMessage();
    }

    public async Task SetNumberOfDaysWorked(string days, bool isValid)
    {
        await ContainsText("//label[@for='response']", "Number of days worked per week?", LocatorType.Xpath);
        await InputText("response", days);
        await Click("//button[text()='Continue']", LocatorType.Xpath);

        if (isValid)
        {
            var question = page.Locator(".govuk-summary-list__key").Nth(5);
            var questionValue = page.Locator(".govuk-summary-list__value").Nth(5);
            await ContainsText(question, "Number of days worked per week?");
            await ContainsText(questionValue, days);
        }
        else
        {
            await ValidateErrorMessage();
        }
    }

    public async Task VerifyCompleted(string days)
    {
        await IsVisible($"//p[text()='The statutory holiday entitlement is {days} days holiday.']", true, LocatorType.Xpath);
    }

    public async Task StartAgain()
    {
        await Click("//a[text()='Start again']", LocatorType.Xpath);
        await IsVisible("//a[@href='/calculate-your-holiday-entitlement/y']", true, LocatorType.Xpath);
    }

    public async Task ClickChange(int rowNumber)
    {
        var title = await page.Locator(".govuk-summary-list__key").Nth(rowNumber).InnerTextAsync();
        rowNumber++;
        await Click($"//a[text()[{rowNumber}][normalize-space()='Change']]", LocatorType.Xpath);
        await ContainsText($"//h1[text()='{title}']", title, LocatorType.Xpath);
    }
}