using FollowersApi.Clients;
using FollowersApi.Models;
using Microsoft.Extensions.Options;

namespace FollowersApi.Services;

public interface IInstagramService
{
    Task<List<InstagramUserResponse>> GetNonFollowersAsync(CancellationToken cancellationToken = default);
}

public class InstagramService(
    IInstagramApi _api,
    IOptions<InstagramOptions> _options,
    ILogger<InstagramService> _logger) : IInstagramService
{
    public async Task<List<InstagramUserResponse>> GetNonFollowersAsync(CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_userId))
            throw new InvalidOperationException("InstagramOptions.UserId was not configured.");

        var following = await GetAllFollowingAsync(cancellationToken);
        var followers = await GetAllFollowersAsync(cancellationToken);

        var followerIds = new HashSet<string>(followers.Select(u => u.Pk));

        var nonFollowers = following
            .Where(f => !followerIds.Contains(f.Pk))
            .OrderBy(f => f.Username, StringComparer.OrdinalIgnoreCase)
            .Select(f => new InstagramUserResponse
            {
                Username = f.Username,
                FullName = f.FullName
            })
            .ToList();

        _logger.LogInformation(
            "Total following: {FollowingCount}, followers: {FollowersCount}, non-followers (do not follow back): {NonFollowersCount}",
            following.Count,
            followers.Count,
            nonFollowers.Count);

        return nonFollowers;
    }

    private async Task<List<InstagramUser>> GetAllFollowingAsync(CancellationToken cancellationToken = default) =>
        await GetPaginatedAsync(
            async (cursor, ct) => await _api.GetFollowingAsync(_userId, 200, cursor),
            cancellationToken);

    private async Task<List<InstagramUser>> GetAllFollowersAsync(CancellationToken cancellationToken = default) =>
        await GetPaginatedAsync(
            async (cursor, ct) => await _api.GetFollowersAsync(_userId, 25, cursor),
            cancellationToken);

    private async Task<List<InstagramUser>> GetPaginatedAsync(
        Func<string?, CancellationToken, Task<InstagramFriendshipResponse>> fetchPage,
        CancellationToken cancellationToken)
    {
        var allUsers = new List<InstagramUser>();
        var nextMaxId = (string?)null;

        while (true)
        {
            var data = await FetchPageWithRetryAsync(fetchPage, nextMaxId, cancellationToken);

            AddUsersIfAny(allUsers, data);

            if (ShouldStopPagination(data))
                break;

            nextMaxId = data?.NextMaxId;

            await Task.Delay(Random.Shared.Next(_delayMinBetweenRequestsMs, _delayMaxBetweenRequestsMs), cancellationToken);
        }

        return allUsers;
    }

    private async Task<InstagramFriendshipResponse?> FetchPageWithRetryAsync(
        Func<string?, CancellationToken, Task<InstagramFriendshipResponse>> fetchPage,
        string? nextMaxId,
        CancellationToken cancellationToken)
    {
        for (var attempt = 1; attempt <= _maxRetryAttempts; attempt++)
        {
            try
            {
                var result = await fetchPage(nextMaxId, cancellationToken);
                return result;
            }
            catch (Exception ex) when (!cancellationToken.IsCancellationRequested)
            {
                if (attempt >= _maxRetryAttempts)
                {
                    _logger.LogError(
                        ex,
                        "Error fetching Instagram user page after {MaxAttempts} attempts. Aborting pagination and returning partial result.",
                        _maxRetryAttempts);
                    
                    throw new InvalidOperationException($"Failed to fetch Instagram data after {_maxRetryAttempts} attempts. See logs for details.", ex);
                }

                _logger.LogWarning(
                    ex,
                    "Error fetching Instagram user page (attempt {Attempt}/{MaxAttempts}). Waiting {DelayMs}ms before retrying.",
                    attempt,
                    _maxRetryAttempts,
                    _retryDelayMs);

                await Task.Delay(_retryDelayMs, cancellationToken);
            }
        }

        throw new InvalidOperationException("Unexpected failure fetching Instagram data. All retry attempts were exhausted.");
    }

    private static void AddUsersIfAny(List<InstagramUser> allUsers, InstagramFriendshipResponse? data)
    {
        if (data?.Users is { Count: > 0 })
            allUsers.AddRange(data.Users);
    }

    private static bool ShouldStopPagination(InstagramFriendshipResponse? data) =>
        data is null || !data.HasMore || string.IsNullOrEmpty(data.NextMaxId);

    private readonly string _userId = _options.Value.UserId;
    private readonly int _delayMinBetweenRequestsMs = _options.Value.DelayMinBetweenRequestsMs;
    private readonly int _delayMaxBetweenRequestsMs = _options.Value.DelayMaxBetweenRequestsMs;
    private readonly int _retryDelayMs = _options.Value.RetryDelayMs;
    private readonly int _maxRetryAttempts = _options.Value.MaxRetryAttempts;
}

