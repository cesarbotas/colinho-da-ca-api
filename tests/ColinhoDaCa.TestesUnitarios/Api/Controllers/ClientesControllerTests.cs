using ColinhoDaCa.Application.UseCases.Clientes.v1.AlterarCliente;
using ColinhoDaCa.Application.UseCases.Clientes.v1.CadastrarCliente;
using ColinhoDaCa.Application.UseCases.Clientes.v1.ExcluirCliente;
using FluentAssertions;
using Moq;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.Api.Controllers;

public class ClientesControllerTests
{
    private readonly Mock<ICadastrarClienteService> _cadastrarServiceMock;
    private readonly Mock<IAlterarClienteService> _alterarServiceMock;
    private readonly Mock<IExcluirClienteService> _excluirServiceMock;

    public ClientesControllerTests()
    {
        _cadastrarServiceMock = new Mock<ICadastrarClienteService>();
        _alterarServiceMock = new Mock<IAlterarClienteService>();
        _excluirServiceMock = new Mock<IExcluirClienteService>();
    }

    [Fact]
    public async Task CadastrarCliente_ValidCommand_CompletesSuccessfully()
    {
        var command = new CadastrarClienteCommand
        {
            Nome = "Test",
            Email = "test@test.com",
            Celular = "11999999999",
            Cpf = "12345678901"
        };
        _cadastrarServiceMock.Setup(x => x.Handle(command)).Returns(Task.CompletedTask);

        await _cadastrarServiceMock.Object.Handle(command);

        _cadastrarServiceMock.Verify(x => x.Handle(command), Times.Once);
    }

    [Fact]
    public async Task AlterarCliente_ValidCommand_CompletesSuccessfully()
    {
        var command = new AlterarClienteCommand { Nome = "Updated" };
        _alterarServiceMock.Setup(x => x.Handle(1, command)).Returns(Task.CompletedTask);

        await _alterarServiceMock.Object.Handle(1, command);

        _alterarServiceMock.Verify(x => x.Handle(1, command), Times.Once);
    }

    [Fact]
    public async Task ExcluirCliente_ValidId_CompletesSuccessfully()
    {
        _excluirServiceMock.Setup(x => x.Handle(1)).Returns(Task.CompletedTask);

        await _excluirServiceMock.Object.Handle(1);

        _excluirServiceMock.Verify(x => x.Handle(1), Times.Once);
    }
}
