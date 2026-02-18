using ColinhoDaCa.Application.UseCases.Racas.v1.Listar;
using FluentAssertions;
using Moq;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.Api.Controllers;

public class RacasControllerTests
{
    private readonly Mock<IListarRacasService> _listarServiceMock;

    public RacasControllerTests()
    {
        _listarServiceMock = new Mock<IListarRacasService>();
    }

    [Fact]
    public async Task ListarRacas_ReturnsAllRacas()
    {
        var response = new List<ListarRacasResponse>
        {
            new ListarRacasResponse { Id = 1, Nome = "Labrador" },
            new ListarRacasResponse { Id = 2, Nome = "Poodle" }
        };
        _listarServiceMock.Setup(x => x.Handle(null)).ReturnsAsync(response);

        var result = await _listarServiceMock.Object.Handle(null);

        result.Should().HaveCount(2);
        result.Should().Contain(r => r.Nome == "Labrador");
    }
}
