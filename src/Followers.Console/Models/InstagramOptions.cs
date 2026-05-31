namespace Followers.Console.Models;

public class InstagramOptions
{
    /// <summary>
    /// Base URL for the Instagram API.
    /// </summary>
    public string BaseUrl { get; set; } = string.Empty;

    /// <summary>
    /// Your Instagram account numeric ID (e.g. 400339888).
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Full Cookie header from DevTools (contains sessionid, csrftoken, etc.).
    /// </summary>
    public string Cookie { get; set; } = string.Empty;

    /// <summary>
    /// Value of the x-ig-app-id header.
    /// </summary>
    public string IgAppId { get; set; } = string.Empty;

    /// <summary>
    /// Browser User-Agent copied from DevTools. Must match the session that produced the cookie.
    /// </summary>
    public string UserAgent { get; set; } = string.Empty;

    /// <summary>
    /// x-csrftoken header. When empty, csrftoken is parsed from <see cref="Cookie"/>.
    /// </summary>
    public string CsrfToken { get; set; } = string.Empty;

    /// <summary>
    /// x-ig-www-claim header from DevTools (session-specific, expires).
    /// </summary>
    public string IgWwwClaim { get; set; } = string.Empty;

    /// <summary>
    /// x-asbd-id header from DevTools.
    /// </summary>
    public string AsbdId { get; set; } = string.Empty;

    /// <summary>
    /// x-web-session-id header from DevTools (per browser tab).
    /// </summary>
    public string WebSessionId { get; set; } = string.Empty;

    /// <summary>
    /// referer header, e.g. https://www.instagram.com/yourusername/
    /// </summary>
    public string Referer { get; set; } = string.Empty;

    /// <summary>
    /// accept-language header from DevTools.
    /// </summary>
    public string AcceptLanguage { get; set; } = string.Empty;

    public string SecChUa { get; set; } = string.Empty;

    public string SecChUaMobile { get; set; } = string.Empty;

    public string SecChUaPlatform { get; set; } = string.Empty;

    public string SecChUaPlatformVersion { get; set; } = string.Empty;

    public string SecChUaFullVersionList { get; set; } = string.Empty;

    public string SecChUaModel { get; set; } = string.Empty;

    public string SecChPrefersColorScheme { get; set; } = string.Empty;

    /// <summary>
    /// Minimum delay (ms) between Instagram API calls. A higher value (e.g. 30000) is recommended to avoid blocks.
    /// </summary>
    public int DelayMinBetweenRequestsMs { get; set; } = 1000;

    /// <summary>
    /// Maximum delay (ms) between Instagram API calls. A higher value (e.g. 60000) is recommended to avoid blocks.
    /// </summary>
    public int DelayMaxBetweenRequestsMs { get; set; } = 10000;

    /// <summary>
    /// Fixed delay (ms) between retry attempts when an API call fails.
    /// </summary>
    public int RetryDelayMs { get; set; } = 2000;

    /// <summary>
    /// Maximum number of attempts per API call (including the first attempt).
    /// </summary>
    public int MaxRetryAttempts { get; set; } = 3;

    /// <summary>
    /// Maximum concurrent connections to the Instagram server. Setting to 1 is recommended to avoid blocks.
    /// </summary>
    public int MaxConnectionsPerServer { get; set; } = 1;

    /// <summary>
    /// Lifetime (minutes) for pooled HTTP connections. Setting to 1 minute is recommended to avoid blocks.
    /// </summary>
    public int PooledConnectionLifetimeMs { get; set; } = 1;

    /// <summary>
    /// Usernames to exclude from the non-followers list (JSON array via <c>INSTAGRAM_SETTINGS_EXCLUDED_USERNAMES</c>).
    /// </summary>
    public string[] ExcludedUsernames { get; set; } = [];
}
