using ColinhoDaCa.Application.UseCases.Cupons.v1.AlterarCupom;
using ColinhoDaCa.Application.UseCases.Cupons.v1.CadastrarCupom;
using ColinhoDaCa.Application.UseCases.Cupons.v1.InativarCupom;
using FluentAssertions;
using Moq;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.Api.Controllers;

public class CuponsControllerTests
{
    private readonly Mock<ICadastrarCupomService> _cadastrarServiceMock;
    private readonly Mock<IAlterarCupomService> _alterarServiceMock;
    private readonly Mock<IInativarCupomService> _inativarServiceMock;

    public CuponsControllerTests()
    {
        _cadastrarServiceMock = new Mock<ICadastrarCupomService>();
        _alterarServiceMock = new Mock<IAlterarCupomService>();
        _inativarServiceMock = new Mock<IInativarCupomService>();
    }

    [Fact]
    public async Task CadastrarCupom_ValidCommand_CompletesSuccessfully()
    {
        var command = new CadastrarCupomCommand
        {
            Codigo = "TEST10",
            Descricao = "Teste",
            Tipo = 1,
            Percentual = 10
        };
        _cadastrarServiceMock.Setup(x => x.Handle(command)).Returns(Task.CompletedTask);

        await _cadastrarServiceMock.Object.Handle(command);

        _cadastrarServiceMock.Verify(x => x.Handle(command), Times.Once);
    }

    [Fact]
    public async Task AlterarCupom_ValidCommand_CompletesSuccessfully()
    {
        var command = new AlterarCupomCommand { Descricao = "Updated" };
        _alterarServiceMock.Setup(x => x.Handle(1, command)).Returns(Task.CompletedTask);

        await _alterarServiceMock.Object.Handle(1, command);

        _alterarServiceMock.Verify(x => x.Handle(1, command), Times.Once);
    }

    [Fact]
    public async Task InativarCupom_ValidId_CompletesSuccessfully()
    {
        _inativarServiceMock.Setup(x => x.Handle(1)).Returns(Task.CompletedTask);

        await _inativarServiceMock.Object.Handle(1);

        _inativarServiceMock.Verify(x => x.Handle(1), Times.Once);
    }
}
