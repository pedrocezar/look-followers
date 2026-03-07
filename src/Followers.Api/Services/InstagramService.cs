using FollowersApi.Clients;
using FollowersApi.Models;
using Microsoft.Extensions.Options;

namespace FollowersApi.Services;

public interface IInstagramService
{
    Task<IReadOnlyCollection<InstagramUser>> GetNonFollowersAsync(CancellationToken cancellationToken = default);
}

public class InstagramService(
    IInstagramApi _api,
    IOptions<InstagramOptions> _options,
    ILogger<InstagramService> _logger) : IInstagramService
{
    public async Task<IReadOnlyCollection<InstagramUser>> GetNonFollowersAsync(CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_options.Value.UserId))
            throw new InvalidOperationException("InstagramOptions.UserId não foi configurado.");

        var following = await GetAllFollowingAsync(cancellationToken);
        var followers = await GetAllFollowersAsync(cancellationToken);

        var followerIds = new HashSet<string>(followers.Select(u => u.Pk));

        var nonFollowers = following
            .Where(f => !followerIds.Contains(f.Pk))
            .OrderBy(f => f.Username, StringComparer.OrdinalIgnoreCase)
            .ToList();

        _logger.LogInformation(
            "Total seguindo: {FollowingCount}, seguidores: {FollowersCount}, não te seguem de volta: {NonFollowersCount}",
            following.Count,
            followers.Count,
            nonFollowers.Count);

        return nonFollowers;
    }

    private Task<List<InstagramUser>> GetAllFollowingAsync(CancellationToken cancellationToken = default) =>
        GetPaginatedAsync(
            (cursor, ct) => _api.GetFollowingAsync(_options.Value.UserId, 200, cursor),
            cancellationToken);

    private Task<List<InstagramUser>> GetAllFollowersAsync(CancellationToken cancellationToken = default) =>
        GetPaginatedAsync(
            (cursor, ct) => _api.GetFollowersAsync(_options.Value.UserId, 200, cursor, "follow_list_page"),
            cancellationToken);

    private static async Task<List<InstagramUser>> GetPaginatedAsync(
        Func<string?, CancellationToken, Task<InstagramFriendshipResponse>> fetchPage,
        CancellationToken cancellationToken)
    {
        var allUsers = new List<InstagramUser>();
        string? nextMaxId = null;

        while (true)
        {
            var data = await fetchPage(nextMaxId, cancellationToken);

            if (data?.Users is { Count: > 0 })
                allUsers.AddRange(data.Users);

            if (data is null || !data.HasMore || string.IsNullOrEmpty(data.NextMaxId))
                break;

            nextMaxId = data.NextMaxId;
        }

        return allUsers;
    }
}

