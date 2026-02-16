namespace ColinhoDaCa.Domain.Clientes.Entities;

public class Cliente
{
    public long Id { get; protected set; }
    public string Nome { get; protected set; }
    public string Email { get; protected set; }
    public string Celular { get; protected set; }
    public string Cpf { get; protected set; }
    public string Observacoes { get; protected set; }
    public DateTime DataInclusao { get; protected set; }
    public DateTime DataAlteracao { get; protected set; }

    protected Cliente()
    {
        Id = default!;
        Nome = default!;
        Email = default!;
        Celular = default!;
        Cpf = default!;
        Observacoes = default!;
    }
    
    private Cliente(string nome, string email, string celular, string cpf, string obs)
    {
        var now = DateTime.Now;

        Nome = nome ?? throw new ArgumentNullException(nameof(nome));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        Celular = celular ?? throw new ArgumentNullException(nameof(celular));
        Cpf = cpf ?? throw new ArgumentNullException(nameof(cpf));
        Observacoes = obs ?? throw new ArgumentNullException(nameof(obs));
        DataInclusao = now;
        DataAlteracao = now;
    }

    public static Cliente Create(string nome, string email, string celular, string cpf, string obs)
    {
        return new Cliente(nome, email, celular, cpf, obs);
    }

    public void Alterar(string nome, string email, string celular, string cpf, string obs)
    {
        Nome = nome ?? throw new ArgumentNullException(nameof(nome));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        Celular = celular ?? throw new ArgumentNullException(nameof(celular));
        Cpf = cpf ?? throw new ArgumentNullException(nameof(cpf));
        Observacoes = obs ?? throw new ArgumentNullException(nameof(obs));
        DataAlteracao = DateTime.Now;
    }
}