namespace ColinhoDaCa.Domain.RefreshTokens.Entities;

public class RefreshTokenDb
{
    public long Id { get; set; }
    public long UsuarioId { get; set; }
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? RevokedAt { get; set; }
}