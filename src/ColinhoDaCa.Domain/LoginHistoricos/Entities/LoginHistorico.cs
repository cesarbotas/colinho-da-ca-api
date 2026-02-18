namespace ColinhoDaCa.Domain.LoginHistoricos.Entities;

public class LoginHistorico
{
    public long Id { get; set; }
    public long UsuarioId { get; set; }
    public string Email { get; set; }
    public string? UserAgent { get; set; }
    public string? Platform { get; set; }
    public string? Language { get; set; }
    public string? ScreenResolution { get; set; }
    public string? Timezone { get; set; }
    public string? ClientIP { get; set; }
    public DateTime DataLogin { get; set; }
}