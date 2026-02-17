using ColinhoDaCa.Application.UseCases.Pets.v1.CadastrarPet;
using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain.Clientes.Entities;
using ColinhoDaCa.Domain.Clientes.Repositories;
using ColinhoDaCa.Domain.Pets.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.UseCases;

public class CadastrarPetServiceTests
{
    private readonly Mock<ILogger<CadastrarPetService>> _loggerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IPetRepository> _petRepositoryMock;
    private readonly Mock<IClienteRepository> _clienteRepositoryMock;
    private readonly CadastrarPetService _service;

    public CadastrarPetServiceTests()
    {
        _loggerMock = new Mock<ILogger<CadastrarPetService>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _petRepositoryMock = new Mock<IPetRepository>();
        _clienteRepositoryMock = new Mock<IClienteRepository>();

        _service = new CadastrarPetService(
            _loggerMock.Object,
            _unitOfWorkMock.Object,
            _petRepositoryMock.Object,
            _clienteRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreatePet()
    {
        // Arrange
        var command = new CadastrarPetCommand
        {
            Nome = "Rex",
            RacaId = 1,
            Idade = 3,
            Peso = 15.5,
            Porte = "Médio",
            Observacoes = "Pet dócil",
            ClienteId = 1
        };

        var cliente = Cliente.Create("João", "joao@test.com", "11999999999", "12345678901", "Test");

        _clienteRepositoryMock.Setup(x => x.GetAsync(command.ClienteId))
            .ReturnsAsync(cliente);

        // Act
        await _service.Handle(command);

        // Assert
        _petRepositoryMock.Verify(x => x.InsertAsync(It.IsAny<Pet>()), Times.Once());
        _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once());
    }

    [Fact]
    public async Task Handle_ClienteNotFound_ShouldThrowException()
    {
        // Arrange
        var command = new CadastrarPetCommand { ClienteId = 999 };

        _clienteRepositoryMock.Setup(x => x.GetAsync(command.ClienteId))
            .ReturnsAsync((Cliente?)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.Handle(command));
    }
}