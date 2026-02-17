using ColinhoDaCa.Application.UseCases.Pets.v1.AlterarPet;
using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain.Pets.Entities;
using ColinhoDaCa.Domain.Pets.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.UseCases;

public class AlterarPetServiceTests
{
    private readonly Mock<ILogger<AlterarPetService>> _loggerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IPetRepository> _petRepositoryMock;
    private readonly AlterarPetService _service;

    public AlterarPetServiceTests()
    {
        _loggerMock = new Mock<ILogger<AlterarPetService>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _petRepositoryMock = new Mock<IPetRepository>();

        _service = new AlterarPetService(
            _loggerMock.Object,
            _unitOfWorkMock.Object,
            _petRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldUpdatePet()
    {
        // Arrange
        var petId = 1L;
        var command = new AlterarPetCommand
        {
            Nome = "Max Alterado",
            RacaId = 2,
            Idade = 4,
            Peso = 18.0,
            Porte = "Grande",
            Observacoes = "Pet alterado",
            ClienteId = 1
        };

        var pet = Pet.Create("Max", 1, 3, 15.5, "Médio", "Test", 1);

        _petRepositoryMock.Setup(x => x.GetAsync(petId))
            .ReturnsAsync(pet);

        // Act
        await _service.Handle(petId, command);

        // Assert
        _petRepositoryMock.Verify(x => x.Update(pet), Times.Once());
        _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once());
        pet.Nome.Should().Be(command.Nome);
        pet.Idade.Should().Be(command.Idade);
    }

    [Fact]
    public async Task Handle_PetNotFound_ShouldThrowException()
    {
        // Arrange
        var petId = 999L;
        var command = new AlterarPetCommand();

        _petRepositoryMock.Setup(x => x.GetAsync(petId))
            .ReturnsAsync((Pet?)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.Handle(petId, command));
    }
}