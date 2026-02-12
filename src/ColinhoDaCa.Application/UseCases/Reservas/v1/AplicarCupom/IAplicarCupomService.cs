namespace ColinhoDaCa.Application.UseCases.Reservas.v1.AplicarCupom;

public interface IAplicarCupomService
{
    Task<AplicarCupomResponse> Handle(long reservaId, AplicarCupomCommand command);
}
