namespace ColinhoDaCa.Application.UseCases.Reservas.v1.CadastrarReserva;

public class CadastrarReservaCommand
{
    public long ClienteId { get; set; }
    public long UsuarioId { get; set; }
    public DateTime DataInicial { get; set; }
    public DateTime DataFinal { get; set; }
    public int QuantidadeDiarias { get; set; }
    public int QuantidadePets { get; set; }
    public decimal ValorTotal { get; set; }
    public string Observacoes { get; set; }
    public List<long> PetIds { get; set; }
}