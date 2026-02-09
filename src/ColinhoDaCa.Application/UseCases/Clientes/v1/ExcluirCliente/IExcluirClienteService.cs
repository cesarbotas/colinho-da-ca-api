namespace ColinhoDaCa.Application.UseCases.Clientes.v1.ExcluirCliente;

public interface IExcluirClienteService
{
    Task Handle(long id);
}