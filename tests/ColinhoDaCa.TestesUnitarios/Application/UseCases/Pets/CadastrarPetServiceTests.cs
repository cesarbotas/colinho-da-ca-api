using ColinhoDaCa.Application.UseCases.Pets.v1.CadastrarPet;
using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain.Pets.Entities;
using ColinhoDaCa.Domain.Pets.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.Application.UseCases.Pets;

public class CadastrarPetServiceTests
{
    private readonly Mock<ILogger<CadastrarPetService>> _loggerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IPetRepository> _petRepositoryMock;
    private readonly CadastrarPetService _service;

    public CadastrarPetServiceTests()
    {
        _loggerMock = new Mock<ILogger<CadastrarPetService>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _petRepositoryMock = new Mock<IPetRepository>();

        _service = new CadastrarPetService(
            _loggerMock.Object,
            _unitOfWorkMock.Object,
            _petRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreatePet()
    {
        var command = new CadastrarPetCommand
        {
            Nome = "Rex",
            RacaId = 1,
            Idade = 3,
            Peso = 15.5,
            Porte = "Médio",
            Observacoes = "Dócil",
            ClienteId = 1
        };

        await _service.Handle(command);

        _petRepositoryMock.Verify(x => x.InsertAsync(It.IsAny<Pet>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
