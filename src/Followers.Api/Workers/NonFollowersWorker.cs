using FollowersApi.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FollowersApi.Workers;

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

            Console.WriteLine(json);
            logger.LogInformation("Finished. Total non-followers: {Count}", nonFollowers.Count);
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
