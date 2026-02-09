namespace ColinhoDaCa.Domain.Pets.Entities;

public class PetDb
{
    public long Id { get; set; }
    public string Nome { get; set; }
    public string Raca { get; set; }
    public int Idade { get; set; }
    public int Peso { get; set; }
    public string Observacoes { get; set; }
    public long ClienteId { get; set; }

    public PetDb()
    {
        
    }

    public static PetDb Create(string nome, string raca, int idade, int peso, string obs, long clienteId)
    {
        return new PetDb
        {
            Nome = nome,
            Raca = raca,
            Idade = idade,
            Peso = peso,
            Observacoes = obs,
            ClienteId = clienteId
        };
    }

    public void Alterar(string nome, string raca, int idade, int peso, string obs, long clienteId)
    {
        Nome = nome;
        Raca = raca;
        Idade = idade;
        Peso = peso;
        Observacoes = obs;
        ClienteId = clienteId;
    }
}