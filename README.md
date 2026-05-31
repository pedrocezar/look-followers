# Look Followers

A .NET console application that discovers **people you follow on Instagram who do not follow you back** (non-followers). It uses Instagram’s internal friendship APIs with pagination, retries, and configurable delays to reduce the risk of rate limits or blocks.

## Features

- **Console output**: Prints a JSON list of users you follow who are not in your followers list.
- **Pagination**: Fetches all following and followers via Instagram’s paginated APIs.
- **Retries**: Configurable retries with delay on transient failures.
- **Rate limiting**: Random delay between requests and single-connection usage to avoid blocks.

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Instagram credentials (see [Configuration](#configuration))

## Quick Start

1. **Clone and run**

   ```bash
   git clone https://github.com/pedrocezar/look-followers.git
   cd look-followers
   dotnet run --project src/Followers.Console
   ```

2. **Read output in terminal**

   ```bash
   [
     { "username": "johndoe", "fullName": "John Doe" },
     { "username": "janedoe", "fullName": "Jane Doe" }
   ]
   ```

## Configuration

Settings are read from `appsettings.json` and environment variables.

### Required (environment variables)

Copy these from the same `followers` request in Chrome DevTools (Network → Headers).

| Variable | Description |
|----------|-------------|
| `INSTAGRAM_SETTINGS_COOKIE` | Full `cookie` header (`sessionid`, `csrftoken`, etc.). |
| `INSTAGRAM_SETTINGS_IG_APP_ID` | `x-ig-app-id` (e.g. `936619743392459`). |
| `INSTAGRAM_SETTINGS_USER_ID` | Your numeric Instagram user ID. |
| `INSTAGRAM_SETTINGS_IG_WWW_CLAIM` | `x-ig-www-claim` (session-specific, expires). |
| `INSTAGRAM_SETTINGS_WEB_SESSION_ID` | `x-web-session-id` (per browser tab). |
| `INSTAGRAM_SETTINGS_REFERER` | `referer`, e.g. `https://www.instagram.com/yourusername/` |

`x-csrftoken` is taken from `INSTAGRAM_SETTINGS_CSRF_TOKEN` when set; otherwise it is parsed from `csrftoken` in the cookie.

### Optional (environment variables or appsettings.json)

| Variable / setting | Default (appsettings) | Description |
|--------------------|----------------------|-------------|
| `INSTAGRAM_SETTINGS_USER_AGENT` / `UserAgent` | Chrome on macOS | Must match the browser session that produced the cookie. |
| `INSTAGRAM_SETTINGS_CSRF_TOKEN` / `CsrfToken` | (from cookie) | Override `x-csrftoken` if needed. |
| `INSTAGRAM_SETTINGS_ASBD_ID` / `AsbdId` | `359341` | `x-asbd-id` from DevTools. |
| `INSTAGRAM_SETTINGS_ACCEPT_LANGUAGE` / `AcceptLanguage` | `pt-BR,...` | `accept-language` header. |
| `INSTAGRAM_SETTINGS_SEC_CH_*` / `SecChUa*` | Chrome 148 / macOS | Client hint headers (`sec-ch-ua`, etc.). |
| `INSTAGRAM_SETTINGS_BASE_URL` / `BaseUrl` | `https://www.instagram.com` | API base URL. |
| `DelayMinBetweenRequestsMs` | 2000 | Min delay (ms) between API calls. |
| `DelayMaxBetweenRequestsMs` | 4000 | Max delay (ms) between API calls. |
| `RetryDelayMs` | 10000 | Delay (ms) before each retry. |
| `MaxRetryAttempts` | 3 | Max number of attempts per request. |
| `MaxConnectionsPerServer` | 1 | Max concurrent connections (keep 1 to reduce block risk). |
| `PooledConnectionLifetimeMs` | 2000 | HTTP connection pool lifetime (milliseconds). |

Higher delays (e.g. 30–60 seconds between requests) are recommended to reduce the chance of Instagram rate limits or blocks.

### Excluded usernames (environment variable only)

| Variable | Description |
|----------|-------------|
| `INSTAGRAM_SETTINGS_EXCLUDED_USERNAMES` | JSON array of Instagram usernames to **omit** from the non-followers list (accounts you want to keep following even if they do not follow back). Example: `["user1","user2"]` |

## Output

Returns users you follow who do not follow you back as a JSON array:

```json
[
  { "username": "johndoe", "fullName": "John Doe" },
  { "username": "janedoe", "fullName": "Jane Doe" }
]
```

## Project structure

```
look-followers/
├── src/
│   └── Followers.Console/
│       ├── Clients/       # Refit API client (IInstagramApi)
│       ├── Models/        # DTOs and options
│       ├── Services/      # InstagramService (non-followers logic)
│       ├── Program.cs     # Console bootstrap and execution flow
│       └── appsettings.json
└── README.md
```

## Disclaimer

This project uses **unofficial** Instagram endpoints. Use at your own risk. Do not abuse the API (respect delays and avoid automation that violates Instagram’s terms). The authors are not responsible for account restrictions or blocks.
