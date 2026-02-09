namespace ColinhoDaCa.Domain.Pets.Entities;

public class PetDb
{
    public long Id { get; set; }
    public string Nome { get; set; }
    public string Raca { get; set; }
    public int Idade { get; set; }
    public double Peso { get; set; }
    public string Porte { get; set; }
    public string Observacoes { get; set; }
    public long ClienteId { get; set; }
    public DateTime DataInclusao { get; set; }
    public DateTime DataAlteracao { get; set; }

    public PetDb()
    {
        
    }

    public static PetDb Create(string nome, string raca, int idade, double peso, string porte, string obs, long clienteId)
    {
        var now = DateTime.Now;
        
        return new PetDb
        {
            Nome = nome,
            Raca = raca,
            Idade = idade,
            Peso = peso,
            Porte = porte,
            Observacoes = obs,
            ClienteId = clienteId,
            DataInclusao = now,
            DataAlteracao = now
        };
    }

    public void Alterar(string nome, string raca, int idade, double peso, string porte, string obs, long clienteId)
    {
        Nome = nome;
        Raca = raca;
        Idade = idade;
        Peso = peso;
        Porte = porte;
        Observacoes = obs;
        ClienteId = clienteId;
        DataAlteracao = DateTime.Now;
    }
}