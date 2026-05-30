using Followers.Console.Clients;
using Followers.Console.Configuration;
using Followers.Console.Http;
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
    InstagramOptionsConfiguration.Configure(options, builder.Configuration));

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
        InstagramHttpHeaders.Apply(client, options);
    });

using var host = builder.Build();

await host.RunAsync();

return Environment.ExitCode;
