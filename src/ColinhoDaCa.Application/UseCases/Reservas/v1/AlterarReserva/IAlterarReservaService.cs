namespace ColinhoDaCa.Application.UseCases.Reservas.v1.AlterarReserva;

public interface IAlterarReservaService
{
    Task Handle(long id, AlterarReservaCommand command);
}