namespace ColinhoDaCa.Application.DTOs.Pets;

public class PetsDto
{
    public long Id { get; set; }
    public string Nome { get; set; }
    public string Raca { get; set; }
    public int Idade { get; set; }
    public string Observacoes { get; set; }
    public long ClienteId { get; set; }
    public string ClienteNome { get; set; }
}
