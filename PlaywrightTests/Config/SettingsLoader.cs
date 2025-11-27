using System.Text.Json;

namespace PlaywrightTests.Config;

public static class SettingsLoader
{
    public static TestSettings Load()
    {
        var json = File.ReadAllText("appsettings.json");
        return JsonSerializer.Deserialize<TestSettings>(json)!;
    }
}