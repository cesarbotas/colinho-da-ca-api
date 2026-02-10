namespace ColinhoDaCa.Application.UseCases.Auth.v1.Login;

public class LoginResponse
{
    public string Token { get; set; }
    public UsuarioResponse Usuario { get; set; }
}

public class UsuarioResponse
{
    public long Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
}
