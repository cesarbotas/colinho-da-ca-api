namespace ColinhoDaCa.Domain.Pets.Entities;

public class Pet
{
    public long Id { get; protected set; }
    public string Nome { get; protected set; }
    public long? RacaId { get; protected set; }
    public int Idade { get; protected set; }
    public double Peso { get; protected set; }
    public string Porte { get; protected set; }
    public string Observacoes { get; protected set; }
    public long ClienteId { get; protected set; }
    public DateTime DataInclusao { get; protected set; }
    public DateTime DataAlteracao { get; protected set; }
    
    protected Pet()
    {
        Id = default!;
        Nome = default!;
        Porte = default!;
        Observacoes = default!;
        ClienteId = default!;
    }
    
    private Pet(string nome, long? racaId, int idade, double peso, string porte, string obs, long clienteId)
    {
        var now = DateTime.Now;
        
        Nome = nome ?? throw new ArgumentNullException(nameof(nome));
        RacaId = racaId;
        Idade = idade;
        Peso = peso;
        Porte = porte ?? throw new ArgumentNullException(nameof(porte));
        Observacoes = obs ?? throw new ArgumentNullException(nameof(obs));
        ClienteId = clienteId;
        DataInclusao = now;
        DataAlteracao = now;
    }

    public static Pet Create(string nome, long? racaId, int idade, double peso, string porte, string obs, long clienteId)
    {
        return new Pet(nome, racaId, idade, peso, porte, obs, clienteId);
    }

    public void Alterar(string nome, long? racaId, int idade, double peso, string porte, string obs, long clienteId)
    {
        Nome = nome ?? throw new ArgumentNullException(nameof(nome));
        RacaId = racaId;
        Idade = idade;
        Peso = peso;
        Porte = porte ?? throw new ArgumentNullException(nameof(porte));
        Observacoes = obs ?? throw new ArgumentNullException(nameof(obs));
        ClienteId = clienteId;
        DataAlteracao = DateTime.Now;
    }
}