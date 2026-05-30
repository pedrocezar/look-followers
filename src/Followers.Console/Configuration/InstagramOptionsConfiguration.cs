using Followers.Console.Models;
using Microsoft.Extensions.Configuration;

namespace Followers.Console.Configuration;

public static class InstagramOptionsConfiguration
{
    public static void Configure(InstagramOptions options, IConfiguration configuration)
    {
        var section = configuration.GetSection("InstagramSettings");

        options.BaseUrl = EnvOrConfig("INSTAGRAM_SETTINGS_BASE_URL", section, "BaseUrl")
            ?? "https://www.instagram.com";
        options.Cookie = EnvOrConfig("INSTAGRAM_SETTINGS_COOKIE", section, "Cookie") ?? string.Empty;
        options.UserId = EnvOrConfig("INSTAGRAM_SETTINGS_USER_ID", section, "UserId") ?? string.Empty;
        options.IgAppId = EnvOrConfig("INSTAGRAM_SETTINGS_IG_APP_ID", section, "IgAppId") ?? string.Empty;
        options.UserAgent = EnvOrConfig("INSTAGRAM_SETTINGS_USER_AGENT", section, "UserAgent") ?? string.Empty;
        options.CsrfToken = EnvOrConfig("INSTAGRAM_SETTINGS_CSRF_TOKEN", section, "CsrfToken") ?? string.Empty;
        options.IgWwwClaim = EnvOrConfig("INSTAGRAM_SETTINGS_IG_WWW_CLAIM", section, "IgWwwClaim") ?? string.Empty;
        options.AsbdId = EnvOrConfig("INSTAGRAM_SETTINGS_ASBD_ID", section, "AsbdId") ?? string.Empty;
        options.WebSessionId = EnvOrConfig("INSTAGRAM_SETTINGS_WEB_SESSION_ID", section, "WebSessionId") ?? string.Empty;
        options.Referer = EnvOrConfig("INSTAGRAM_SETTINGS_REFERER", section, "Referer") ?? string.Empty;
        options.AcceptLanguage = EnvOrConfig("INSTAGRAM_SETTINGS_ACCEPT_LANGUAGE", section, "AcceptLanguage") ?? string.Empty;
        options.SecChUa = EnvOrConfig("INSTAGRAM_SETTINGS_SEC_CH_UA", section, "SecChUa") ?? string.Empty;
        options.SecChUaMobile = EnvOrConfig("INSTAGRAM_SETTINGS_SEC_CH_UA_MOBILE", section, "SecChUaMobile") ?? string.Empty;
        options.SecChUaPlatform = EnvOrConfig("INSTAGRAM_SETTINGS_SEC_CH_UA_PLATFORM", section, "SecChUaPlatform") ?? string.Empty;
        options.SecChUaPlatformVersion = EnvOrConfig("INSTAGRAM_SETTINGS_SEC_CH_UA_PLATFORM_VERSION", section, "SecChUaPlatformVersion") ?? string.Empty;
        options.SecChUaFullVersionList = EnvOrConfig("INSTAGRAM_SETTINGS_SEC_CH_UA_FULL_VERSION_LIST", section, "SecChUaFullVersionList") ?? string.Empty;
        options.SecChUaModel = EnvOrConfig("INSTAGRAM_SETTINGS_SEC_CH_UA_MODEL", section, "SecChUaModel") ?? string.Empty;
        options.SecChPrefersColorScheme = EnvOrConfig("INSTAGRAM_SETTINGS_SEC_CH_PREFERS_COLOR_SCHEME", section, "SecChPrefersColorScheme") ?? string.Empty;

        options.DelayMinBetweenRequestsMs = ReadInt(
            "INSTAGRAM_SETTINGS_DELAY_MIN_BETWEEN_REQUESTS_MS",
            section,
            "DelayMinBetweenRequestsMs",
            options.DelayMinBetweenRequestsMs);
        options.DelayMaxBetweenRequestsMs = ReadInt(
            "INSTAGRAM_SETTINGS_DELAY_MAX_BETWEEN_REQUESTS_MS",
            section,
            "DelayMaxBetweenRequestsMs",
            options.DelayMaxBetweenRequestsMs);
        options.RetryDelayMs = ReadInt(
            "INSTAGRAM_SETTINGS_RETRY_DELAY_MS",
            section,
            "RetryDelayMs",
            options.RetryDelayMs);
        options.MaxRetryAttempts = ReadInt(
            "INSTAGRAM_SETTINGS_MAX_RETRY_ATTEMPTS",
            section,
            "MaxRetryAttempts",
            options.MaxRetryAttempts);
        options.MaxConnectionsPerServer = ReadInt(
            "INSTAGRAM_SETTINGS_MAX_CONNECTIONS_PER_SERVER",
            section,
            "MaxConnectionsPerServer",
            options.MaxConnectionsPerServer);
        options.PooledConnectionLifetimeMs = ReadInt(
            "INSTAGRAM_SETTINGS_POOLED_CONNECTION_LIFETIME_MS",
            section,
            "PooledConnectionLifetimeMs",
            options.PooledConnectionLifetimeMs);
    }

    private static string? EnvOrConfig(string envName, IConfigurationSection section, string key) =>
        Environment.GetEnvironmentVariable(envName) ?? section[key];

    private static int ReadInt(string envName, IConfigurationSection section, string key, int fallback)
    {
        var env = Environment.GetEnvironmentVariable(envName);
        if (int.TryParse(env, out var fromEnv))
            return fromEnv;

        return section.GetValue(key, fallback);
    }
}
