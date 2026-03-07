namespace FollowersApi.Models;

public class InstagramOptions
{
    /// <summary>
    /// URL base da API do Instagram.
    /// </summary>
    public string BaseUrl { get; set; } = string.Empty;

    /// <summary>
    /// ID numérico da sua conta Instagram (por exemplo: 400339888).
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Cabeçalho Cookie completo copiado do DevTools (contendo sessionid, csrftoken, etc.).
    /// </summary>
    public string Cookie { get; set; } = string.Empty;

    /// <summary>
    /// Valor do cabeçalho x-ig-app-id.
    /// </summary>
    public string IgAppId { get; set; } = string.Empty;
}

