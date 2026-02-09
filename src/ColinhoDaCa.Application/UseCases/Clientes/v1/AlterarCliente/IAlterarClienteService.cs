namespace ColinhoDaCa.Application.UseCases.Clientes.v1.AlterarCliente;

public interface IAlterarClienteService
{
    Task Handle(long id, AlterarClienteCommand command);
}