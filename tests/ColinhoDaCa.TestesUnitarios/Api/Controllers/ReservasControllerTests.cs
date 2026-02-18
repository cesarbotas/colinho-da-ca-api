using ColinhoDaCa.Application.UseCases.Reservas.v1.AlterarReserva;
using ColinhoDaCa.Application.UseCases.Reservas.v1.CadastrarReserva;
using ColinhoDaCa.Application.UseCases.Reservas.v1.CancelarReserva;
using ColinhoDaCa.Application.UseCases.Reservas.v1.ConfirmarReserva;
using FluentAssertions;
using Moq;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.Api.Controllers;

public class ReservasControllerTests
{
    private readonly Mock<ICadastrarReservaService> _cadastrarServiceMock;
    private readonly Mock<IAlterarReservaService> _alterarServiceMock;
    private readonly Mock<ICancelarReservaService> _cancelarServiceMock;
    private readonly Mock<IConfirmarReservaService> _confirmarServiceMock;

    public ReservasControllerTests()
    {
        _cadastrarServiceMock = new Mock<ICadastrarReservaService>();
        _alterarServiceMock = new Mock<IAlterarReservaService>();
        _cancelarServiceMock = new Mock<ICancelarReservaService>();
        _confirmarServiceMock = new Mock<IConfirmarReservaService>();
    }

    [Fact]
    public async Task CadastrarReserva_ValidCommand_CompletesSuccessfully()
    {
        var command = new CadastrarReservaCommand
        {
            ClienteId = 1,
            DataInicial = DateTime.Now,
            DataFinal = DateTime.Now.AddDays(5),
            QuantidadeDiarias = 5,
            QuantidadePets = 1,
            ValorTotal = 500,
            ValorFinal = 500,
            Observacoes = "Teste",
            PetIds = new List<long> { 1 }
        };
        _cadastrarServiceMock.Setup(x => x.Handle(command)).Returns(Task.CompletedTask);

        await _cadastrarServiceMock.Object.Handle(command);

        _cadastrarServiceMock.Verify(x => x.Handle(command), Times.Once);
    }

    [Fact]
    public async Task AlterarReserva_ValidCommand_CompletesSuccessfully()
    {
        var command = new AlterarReservaCommand { Observacoes = "Updated" };
        _alterarServiceMock.Setup(x => x.Handle(1, command)).Returns(Task.CompletedTask);

        await _alterarServiceMock.Object.Handle(1, command);

        _alterarServiceMock.Verify(x => x.Handle(1, command), Times.Once);
    }

    [Fact]
    public async Task CancelarReserva_ValidId_CompletesSuccessfully()
    {
        _cancelarServiceMock.Setup(x => x.Handle(1)).Returns(Task.CompletedTask);

        await _cancelarServiceMock.Object.Handle(1);

        _cancelarServiceMock.Verify(x => x.Handle(1), Times.Once);
    }

    [Fact]
    public async Task ConfirmarReserva_ValidId_CompletesSuccessfully()
    {
        _confirmarServiceMock.Setup(x => x.Handle(1)).Returns(Task.CompletedTask);

        await _confirmarServiceMock.Object.Handle(1);

        _confirmarServiceMock.Verify(x => x.Handle(1), Times.Once);
    }
}
