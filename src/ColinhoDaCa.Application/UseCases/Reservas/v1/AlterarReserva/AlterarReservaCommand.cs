namespace ColinhoDaCa.Application.UseCases.Reservas.v1.AlterarReserva;

public class AlterarReservaCommand
{
    public long ClienteId { get; set; }
    public DateTime DataInicial { get; set; }
    public DateTime DataFinal { get; set; }
    public int QuantidadeDiarias { get; set; }
    public int QuantidadePets { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal ValorDesconto { get; set; }
    public decimal ValorFinal { get; set; }
    public long? CupomId { get; set; }
    public string Observacoes { get; set; }
    public List<long> PetIds { get; set; }
}