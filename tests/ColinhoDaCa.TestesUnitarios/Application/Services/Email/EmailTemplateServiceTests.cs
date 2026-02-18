using ColinhoDaCa.Application.Services.EmailTemplates;
using FluentAssertions;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.Application.Services.Email;

public class EmailTemplateServiceTests
{
    [Fact]
    public void GerarEmailNovaReserva_ShouldReturnHtmlContent()
    {
        var pets = new List<(string Nome, string Raca)>
        {
            ("Rex", "Labrador"),
            ("Max", "Poodle")
        };

        var result = EmailTemplateService.GerarEmailNovaReserva(
            "João Silva",
            123,
            DateTime.Now,
            DateTime.Now.AddDays(5),
            5,
            2,
            500m,
            0m,
            500m,
            "Observações teste",
            pets
        );

        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("João Silva");
        result.Should().Contain("Rex");
        result.Should().Contain("Max");
    }
}
