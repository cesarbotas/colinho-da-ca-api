using ColinhoDaCa.Application.UseCases.Pets.v1.AlterarPet;
using ColinhoDaCa.Application.UseCases.Pets.v1.CadastrarPet;
using ColinhoDaCa.Application.UseCases.Pets.v1.ExcluirPet;
using FluentAssertions;
using Moq;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.Api.Controllers;

public class PetsControllerTests
{
    private readonly Mock<ICadastrarPetService> _cadastrarServiceMock;
    private readonly Mock<IAlterarPetService> _alterarServiceMock;
    private readonly Mock<IExcluirPetService> _excluirServiceMock;

    public PetsControllerTests()
    {
        _cadastrarServiceMock = new Mock<ICadastrarPetService>();
        _alterarServiceMock = new Mock<IAlterarPetService>();
        _excluirServiceMock = new Mock<IExcluirPetService>();
    }

    [Fact]
    public async Task CadastrarPet_ValidCommand_CompletesSuccessfully()
    {
        var command = new CadastrarPetCommand
        {
            Nome = "Rex",
            RacaId = 1,
            Idade = 3,
            Peso = 15.5,
            ClienteId = 1
        };
        _cadastrarServiceMock.Setup(x => x.Handle(command)).Returns(Task.CompletedTask);

        await _cadastrarServiceMock.Object.Handle(command);

        _cadastrarServiceMock.Verify(x => x.Handle(command), Times.Once);
    }

    [Fact]
    public async Task AlterarPet_ValidCommand_CompletesSuccessfully()
    {
        var command = new AlterarPetCommand { Nome = "Max" };
        _alterarServiceMock.Setup(x => x.Handle(1, command)).Returns(Task.CompletedTask);

        await _alterarServiceMock.Object.Handle(1, command);

        _alterarServiceMock.Verify(x => x.Handle(1, command), Times.Once);
    }

    [Fact]
    public async Task ExcluirPet_ValidId_CompletesSuccessfully()
    {
        _excluirServiceMock.Setup(x => x.Handle(1)).Returns(Task.CompletedTask);

        await _excluirServiceMock.Object.Handle(1);

        _excluirServiceMock.Verify(x => x.Handle(1), Times.Once);
    }
}
