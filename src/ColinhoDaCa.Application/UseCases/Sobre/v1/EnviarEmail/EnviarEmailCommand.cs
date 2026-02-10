namespace ColinhoDaCa.Application.UseCases.Sobre.v1.EnviarEmail;

public class EnviarEmailCommand
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Celular { get; set; }
    public string Assunto { get; set; }
    public string Mensagem { get; set; }
}