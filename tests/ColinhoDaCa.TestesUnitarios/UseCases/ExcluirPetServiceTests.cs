using ColinhoDaCa.Application.UseCases.Pets.v1.ExcluirPet;
using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain.Pets.Entities;
using ColinhoDaCa.Domain.Pets.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.UseCases;

public class ExcluirPetServiceTests
{
    private readonly Mock<ILogger<ExcluirPetService>> _loggerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IPetRepository> _petRepositoryMock;
    private readonly ExcluirPetService _service;

    public ExcluirPetServiceTests()
    {
        _loggerMock = new Mock<ILogger<ExcluirPetService>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _petRepositoryMock = new Mock<IPetRepository>();

        _service = new ExcluirPetService(
            _loggerMock.Object,
            _unitOfWorkMock.Object,
            _petRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_PetWithoutReservations_ShouldDeletePet()
    {
        // Arrange
        var petId = 1L;
        var pet = Pet.Create("Rex", 1, 3, 15.5, "Médio", "Test", 1);

        _petRepositoryMock.Setup(x => x.GetAsync(petId))
            .ReturnsAsync(pet);
        _petRepositoryMock.Setup(x => x.HasReservationsAsync(petId))
            .ReturnsAsync(false);

        // Act
        await _service.Handle(petId);

        // Assert
        _petRepositoryMock.Verify(x => x.Delete(pet), Times.Once());
        _petRepositoryMock.Verify(x => x.Update(It.IsAny<Pet>()), Times.Never());
        _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once());
    }

    [Fact]
    public async Task Handle_PetWithReservations_ShouldInactivatePet()
    {
        // Arrange
        var petId = 1L;
        var pet = Pet.Create("Rex", 1, 3, 15.5, "Médio", "Test", 1);

        _petRepositoryMock.Setup(x => x.GetAsync(petId))
            .ReturnsAsync(pet);
        _petRepositoryMock.Setup(x => x.HasReservationsAsync(petId))
            .ReturnsAsync(true);

        // Act
        await _service.Handle(petId);

        // Assert
        _petRepositoryMock.Verify(x => x.Update(pet), Times.Once());
        _petRepositoryMock.Verify(x => x.Delete(It.IsAny<Pet>()), Times.Never());
        pet.Ativo.Should().BeFalse();
        _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once());
    }

    [Fact]
    public async Task Handle_PetNotFound_ShouldThrowException()
    {
        // Arrange
        var petId = 999L;

        _petRepositoryMock.Setup(x => x.GetAsync(petId))
            .ReturnsAsync((Pet?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.Handle(petId));
        exception.Message.Should().Be("Pet não encontrado");
    }
}