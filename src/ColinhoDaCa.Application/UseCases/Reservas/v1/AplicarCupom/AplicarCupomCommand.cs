namespace ColinhoDaCa.Application.UseCases.Reservas.v1.AplicarCupom;

public class AplicarCupomCommand
{
    public string CodigoCupom { get; set; }
    public decimal ValorTotal { get; set; }
    public int QuantidadePets { get; set; }
    public int QuantidadeDiarias { get; set; }
}
