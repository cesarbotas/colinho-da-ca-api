using ColinhoDaCa.Application._Shared.DTOs.Paginacao;
using ColinhoDaCa.Application.DTOs.Clientes;
using ColinhoDaCa.Domain.Clientes.Repositories;
using Microsoft.Extensions.Logging;

namespace ColinhoDaCa.Application.UseCases.Clientes.v1.ListarCliente;

public class ListarClienteService : IListarClienteService
{
    private readonly ILogger<ListarClienteService> _logger;
    private readonly IClienteReadRepository _clienteReadRepository;

    public ListarClienteService(ILogger<ListarClienteService> logger, 
        IClienteReadRepository clienteReadRepository)
    {
        _logger = logger;
        _clienteReadRepository = clienteReadRepository;
    }

    public async Task<ResultadoPaginadoDto<ClientesDto>> Handle(ListarClienteQuery query)
    {
        try
        {
            return await _clienteReadRepository.PesquisarClientesDto(query);
        }
        catch (Exception)
        {
            throw;
        }
    }
}