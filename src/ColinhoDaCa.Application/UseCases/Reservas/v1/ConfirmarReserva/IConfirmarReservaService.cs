namespace ColinhoDaCa.Application.UseCases.Reservas.v1.ConfirmarReserva;

public interface IConfirmarReservaService
{
    Task Handle(long reservaId);
}
