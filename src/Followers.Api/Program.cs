using System.Net.Http.Headers;
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
    options.UserId = Environment.GetEnvironmentVariable("InstagramSettings__UserId") ?? string.Empty;
    options.Cookie = Environment.GetEnvironmentVariable("InstagramSettings__Cookie") ?? string.Empty;
    options.CsrfToken = Environment.GetEnvironmentVariable("InstagramSettings__CsrfToken") ?? string.Empty;
    options.IgAppId = Environment.GetEnvironmentVariable("InstagramSettings__IgAppId") ?? string.Empty;
    options.XIgWwwClaim = Environment.GetEnvironmentVariable("InstagramSettings__XIgWwwClaim") ?? string.Empty;
    options.AsbdId = Environment.GetEnvironmentVariable("InstagramSettings__AsbdId") ?? string.Empty;
    options.WebSessionId = Environment.GetEnvironmentVariable("InstagramSettings__WebSessionId") ?? string.Empty;
    options.UserAgent = Environment.GetEnvironmentVariable("InstagramSettings__UserAgent") ?? string.Empty;
});

builder.Services.AddScoped<InstagramService>();

builder.Services
    .AddRefitClient<IInstagramApi>()
    .ConfigureHttpClient((sp, client) =>
    {
        var options = sp.GetRequiredService<IOptions<InstagramOptions>>().Value;

        client.BaseAddress = new Uri(options.BaseUrl);

        var headers = client.DefaultRequestHeaders;
        headers.Clear();

        headers.Accept.Clear();
        headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));

        if (!string.IsNullOrWhiteSpace(options.UserAgent))
            headers.UserAgent.ParseAdd(options.UserAgent);

        if (!string.IsNullOrWhiteSpace(options.CsrfToken))
            headers.Add("x-csrftoken", options.CsrfToken);

        if (!string.IsNullOrWhiteSpace(options.IgAppId))
            headers.Add("x-ig-app-id", options.IgAppId);

        if (!string.IsNullOrWhiteSpace(options.XIgWwwClaim))
            headers.Add("x-ig-www-claim", options.XIgWwwClaim);

        if (!string.IsNullOrWhiteSpace(options.AsbdId))
            headers.Add("x-asbd-id", options.AsbdId);

        if (!string.IsNullOrWhiteSpace(options.WebSessionId))
            headers.Add("x-web-session-id", options.WebSessionId);

        headers.Add("x-requested-with", "XMLHttpRequest");

        if (!string.IsNullOrWhiteSpace(options.Cookie))
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
