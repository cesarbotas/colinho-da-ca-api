namespace ColinhoDaCa.Application.UseCases.Pets.v1.CadastrarPet;

public class CadastrarPetCommand
{
    public string Nome { get; set; }
    public string Raca { get; set; }
    public int Idade { get; set; }
    public double Peso { get; set; }
    public string Porte { get; set; }
    public string Observacoes { get; set; }
    public long ClienteId { get; set; }
}