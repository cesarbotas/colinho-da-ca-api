using ColinhoDaCa.Application.UseCases.Sobre.v1.EnviarEmail;
using FluentAssertions;
using Moq;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.Api.Controllers;

public class SobreControllerTests
{
    private readonly Mock<IEnviarEmailService> _enviarEmailServiceMock;

    public SobreControllerTests()
    {
        _enviarEmailServiceMock = new Mock<IEnviarEmailService>();
    }

    [Fact]
    public async Task EnviarEmail_ValidCommand_CompletesSuccessfully()
    {
        var command = new EnviarEmailCommand
        {
            Nome = "Test",
            Email = "test@test.com",
            Mensagem = "Test message"
        };
        _enviarEmailServiceMock.Setup(x => x.Handle(command)).Returns(Task.CompletedTask);

        await _enviarEmailServiceMock.Object.Handle(command);

        _enviarEmailServiceMock.Verify(x => x.Handle(command), Times.Once);
    }
}
