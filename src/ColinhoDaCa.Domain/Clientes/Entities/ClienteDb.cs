namespace ColinhoDaCa.Domain.Clientes.Entities;

public class ClienteDb
{
    public long Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Celular { get; set; }
    public string Cpf { get; set; }
    public string Observacoes { get; set; }
    public DateTime DataInclusao { get; set; }
    public DateTime DataAlteracao { get; set; }

    public ClienteDb()
    {
        
    }

    public static ClienteDb Create(string nome, string email, string celular, string cpf, string obs)
    {
        var now = DateTime.Now;

        return new ClienteDb
        {
            Nome = nome,
            Email = email,
            Celular = celular,
            Cpf = cpf,
            Observacoes = obs,
            DataInclusao = now,
            DataAlteracao = now
        };
    }

    public void Alterar(string nome, string email, string celular, string cpf, string obs)
    {
        Nome = nome;
        Email = email;
        Celular = celular;
        Cpf = cpf;
        Observacoes = obs;
        DataAlteracao = DateTime.Now;
    }
}