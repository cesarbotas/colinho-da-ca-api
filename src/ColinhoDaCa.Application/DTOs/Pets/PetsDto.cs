namespace ColinhoDaCa.Application.DTOs.Pets;

public class PetsDto
{
    public long Id { get; set; }
    public string Nome { get; set; }
    public long? RacaId { get; set; }
    public string RacaNome { get; set; }
    public int Idade { get; set; }
    public double Peso { get; set; }
    public string Porte { get; set; }
    public string Observacoes { get; set; }
    public long ClienteId { get; set; }
    public string ClienteNome { get; set; }
    public bool Ativo { get; set; }
}