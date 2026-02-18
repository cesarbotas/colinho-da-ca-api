namespace ColinhoDaCa.Application.UseCases.Reservas.v1.AplicarCupom;

public class AplicarCupomResponse
{
    public decimal ValorTotal { get; set; }
    public decimal ValorDesconto { get; set; }
    public decimal ValorFinal { get; set; }
    public string CupomAplicado { get; set; }
}
