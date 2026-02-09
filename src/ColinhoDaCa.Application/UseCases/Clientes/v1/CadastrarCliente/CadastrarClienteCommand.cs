namespace ColinhoDaCa.Application.UseCases.Clientes.v1.CadastrarCliente;

public class CadastrarClienteCommand
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Celular { get; set; }
    public string Cpf { get; set; }
    public string Endereco { get; set; }
    public string Observacoes { get; set; }
}