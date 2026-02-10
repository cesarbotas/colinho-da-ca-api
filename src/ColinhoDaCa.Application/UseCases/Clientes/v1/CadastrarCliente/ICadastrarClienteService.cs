using ColinhoDaCa.Domain.Clientes.Entities;

namespace ColinhoDaCa.Application.UseCases.Clientes.v1.CadastrarCliente;

public interface ICadastrarClienteService
{
    Task Handle(CadastrarClienteCommand command);
}