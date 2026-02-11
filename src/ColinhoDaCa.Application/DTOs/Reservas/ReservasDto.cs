namespace ColinhoDaCa.Application.DTOs.Reservas;

public class ReservasDto
{
    public long Id { get; set; }
    public long ClienteId { get; set; }
    public string ClienteNome { get; set; }
    public DateTime DataInicial { get; set; }
    public DateTime DataFinal { get; set; }
    public int QuantidadeDiarias { get; set; }
    public int QuantidadePets { get; set; }
    public decimal ValorTotal { get; set; }
    public string Observacoes { get; set; }
    public List<PetReservaDto> Pets { get; set; }
}

public class PetReservaDto
{
    public long Id { get; set; }
    public string Nome { get; set; }
}
