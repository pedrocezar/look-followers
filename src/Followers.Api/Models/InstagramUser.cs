using System.Text.Json.Serialization;

namespace FollowersApi.Models;

public class InstagramUser
{
    [JsonPropertyName("pk")]
    public string Pk { get; set; } = string.Empty;

    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    [JsonPropertyName("full_name")]
    public string FullName { get; set; } = string.Empty;

    [JsonPropertyName("is_private")]
    public bool IsPrivate { get; set; }

    [JsonPropertyName("is_verified")]
    public bool IsVerified { get; set; }
}

