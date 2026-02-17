using ColinhoDaCa.Application.UseCases.Reservas.v1.AlterarReserva;
using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain.Reservas.Entities;
using ColinhoDaCa.Domain.Reservas.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.UseCases;

public class AlterarReservaServiceTests
{
    private readonly Mock<ILogger<AlterarReservaService>> _loggerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IReservaRepository> _reservaRepositoryMock;
    private readonly AlterarReservaService _service;

    public AlterarReservaServiceTests()
    {
        _loggerMock = new Mock<ILogger<AlterarReservaService>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _reservaRepositoryMock = new Mock<IReservaRepository>();

        _service = new AlterarReservaService(
            _loggerMock.Object,
            _unitOfWorkMock.Object,
            _reservaRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldUpdateReserva()
    {
        // Arrange
        var reservaId = 1L;
        var command = new AlterarReservaCommand
        {
            ClienteId = 1,
            DataInicial = DateTime.Today.AddDays(2),
            DataFinal = DateTime.Today.AddDays(5),
            Observacoes = "Reserva alterada",
            PetIds = new List<long> { 1, 2 }
        };

        var reserva = ReservaDb.Create(1, DateTime.Today.AddDays(1), DateTime.Today.AddDays(3), 2, 1, 200m, 0m, 200m, null, "Test", new List<long> { 1 }, 1);

        _reservaRepositoryMock.Setup(x => x.GetAsync(reservaId))
            .ReturnsAsync(reserva);

        // Act
        await _service.Handle(reservaId, command);

        // Assert
        _reservaRepositoryMock.Verify(x => x.Update(reserva), Times.Once());
        _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once());
    }

    [Fact]
    public async Task Handle_ReservaNotFound_ShouldThrowException()
    {
        // Arrange
        var reservaId = 999L;
        var command = new AlterarReservaCommand();

        _reservaRepositoryMock.Setup(x => x.GetAsync(reservaId))
            .ReturnsAsync((ReservaDb?)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.Handle(reservaId, command));
    }
}