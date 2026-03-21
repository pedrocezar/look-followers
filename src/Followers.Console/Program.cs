using Followers.Console.Clients;
using Followers.Console.Models;
using Followers.Console.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Followers.Console.Workers;
using Microsoft.Extensions.Options;
using Refit;

var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
{
    Args = args,
    ContentRootPath = AppContext.BaseDirectory
});

builder.Services.Configure<InstagramOptions>(options =>
{
    options.BaseUrl = builder.Configuration["InstagramSettings:BaseUrl"] ?? throw new InvalidOperationException("InstagramSettings:BaseUrl was not configured.");
    options.Cookie = Environment.GetEnvironmentVariable("INSTAGRAM_SETTINGS_COOKIE") ?? string.Empty;
    options.UserId = Environment.GetEnvironmentVariable("INSTAGRAM_SETTINGS_USER_ID") ?? string.Empty;
    options.IgAppId = Environment.GetEnvironmentVariable("INSTAGRAM_SETTINGS_IG_APP_ID") ?? string.Empty;
    options.DelayMinBetweenRequestsMs = int.TryParse(Environment.GetEnvironmentVariable("INSTAGRAM_SETTINGS_DELAY_MIN_BETWEEN_REQUESTS_MS"), out var minDelay) ? minDelay : 2000;
    options.DelayMaxBetweenRequestsMs = int.TryParse(Environment.GetEnvironmentVariable("INSTAGRAM_SETTINGS_DELAY_MAX_BETWEEN_REQUESTS_MS"), out var maxDelay) ? maxDelay : 4000;
    options.RetryDelayMs = int.TryParse(Environment.GetEnvironmentVariable("INSTAGRAM_SETTINGS_RETRY_DELAY_MS"), out var retryDelay) ? retryDelay : 10000;
    options.MaxRetryAttempts = int.TryParse(Environment.GetEnvironmentVariable("INSTAGRAM_SETTINGS_MAX_RETRY_ATTEMPTS"), out var maxAttempts) ? maxAttempts : 3;
    options.MaxConnectionsPerServer = int.TryParse(Environment.GetEnvironmentVariable("INSTAGRAM_SETTINGS_MAX_CONNECTIONS_PER_SERVER"), out var maxConnections) ? maxConnections : 1;
    options.PooledConnectionLifetimeMs = int.TryParse(Environment.GetEnvironmentVariable("INSTAGRAM_SETTINGS_POOLED_CONNECTION_LIFETIME_MS"), out var pooledConnectionLifetime) ? pooledConnectionLifetime : 2000;
});

builder.Services.Configure<HostOptions>(options =>
{
    options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.StopHost;
});

builder.Services.AddScoped<IInstagramService, InstagramService>();

builder.Services.AddHostedService<NonFollowersWorker>();

builder.Services.AddRefitClient<IInstagramApi>()
    .ConfigurePrimaryHttpMessageHandler(sp =>
    {
        var options = sp.GetRequiredService<IOptions<InstagramOptions>>().Value;
        return new SocketsHttpHandler
        {
            MaxConnectionsPerServer = options.MaxConnectionsPerServer,
            PooledConnectionLifetime = TimeSpan.FromMilliseconds(options.PooledConnectionLifetimeMs)
        };
    })
    .ConfigureHttpClient((sp, client) =>
    {
        var options = sp.GetRequiredService<IOptions<InstagramOptions>>().Value;
        client.BaseAddress = new Uri(options.BaseUrl);
        var headers = client.DefaultRequestHeaders;
        headers.Clear();
        headers.Add("x-ig-app-id", options.IgAppId);
        headers.Add("cookie", options.Cookie);
        headers.Add("origin", options.BaseUrl);
        headers.Add("referer", options.BaseUrl);
    });

using var host = builder.Build();

await host.RunAsync();

return Environment.ExitCode;
