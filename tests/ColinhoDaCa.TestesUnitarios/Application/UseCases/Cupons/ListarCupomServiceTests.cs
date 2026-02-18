using ColinhoDaCa.Application._Shared.DTOs.Paginacao;
using ColinhoDaCa.Application.UseCases.Cupons.v1.ListarCupom;
using ColinhoDaCa.Domain.Cupons.Entities;
using ColinhoDaCa.Domain.Cupons.Enums;
using ColinhoDaCa.Domain.Cupons.Repositories;
using Moq;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.Application.UseCases.Cupons;

public class ListarCupomServiceTests
{
    private readonly Mock<ICupomRepository> _cupomRepositoryMock;
    private readonly ListarCupomService _service;

    public ListarCupomServiceTests()
    {
        _cupomRepositoryMock = new Mock<ICupomRepository>();
        _service = new ListarCupomService(_cupomRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_DeveRetornarCuponsPaginados()
    {
        var cupons = new List<Cupom>
        {
            Cupom.Create("CUPOM10", "Desconto 10%", TipoCupom.PercentualSobreTotal, 10, null, null, null, null, null, null)
        };
        var query = new ListarCupomQuery { Paginacao = new PaginacaoDto { NumeroPagina = 1, QuantidadeRegistros = 10 } };
        _cupomRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(cupons);

        var result = await _service.Handle(query);

        Assert.NotNull(result);
        Assert.Equal(1, result.Total);
    }
}
