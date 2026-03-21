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
    options.DelayMinBetweenRequestsMs = int.TryParse(builder.Configuration["InstagramSettings:DelayMinBetweenRequestsMs"], out var minDelay) ? minDelay : 1000;
    options.DelayMaxBetweenRequestsMs = int.TryParse(builder.Configuration["InstagramSettings:DelayMaxBetweenRequestsMs"], out var maxDelay) ? maxDelay : 10000;
    options.RetryDelayMs = int.TryParse(builder.Configuration["InstagramSettings:RetryDelayMs"], out var retryDelay) ? retryDelay : 2000;
    options.MaxRetryAttempts = int.TryParse(builder.Configuration["InstagramSettings:MaxRetryAttempts"], out var maxAttempts) ? maxAttempts : 3;
    options.MaxConnectionsPerServer = int.TryParse(builder.Configuration["InstagramSettings:MaxConnectionsPerServer"], out var maxConnections) ? maxConnections : 1;
    options.PooledConnectionLifetimeMs = int.TryParse(builder.Configuration["InstagramSettings:PooledConnectionLifetimeMs"], out var pooledConnectionLifetime) ? pooledConnectionLifetime : 1000;
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
