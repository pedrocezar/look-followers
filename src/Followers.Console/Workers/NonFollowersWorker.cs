using System.Text.Json;
using Followers.Console.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Followers.Console.Workers;

public sealed class NonFollowersWorker(
    IInstagramService instagramService,
    ILogger<NonFollowersWorker> logger,
    IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            logger.LogInformation("Fetching non-followers from Instagram...");
            var nonFollowers = await instagramService.GetNonFollowersAsync(stoppingToken);

            var json = JsonSerializer.Serialize(nonFollowers, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            logger.LogInformation(json);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch non-followers.");
            throw new InvalidOperationException("An error occurred while fetching non-followers.", ex);
        }
        finally
        {
            hostApplicationLifetime.StopApplication();
        }
    }
}
