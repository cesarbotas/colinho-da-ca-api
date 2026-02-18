using ColinhoDaCa.Application.UseCases.Reservas.v1.AplicarCupom;
using ColinhoDaCa.Domain._Shared.Exceptions;
using ColinhoDaCa.Domain.Cupons.Entities;
using ColinhoDaCa.Domain.Cupons.Enums;
using ColinhoDaCa.Domain.Cupons.Repositories;
using Moq;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.Application.UseCases.Reservas;

public class AplicarCupomServiceTests
{
    private readonly Mock<ICupomRepository> _cupomRepositoryMock;
    private readonly AplicarCupomService _service;

    public AplicarCupomServiceTests()
    {
        _cupomRepositoryMock = new Mock<ICupomRepository>();
        _service = new AplicarCupomService(_cupomRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_CupomValido_DeveRetornarDesconto()
    {
        var cupom = Cupom.Create("CUPOM10", "Desconto 10%", TipoCupom.PercentualSobreTotal, 10, null, null, null, null, null, null);
        var command = new AplicarCupomCommand { CodigoCupom = "CUPOM10", ValorTotal = 100, QuantidadePets = 1, QuantidadeDiarias = 5 };
        _cupomRepositoryMock.Setup(x => x.GetByCodigoAsync("CUPOM10")).ReturnsAsync(cupom);

        var result = await _service.Handle(1, command);

        Assert.NotNull(result);
        Assert.Equal(10, result.ValorDesconto);
    }

    [Fact]
    public async Task Handle_CupomNaoEncontrado_DeveLancarExcecao()
    {
        var command = new AplicarCupomCommand { CodigoCupom = "INVALIDO" };
        _cupomRepositoryMock.Setup(x => x.GetByCodigoAsync("INVALIDO")).ReturnsAsync((Cupom?)null);

        await Assert.ThrowsAsync<ValidationException>(() => _service.Handle(1, command));
    }

    [Fact]
    public async Task Handle_CupomInativo_DeveLancarExcecao()
    {
        var cupom = Cupom.Create("CUPOM10", "Desconto 10%", TipoCupom.PercentualSobreTotal, 10, null, null, null, null, null, null);
        cupom.Inativar();
        var command = new AplicarCupomCommand { CodigoCupom = "CUPOM10" };
        _cupomRepositoryMock.Setup(x => x.GetByCodigoAsync("CUPOM10")).ReturnsAsync(cupom);

        await Assert.ThrowsAsync<ValidationException>(() => _service.Handle(1, command));
    }
}
