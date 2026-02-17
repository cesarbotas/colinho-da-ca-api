using ColinhoDaCa.Application.UseCases.Clientes.v1.AlterarCliente;
using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain._Shared.Exceptions;
using ColinhoDaCa.Domain.Clientes.Entities;
using ColinhoDaCa.Domain.Clientes.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.UseCases;

public class AlterarClienteServiceTests
{
    private readonly Mock<ILogger<AlterarClienteService>> _loggerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IClienteRepository> _clienteRepositoryMock;
    private readonly AlterarClienteService _service;

    public AlterarClienteServiceTests()
    {
        _loggerMock = new Mock<ILogger<AlterarClienteService>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _clienteRepositoryMock = new Mock<IClienteRepository>();

        _service = new AlterarClienteService(
            _loggerMock.Object,
            _unitOfWorkMock.Object,
            _clienteRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldUpdateCliente()
    {
        // Arrange
        var clienteId = 1L;
        var command = new AlterarClienteCommand
        {
            Nome = "João Silva Alterado",
            Email = "joao.alterado@test.com",
            Celular = "11888888888",
            Cpf = "98765432100",
            Observacoes = "Cliente alterado"
        };

        var cliente = Cliente.Create("João", "joao@test.com", "11999999999", "12345678901", "Test");

        _clienteRepositoryMock.Setup(x => x.GetAsync(clienteId))
            .ReturnsAsync(cliente);

        // Act
        await _service.Handle(clienteId, command);

        // Assert
        _clienteRepositoryMock.Verify(x => x.Update(cliente), Times.Once());
        _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once());
        cliente.Nome.Should().Be(command.Nome);
        cliente.Email.Should().Be(command.Email);
    }

    [Fact]
    public async Task Handle_ClienteNotFound_ShouldThrowException()
    {
        // Arrange
        var clienteId = 999L;
        var command = new AlterarClienteCommand();

        _clienteRepositoryMock.Setup(x => x.GetAsync(clienteId))
            .ReturnsAsync((Cliente?)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.Handle(clienteId, command));
    }
}