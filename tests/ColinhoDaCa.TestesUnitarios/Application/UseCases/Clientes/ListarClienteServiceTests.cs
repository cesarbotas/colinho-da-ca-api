using ColinhoDaCa.Application._Shared.DTOs.Paginacao;
using ColinhoDaCa.Application.DTOs.Clientes;
using ColinhoDaCa.Application.UseCases.Clientes.v1.ListarCliente;
using ColinhoDaCa.Domain.Clientes.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.Application.UseCases.Clientes;

public class ListarClienteServiceTests
{
    private readonly Mock<ILogger<ListarClienteService>> _loggerMock;
    private readonly Mock<IClienteReadRepository> _clienteReadRepositoryMock;
    private readonly ListarClienteService _service;

    public ListarClienteServiceTests()
    {
        _loggerMock = new Mock<ILogger<ListarClienteService>>();
        _clienteReadRepositoryMock = new Mock<IClienteReadRepository>();
        _service = new ListarClienteService(_loggerMock.Object, _clienteReadRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_DeveRetornarClientesPaginados()
    {
        var query = new ListarClienteQuery { Paginacao = new PaginacaoDto { NumeroPagina = 1, QuantidadeRegistros = 10 } };
        var resultado = new ResultadoPaginadoDto<ClientesDto> { Page = 1, PageSize = 10, Total = 1, Data = new List<ClientesDto>() };
        _clienteReadRepositoryMock.Setup(x => x.PesquisarClientesDto(query)).ReturnsAsync(resultado);

        var result = await _service.Handle(query);

        Assert.NotNull(result);
        Assert.Equal(1, result.Page);
    }
}
