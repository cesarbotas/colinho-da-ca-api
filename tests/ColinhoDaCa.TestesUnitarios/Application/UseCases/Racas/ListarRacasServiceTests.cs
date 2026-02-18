using ColinhoDaCa.Application.UseCases.Racas.v1.Listar;
using ColinhoDaCa.Domain.Racas.Entities;
using ColinhoDaCa.Domain.Racas.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.Application.UseCases.Racas;

public class ListarRacasServiceTests
{
    private readonly Mock<IRacaRepository> _racaRepositoryMock;
    private readonly ListarRacasService _service;

    public ListarRacasServiceTests()
    {
        _racaRepositoryMock = new Mock<IRacaRepository>();
        _service = new ListarRacasService(_racaRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAllRacas()
    {
        var racas = new List<Raca>
        {
            new Raca { Id = 1, Nome = "Labrador" },
            new Raca { Id = 2, Nome = "Poodle" },
            new Raca { Id = 3, Nome = "Bulldog" }
        };

        _racaRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(racas);

        var result = await _service.Handle(null);

        result.Should().HaveCount(3);
        result.Should().Contain(r => r.Nome == "Labrador");
    }
}
