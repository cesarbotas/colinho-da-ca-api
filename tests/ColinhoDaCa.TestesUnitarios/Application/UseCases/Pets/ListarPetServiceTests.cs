using ColinhoDaCa.Application._Shared.DTOs.Paginacao;
using ColinhoDaCa.Application.DTOs.Pets;
using ColinhoDaCa.Application.UseCases.Pets.v1.ListarPet;
using ColinhoDaCa.Domain.Pets.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.Application.UseCases.Pets;

public class ListarPetServiceTests
{
    private readonly Mock<ILogger<ListarPetService>> _loggerMock;
    private readonly Mock<IPetReadRepository> _petReadRepositoryMock;
    private readonly ListarPetService _service;

    public ListarPetServiceTests()
    {
        _loggerMock = new Mock<ILogger<ListarPetService>>();
        _petReadRepositoryMock = new Mock<IPetReadRepository>();
        _service = new ListarPetService(_loggerMock.Object, _petReadRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_DeveRetornarPetsPaginados()
    {
        var query = new ListarPetQuery { Paginacao = new PaginacaoDto { NumeroPagina = 1, QuantidadeRegistros = 10 } };
        var resultado = new ResultadoPaginadoDto<PetsDto> { Page = 1, PageSize = 10, Total = 1, Data = new List<PetsDto>() };
        _petReadRepositoryMock.Setup(x => x.PesquisarPetsDto(query)).ReturnsAsync(resultado);

        var result = await _service.Handle(query);

        Assert.NotNull(result);
        Assert.Equal(1, result.Page);
    }
}
