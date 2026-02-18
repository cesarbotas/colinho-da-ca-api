using ColinhoDaCa.Application.UseCases.Cupons.v1.CadastrarCupom;
using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain._Shared.Exceptions;
using ColinhoDaCa.Domain.Cupons.Entities;
using ColinhoDaCa.Domain.Cupons.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.Application.UseCases.Cupons;

public class CadastrarCupomServiceTests
{
    private readonly Mock<ICupomRepository> _cupomRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CadastrarCupomService _service;

    public CadastrarCupomServiceTests()
    {
        _cupomRepositoryMock = new Mock<ICupomRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _service = new CadastrarCupomService(
            _cupomRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateCupom()
    {
        var command = new CadastrarCupomCommand
        {
            Codigo = "DESCONTO10",
            Descricao = "10% de desconto",
            Tipo = 1,
            Percentual = 10,
            DataInicio = DateTime.Now,
            DataFim = DateTime.Now.AddDays(30)
        };

        _cupomRepositoryMock.Setup(x => x.GetByCodigoAsync(command.Codigo))
            .ReturnsAsync((Cupom?)null);

        await _service.Handle(command);

        _cupomRepositoryMock.Verify(x => x.InsertAsync(It.IsAny<Cupom>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ExistingCodigo_ShouldThrowValidationException()
    {
        var command = new CadastrarCupomCommand { Codigo = "EXISTING" };
        var existingCupom = Cupom.Create("EXISTING", "Test", ColinhoDaCa.Domain.Cupons.Enums.TipoCupom.PercentualSobreTotal, 10, null, null, null, null, DateTime.Now, DateTime.Now.AddDays(30));

        _cupomRepositoryMock.Setup(x => x.GetByCodigoAsync(command.Codigo))
            .ReturnsAsync(existingCupom);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => _service.Handle(command));
        exception.Message.Should().Be("Código de cupom já existe");
    }
}
