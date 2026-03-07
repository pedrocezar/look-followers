using FollowersApi.Models;
using Refit;

namespace FollowersApi.Clients;

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
        [AliasAs("next_max_id")] string? nextMaxId = null,
        [AliasAs("search_surface")] string searchSurface = "follow_list_page");
}

