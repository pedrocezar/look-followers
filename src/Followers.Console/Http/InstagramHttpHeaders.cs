using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Followers.Console.Models;

namespace Followers.Console.Http;

public static class InstagramHttpHeaders
{
    private static readonly Regex CookieValueRegex = new(
        @"(?:^|;\s*)csrftoken=([^;]*)",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant,
        TimeSpan.FromMilliseconds(100));

    public static void Apply(HttpClient client, InstagramOptions options)
    {
        var headers = client.DefaultRequestHeaders;
        headers.Clear();

        var csrfToken = !string.IsNullOrWhiteSpace(options.CsrfToken)
            ? options.CsrfToken
            : TryExtractCsrfToken(options.Cookie);

        TryAdd(headers, "User-Agent", options.UserAgent);
        TryAdd(headers, "accept", "*/*");
        TryAdd(headers, "accept-language", options.AcceptLanguage);
        TryAdd(headers, "cookie", options.Cookie);
        TryAdd(headers, "dnt", "1");
        TryAdd(headers, "priority", "u=1, i");
        TryAdd(headers, "referer", options.Referer);
        TryAdd(headers, "sec-ch-prefers-color-scheme", options.SecChPrefersColorScheme);
        TryAdd(headers, "sec-ch-ua", options.SecChUa);
        TryAdd(headers, "sec-ch-ua-full-version-list", options.SecChUaFullVersionList);
        TryAdd(headers, "sec-ch-ua-mobile", options.SecChUaMobile);
        TryAdd(headers, "sec-ch-ua-model", options.SecChUaModel);
        TryAdd(headers, "sec-ch-ua-platform", options.SecChUaPlatform);
        TryAdd(headers, "sec-ch-ua-platform-version", options.SecChUaPlatformVersion);
        TryAdd(headers, "sec-fetch-dest", "empty");
        TryAdd(headers, "sec-fetch-mode", "cors");
        TryAdd(headers, "sec-fetch-site", "same-origin");
        TryAdd(headers, "x-asbd-id", options.AsbdId);
        TryAdd(headers, "x-csrftoken", csrfToken);
        TryAdd(headers, "x-ig-app-id", options.IgAppId);
        TryAdd(headers, "x-ig-max-touch-points", "0");
        TryAdd(headers, "x-ig-www-claim", options.IgWwwClaim);
        TryAdd(headers, "x-requested-with", "XMLHttpRequest");
        TryAdd(headers, "x-web-session-id", options.WebSessionId);
    }

    public static string? TryExtractCsrfToken(string cookie)
    {
        if (string.IsNullOrWhiteSpace(cookie))
            return null;

        var match = CookieValueRegex.Match(cookie);
        return match.Success ? Uri.UnescapeDataString(match.Groups[1].Value) : null;
    }

    private static void TryAdd(HttpRequestHeaders headers, string name, string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
            headers.TryAddWithoutValidation(name, value);
    }
}
