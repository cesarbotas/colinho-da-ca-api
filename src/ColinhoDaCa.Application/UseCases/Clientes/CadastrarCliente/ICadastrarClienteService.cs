using ColinhoDaCa.Domain.Clientes.Entities;

namespace ColinhoDaCa.Application.UseCases.Clientes.CadastrarCliente;

public interface ICadastrarClienteService
{
    Task Execute(CadastrarClienteCommand command);
}
