namespace ColinhoDaCa.Application.UseCases.Auth.v1.Login;

public class LoginResponse
{
    public string Token { get; set; }
    public UsuarioResponse Usuario { get; set; }
}

public class UsuarioResponse
{
    public long Id { get; set; }
    public long ClienteId { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Celular { get; set; }
    public string Cpf { get; set; }
    public List<PerfilResponse> Perfis { get; set; }
}

public class PerfilResponse
{
    public long Id { get; set; }
    public string Nome { get; set; }
}