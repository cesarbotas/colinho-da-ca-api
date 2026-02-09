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

    public static ClienteDb Create(string nome, string email, string celular, string cpf, string endereco, string obs)
    {
        return new ClienteDb
        {
            Nome = nome,
            Email = email,
            Celular = celular,
            Cpf = cpf,
            Endereco = endereco,
            Observacoes = obs
        };
    }

    public void Alterar(string nome, string email, string celular, string cpf, string endereco, string obs)
    {
        Nome = nome;
        Email = email;
        Celular = celular;
        Cpf = cpf;
        Endereco = endereco;
        Observacoes = obs;
    }
}