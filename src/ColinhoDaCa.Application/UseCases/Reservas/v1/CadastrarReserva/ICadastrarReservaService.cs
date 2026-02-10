namespace ColinhoDaCa.Application.UseCases.Reservas.v1.CadastrarReserva;

public interface ICadastrarReservaService
{
    Task Handle(CadastrarReservaCommand command);
}