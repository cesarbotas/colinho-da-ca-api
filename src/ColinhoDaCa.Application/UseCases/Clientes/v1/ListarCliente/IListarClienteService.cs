using ColinhoDaCa.Application._Shared.DTOs.Paginacao;
using ColinhoDaCa.Application.DTOs.Clientes;

namespace ColinhoDaCa.Application.UseCases.Clientes.v1.ListarCliente;

public interface IListarClienteService
{
    Task<ResultadoPaginadoDto<ClientesDto>> Handle(ListarClienteQuery query);
}