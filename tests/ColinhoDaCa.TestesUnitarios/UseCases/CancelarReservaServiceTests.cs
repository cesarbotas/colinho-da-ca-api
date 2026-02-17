using ColinhoDaCa.Application.UseCases.Reservas.v1.CancelarReserva;
using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain.Reservas.Entities;
using ColinhoDaCa.Domain.Reservas.Enums;
using ColinhoDaCa.Domain.Reservas.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.UseCases;

public class CancelarReservaServiceTests
{
    private readonly Mock<ILogger<CancelarReservaService>> _loggerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IReservaRepository> _reservaRepositoryMock;
    private readonly CancelarReservaService _service;

    public CancelarReservaServiceTests()
    {
        _loggerMock = new Mock<ILogger<CancelarReservaService>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _reservaRepositoryMock = new Mock<IReservaRepository>();

        _service = new CancelarReservaService(
            _loggerMock.Object,
            _unitOfWorkMock.Object,
            _reservaRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidReserva_ShouldCancelReserva()
    {
        // Arrange
        var reservaId = 1L;
        var usuarioId = 1L;
        var reserva = ReservaDb.Create(1, DateTime.Today.AddDays(1), DateTime.Today.AddDays(3), 2, 1, 200m, 0m, 200m, null, "Test", new List<long> { 1 }, usuarioId);

        _reservaRepositoryMock.Setup(x => x.GetAsync(reservaId))
            .ReturnsAsync(reserva);

        // Act
        await _service.Handle(reservaId, usuarioId);

        // Assert
        _reservaRepositoryMock.Verify(x => x.Update(reserva), Times.Once());
        _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once());
        reserva.Status.Should().Be(ReservaStatus.ReservaCancelada);
    }

    [Fact]
    public async Task Handle_ReservaNotFound_ShouldThrowException()
    {
        // Arrange
        var reservaId = 999L;
        var usuarioId = 1L;

        _reservaRepositoryMock.Setup(x => x.GetAsync(reservaId))
            .ReturnsAsync((ReservaDb?)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.Handle(reservaId, usuarioId));
    }
}