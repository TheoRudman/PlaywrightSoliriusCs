namespace PlaywrightTests.Config;

public class TestSettings
{
    public string BaseUrl { get; set; } = "";
    public string Channel { get; set; } = "chrome";
    public bool Headless { get; set; } = true;
}