using ColinhoDaCa.Application.Services.Email;
using ColinhoDaCa.Application.UseCases.Sobre.v1.EnviarEmail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.Application.UseCases.Sobre;

public class EnviarEmailServiceTests
{
    private readonly Mock<ILogger<EnviarEmailService>> _loggerMock;
    private readonly Mock<IEmailService> _emailServiceMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly EnviarEmailService _service;

    public EnviarEmailServiceTests()
    {
        _loggerMock = new Mock<ILogger<EnviarEmailService>>();
        _emailServiceMock = new Mock<IEmailService>();
        _configurationMock = new Mock<IConfiguration>();

        _configurationMock.Setup(x => x["Email:EmailDestino"]).Returns("destino@test.com");

        _service = new EnviarEmailService(
            _loggerMock.Object,
            _emailServiceMock.Object,
            _configurationMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldSendEmail()
    {
        var command = new EnviarEmailCommand
        {
            Nome = "Test User",
            Email = "test@test.com",
            Mensagem = "Test message"
        };

        await _service.Handle(command);

        _emailServiceMock.Verify(x => x.EnviarEmailAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>()), Times.Once);
    }
}
