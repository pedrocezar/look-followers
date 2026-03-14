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

    /// <summary> 
    /// Tempo mínimo de espera (em ms) entre chamadas à API do Instagram. Configurar um valor alto (ex: 30000) é recomendado para evitar bloqueios. 
    /// </summary>
    public int DelayMinBetweenRequestsMs { get; set; } = 1000;

    /// <summary> Tempo máximo de espera (em ms) entre chamadas à API do Instagram. Configurar um valor alto (ex: 60000) é recomendado para evitar bloqueios. </summary>
    public int DelayMaxBetweenRequestsMs { get; set; } = 10000;

    /// <summary>
    /// Tempo fixo de espera (em ms) entre tentativas de retry quando uma chamada à API falhar.
    /// </summary>
    public int RetryDelayMs { get; set; } = 2000;

    /// <summary>
    /// Número máximo de tentativas ao chamar a API (incluindo a primeira tentativa).
    /// </summary>
    public int MaxRetryAttempts { get; set; } = 3;

    /// <summary> 
    /// Número máximo de conexões simultâneas com o servidor do Instagram. Configurar como 1 é recomendado para evitar bloqueios. 
    /// </summary>
    public int MaxConnectionsPerServer { get; set; } = 1;

    /// <summary> 
    /// Tempo de vida (em minutos) para conexões HTTP persistentes. Configurar como 1 minuto é recomendado para evitar bloqueios. 
    /// </summary>
    public int PooledConnectionLifetimeMinutes { get; set; } = 1;
}

