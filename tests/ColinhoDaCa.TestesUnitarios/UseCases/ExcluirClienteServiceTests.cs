using ColinhoDaCa.Application.UseCases.Clientes.v1.ExcluirCliente;
using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain.Clientes.Entities;
using ColinhoDaCa.Domain.Clientes.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.UseCases;

public class ExcluirClienteServiceTests
{
    private readonly Mock<ILogger<ExcluirClienteService>> _loggerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IClienteRepository> _clienteRepositoryMock;
    private readonly ExcluirClienteService _service;

    public ExcluirClienteServiceTests()
    {
        _loggerMock = new Mock<ILogger<ExcluirClienteService>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _clienteRepositoryMock = new Mock<IClienteRepository>();

        _service = new ExcluirClienteService(
            _loggerMock.Object,
            _unitOfWorkMock.Object,
            _clienteRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidId_ShouldDeleteCliente()
    {
        // Arrange
        var clienteId = 1L;
        var cliente = Cliente.Create("João", "joao@test.com", "11999999999", "12345678901", "Test");

        _clienteRepositoryMock.Setup(x => x.GetAsync(clienteId))
            .ReturnsAsync(cliente);

        // Act
        await _service.Handle(clienteId);

        // Assert
        _clienteRepositoryMock.Verify(x => x.Delete(cliente), Times.Once());
        _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once());
    }

    [Fact]
    public async Task Handle_ClienteNotFound_ShouldThrowException()
    {
        // Arrange
        var clienteId = 999L;

        _clienteRepositoryMock.Setup(x => x.GetAsync(clienteId))
            .ReturnsAsync((Cliente?)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.Handle(clienteId));
    }
}