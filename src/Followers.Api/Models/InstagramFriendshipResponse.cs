using System.Text.Json.Serialization;

namespace FollowersApi.Models;

public class InstagramFriendshipResponse
{
    [JsonPropertyName("users")]
    public List<InstagramUser> Users { get; set; } = new();

    [JsonPropertyName("big_list")]
    public bool BigList { get; set; }

    [JsonPropertyName("page_size")]
    public int PageSize { get; set; }

    [JsonPropertyName("next_max_id")]
    public string? NextMaxId { get; set; }

    [JsonPropertyName("has_more")]
    public bool HasMore { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }
}

