using FollowersApi.Clients;
using FollowersApi.Models;
using FollowersApi.Services;
using Microsoft.Extensions.Options;
using Refit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.Configure<InstagramOptions>(options =>
{
    options.BaseUrl = builder.Configuration["InstagramSettings:BaseUrl"] ?? throw new InvalidOperationException("InstagramSettings:BaseUrl não foi configurado.");
    options.Cookie = Environment.GetEnvironmentVariable("InstagramSettings_Cookie") ?? string.Empty;
    options.IgAppId = Environment.GetEnvironmentVariable("InstagramSettings_IgAppId") ?? string.Empty;
    options.UserId = Environment.GetEnvironmentVariable("InstagramSettings_UserId") ?? string.Empty;
    options.DelayMinBetweenRequestsMs = int.TryParse(builder.Configuration["InstagramSettings:DelayMinBetweenRequestsMs"], out var minDelay) ? minDelay : 1000;
    options.DelayMaxBetweenRequestsMs = int.TryParse(builder.Configuration["InstagramSettings:DelayMaxBetweenRequestsMs"], out var maxDelay) ? maxDelay : 10000;
    options.RetryDelayMs = int.TryParse(builder.Configuration["InstagramSettings:RetryDelayMs"], out var retryDelay) ? retryDelay : 2000;
    options.MaxRetryAttempts = int.TryParse(builder.Configuration["InstagramSettings:MaxRetryAttempts"], out var maxAttempts) ? maxAttempts : 3;
});

builder.Services.AddScoped<InstagramService>();

builder.Services.AddRefitClient<IInstagramApi>().ConfigureHttpClient((sp, client) =>
{
    var options = sp.GetRequiredService<IOptions<InstagramOptions>>().Value;
    client.BaseAddress = new Uri(options.BaseUrl);
    var headers = client.DefaultRequestHeaders;
    headers.Clear();
    headers.Add("x-ig-app-id", options.IgAppId);
    headers.Add("cookie", options.Cookie);
});

var app = builder.Build();

if (!app.Environment.IsProduction())
    app.MapOpenApi();

app.UseHttpsRedirection();

app.MapGet("/api/nonfollowers", async (InstagramService instagramService, CancellationToken cancellationToken) =>
    await instagramService.GetNonFollowersAsync(cancellationToken))
    .WithName("GetNonFollowers")
    .WithSummary("Retorna as pessoas que você segue e que não te seguem de volta.")
    .WithDescription("Busca todos os seus 'following' e todos os 'followers' no Instagram (paginando pelas APIs) e devolve apenas quem não te segue de volta.");

await app.RunAsync();
