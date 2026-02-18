namespace ColinhoDaCa.Application.UseCases.Reservas.v1.CancelarReserva;

public interface ICancelarReservaService
{
    Task Handle(long reservaId);
}
