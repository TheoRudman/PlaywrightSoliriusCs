namespace PlaywrightTests.Config;

// Config object used by fixture to build browser instance.
// Defaults exist to avoid null values if config keys are missing.
public class TestSettings
{
    public string BaseUrl { get; set; } = "";
    public string Channel { get; set; } = "chrome";
    public bool Headless { get; set; } = true;
}