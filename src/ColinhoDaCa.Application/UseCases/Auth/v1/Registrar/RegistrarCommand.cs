namespace ColinhoDaCa.Application.UseCases.Auth.v1.Registrar;

public class RegistrarCommand
{
    public string Nome { get; set; }
    public string Cpf { get; set; }
    public string Celular { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
}