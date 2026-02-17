namespace ColinhoDaCa.TestesIntegrados.Models;

public class LoginResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public int ExpiresIn { get; set; }
    public string TokenType { get; set; } = string.Empty;
}

public class RefreshTokenCommand
{
    public string RefreshToken { get; set; } = string.Empty;
}