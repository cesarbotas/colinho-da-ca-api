using ColinhoDaCa.Application.UseCases.Reservas.v1.CadastrarReserva;
using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain.Clientes.Entities;
using ColinhoDaCa.Domain.Clientes.Repositories;
using ColinhoDaCa.Domain.Pets.Entities;
using ColinhoDaCa.Domain.Pets.Repositories;
using ColinhoDaCa.Domain.Reservas.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.UseCases;

public class CadastrarReservaServiceTests
{
    private readonly Mock<ILogger<CadastrarReservaService>> _loggerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IReservaRepository> _reservaRepositoryMock;
    private readonly Mock<IClienteRepository> _clienteRepositoryMock;
    private readonly Mock<IPetRepository> _petRepositoryMock;
    private readonly CadastrarReservaService _service;

    public CadastrarReservaServiceTests()
    {
        _loggerMock = new Mock<ILogger<CadastrarReservaService>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _reservaRepositoryMock = new Mock<IReservaRepository>();
        _clienteRepositoryMock = new Mock<IClienteRepository>();
        _petRepositoryMock = new Mock<IPetRepository>();

        _service = new CadastrarReservaService(
            _loggerMock.Object,
            _unitOfWorkMock.Object,
            _reservaRepositoryMock.Object,
            _clienteRepositoryMock.Object,
            _petRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateReserva()
    {
        // Arrange
        var command = new CadastrarReservaCommand
        {
            ClienteId = 1,
            DataInicial = DateTime.Today.AddDays(1),
            DataFinal = DateTime.Today.AddDays(3),
            Observacoes = "Reserva teste",
            PetIds = new List<long> { 1, 2 }
        };

        var cliente = Cliente.Create("João", "joao@test.com", "11999999999", "12345678901", "Test");
        var pets = new List<Pet>
        {
            Pet.Create("Rex", 1, 3, 15.5, "Médio", "Test", 1),
            Pet.Create("Max", 2, 2, 8.0, "Pequeno", "Test", 1)
        };

        _clienteRepositoryMock.Setup(x => x.GetAsync(command.ClienteId))
            .ReturnsAsync(cliente);
        _petRepositoryMock.Setup(x => x.GetByIdsAsync(command.PetIds))
            .ReturnsAsync(pets);

        // Act
        await _service.Handle(command);

        // Assert
        _reservaRepositoryMock.Verify(x => x.InsertAsync(It.IsAny<ReservaDb>()), Times.Once());
        _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once());
    }

    [Fact]
    public async Task Handle_ClienteNotFound_ShouldThrowException()
    {
        // Arrange
        var command = new CadastrarReservaCommand
        {
            ClienteId = 999,
            DataInicial = DateTime.Today.AddDays(1),
            DataFinal = DateTime.Today.AddDays(3),
            PetIds = new List<long> { 1 }
        };

        _clienteRepositoryMock.Setup(x => x.GetAsync(command.ClienteId))
            .ReturnsAsync((Cliente?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.Handle(command));
        exception.Message.Should().Contain("Cliente não encontrado");
    }
}