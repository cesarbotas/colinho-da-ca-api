using ColinhoDaCa.Application.Services.Email;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.Application.Services.Email;

public class EmailServiceTests
{
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<ILogger<EmailService>> _loggerMock;

    public EmailServiceTests()
    {
        _configurationMock = new Mock<IConfiguration>();
        _loggerMock = new Mock<ILogger<EmailService>>();
        
        _configurationMock.Setup(x => x["Email:SmtpServer"]).Returns("smtp.test.com");
        _configurationMock.Setup(x => x["Email:SmtpPort"]).Returns("587");
        _configurationMock.Setup(x => x["Email:EmailRemetente"]).Returns("test@test.com");
        _configurationMock.Setup(x => x["Email:SenhaEmail"]).Returns("password");
    }

    [Fact]
    public void Constructor_ShouldInitializeWithConfiguration()
    {
        var service = new EmailService(_configurationMock.Object, _loggerMock.Object);
        
        service.Should().NotBeNull();
    }
}
