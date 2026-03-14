namespace FollowersApi.Models;

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
    public int PooledConnectionLifetimeMinutes { get; set; } = 1;
}

