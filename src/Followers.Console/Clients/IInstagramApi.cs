using Followers.Console.Models;
using Refit;

namespace Followers.Console.Clients;

public interface IInstagramApi
{
    [Get("/api/v1/friendships/{userId}/following/")]
    Task<InstagramFriendshipResponse> GetFollowingAsync(
        string userId,
        [AliasAs("count")] int count = 200,
        [AliasAs("max_id")] string? maxId = null);

    [Get("/api/v1/friendships/{userId}/followers/")]
    Task<InstagramFriendshipResponse> GetFollowersAsync(
        string userId,
        [AliasAs("count")] int count = 200,
        [AliasAs("max_id")] string? maxId = null);
}

