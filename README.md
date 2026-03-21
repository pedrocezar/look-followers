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
   dotnet run --project src/Followers.Api
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

| Variable | Description |
|----------|-------------|
| `InstagramSettings_Cookie` | Full `cookie` header from browser DevTools. |
| `InstagramSettings_IgAppId` | Value of the `x-ig-app-id` header. |
| `InstagramSettings_UserId` | Your Instagram numeric user ID. |

### Optional (appsettings.json)

| Setting | Default | Description |
|---------|---------|-------------|
| `InstagramSettings:BaseUrl` | `https://instagram.com` | Base URL for Instagram API. |
| `InstagramSettings:DelayMinBetweenRequestsMs` | 1000 | Min delay (ms) between API calls. |
| `InstagramSettings:DelayMaxBetweenRequestsMs` | 10000 | Max delay (ms) between API calls. |
| `InstagramSettings:RetryDelayMs` | 2000 | Delay (ms) before each retry. |
| `InstagramSettings:MaxRetryAttempts` | 3 | Max number of attempts per request. |
| `InstagramSettings:MaxConnectionsPerServer` | 1 | Max concurrent connections (keep 1 to reduce block risk). |
| `InstagramSettings:PooledConnectionLifetimeMs` | 1 | HTTP connection pool lifetime (milliseconds). |

Higher delays (e.g. 30–60 seconds between requests) are recommended to reduce the chance of Instagram rate limits or blocks.

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
│   └── Followers.Api/
│       ├── Clients/       # Refit API client (IInstagramApi)
│       ├── Models/        # DTOs and options
│       ├── Services/      # InstagramService (non-followers logic)
│       ├── Program.cs     # Console bootstrap and execution flow
│       └── appsettings.json
└── README.md
```

## Disclaimer

This project uses **unofficial** Instagram endpoints. Use at your own risk. Do not abuse the API (respect delays and avoid automation that violates Instagram’s terms). The authors are not responsible for account restrictions or blocks.
