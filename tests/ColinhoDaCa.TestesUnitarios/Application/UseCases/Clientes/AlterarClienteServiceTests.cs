using ColinhoDaCa.Application.UseCases.Clientes.v1.AlterarCliente;
using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain._Shared.Exceptions;
using ColinhoDaCa.Domain.Clientes.Entities;
using ColinhoDaCa.Domain.Clientes.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.Application.UseCases.Clientes;

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
        var cliente = Cliente.Create("Old Name", "old@test.com", "11999999999", "12345678901", "Old");
        var command = new AlterarClienteCommand
        {
            Nome = "New Name",
            Email = "new@test.com",
            Celular = "11988888888",
            Cpf = "12345678901",
            Observacoes = "New"
        };

        _clienteRepositoryMock.Setup(x => x.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Cliente, bool>>>(), It.IsAny<bool>()))
            .ReturnsAsync(cliente);
        _clienteRepositoryMock.Setup(x => x.GetByEmailAsync(command.Email)).ReturnsAsync((Cliente?)null);
        _clienteRepositoryMock.Setup(x => x.GetByCpfAsync(command.Cpf)).ReturnsAsync((Cliente?)null);

        await _service.Handle(cliente.Id, command);

        _clienteRepositoryMock.Verify(x => x.Update(It.IsAny<Cliente>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ClienteNotFound_ShouldThrowEntityNotFoundException()
    {
        var command = new AlterarClienteCommand();
        _clienteRepositoryMock.Setup(x => x.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Cliente, bool>>>(), It.IsAny<bool>()))
            .ReturnsAsync((Cliente?)null);

        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.Handle(999, command));
        exception.Message.Should().Be("Cliente não encontrado");
    }
}
