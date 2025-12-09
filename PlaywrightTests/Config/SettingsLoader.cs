using System.Text.Json;

namespace PlaywrightTests.Config;

// Loads configuration from appsettings.json at runtime.
// Useful for switching test environments without changing code.
public static class SettingsLoader
{
    public static TestSettings Load()
    {
        // Reads config file once per execution
        var json = File.ReadAllText("appsettings.json");

        // Deserialises JSON values into strongly-typed TestSettings object
        return JsonSerializer.Deserialize<TestSettings>(json)!;
    }
}