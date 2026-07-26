using Microsoft.Extensions.Configuration;

namespace Formulaar1.Tests;

public class ConfigurationTests
{
    [Fact]
    public void GetStringSetting_PrefersComposeStyleKey_WhenAppSettingsValueIsBlank()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["APICredentials:Sonarr:ApiKey"] = "",
                ["Sonarr:ApiKey"] = "compose-key"
            })
            .Build();

        var result = Program.GetStringSetting(config, "Sonarr:ApiKey", "APICredentials:Sonarr:ApiKey");

        Assert.Equal("compose-key", result);
    }

    [Fact]
    public void GetStringSetting_FallsBackToAppSettingsKey_WhenComposeStyleKeyIsMissing()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["APICredentials:qBittorrentClient:BasePath"] = "http://localhost:8080"
            })
            .Build();

        var result = Program.GetStringSetting(config, "qBittorrentClient:BasePath", "APICredentials:qBittorrentClient:BasePath");

        Assert.Equal("http://localhost:8080", result);
    }

    [Fact]
    public void GetBoolSetting_PrefersComposeStyleKey()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["APICredentials:bugsnag:enabled"] = "true",
                ["bugsnag:enabled"] = "false"
            })
            .Build();

        var result = Program.GetBoolSetting(config, defaultValue: true, "bugsnag:enabled", "APICredentials:bugsnag:enabled");

        Assert.False(result);
    }
}
