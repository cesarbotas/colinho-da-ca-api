using ColinhoDaCa.Application._Shared.DTOs.Paginacao;
using ColinhoDaCa.Application.DTOs.Clientes;
using ColinhoDaCa.Application.UseCases.Clientes.v1.ListarCliente;
using ColinhoDaCa.Domain.Clientes.Entities;

namespace ColinhoDaCa.Domain.Clientes.Repositories;

public interface IClienteReadRepository
{
    IQueryable<ClienteDb> AsQueryable();
    Task<ResultadoPaginadoDto<ClientesDto>> PesquisarClientesDto(ListarClienteQuery query);
}