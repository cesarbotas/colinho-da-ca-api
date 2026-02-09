namespace ColinhoDaCa.Domain.Clientes.Entities;

public class ClienteDb
{
    public long Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Celular { get; set; }
    public string Cpf { get; set; }
    public string Endereco { get; set; }
    public string Observacoes { get; set; }

    public ClienteDb()
    {
        
    }

    public ClienteDb(CadastrarClienteCommand command)
    {
        Nome = command.Nome;
        Email = command.Email;
        Celular = command.Celular;
        Cpf = command.Cpf;
        Endereco = command.Endereco;
        Observacoes = command.Observacoes;
    }
}
