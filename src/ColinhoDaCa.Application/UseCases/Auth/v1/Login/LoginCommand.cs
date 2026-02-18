namespace ColinhoDaCa.Application.UseCases.Auth.v1.Login;

public class LoginCommand
{
    public string Email { get; set; }
    public string Senha { get; set; }
    public DeviceInfo? DeviceInfo { get; set; }
}

public class DeviceInfo
{
    public string? UserAgent { get; set; }
    public string? Platform { get; set; }
    public string? Language { get; set; }
    public string? ScreenResolution { get; set; }
    public string? Timezone { get; set; }
    public string? ClientIP { get; set; }
    public DateTime? Timestamp { get; set; }
}