namespace ColinhoDaCa.Application.UseCases.Reservas.v1.ExcluirReserva;

public interface IExcluirReservaService
{
    Task Handle(long id);
}