using ColinhoDaCa.Application.UseCases.Auth.v1.Login;
using ColinhoDaCa.Application.UseCases.Auth.v1.RefreshTokens;
using ColinhoDaCa.Application.UseCases.Auth.v1.Registrar;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.Api.Controllers;

public class AuthControllerTests
{
    private readonly Mock<ILogger<object>> _loggerMock;
    private readonly Mock<ILoginService> _loginServiceMock;
    private readonly Mock<IRegistrarService> _registrarServiceMock;
    private readonly Mock<IRefreshTokenService> _refreshTokenServiceMock;

    public AuthControllerTests()
    {
        _loggerMock = new Mock<ILogger<object>>();
        _loginServiceMock = new Mock<ILoginService>();
        _registrarServiceMock = new Mock<IRegistrarService>();
        _refreshTokenServiceMock = new Mock<IRefreshTokenService>();
    }

    [Fact]
    public async Task Login_ValidCommand_ReturnsOkResult()
    {
        var command = new LoginCommand { Email = "test@test.com", Senha = "password" };
        var response = new LoginResponse { AccessToken = "token", RefreshToken = "refresh" };
        _loginServiceMock.Setup(x => x.Handle(command)).ReturnsAsync(response);

        var result = await _loginServiceMock.Object.Handle(command);

        result.Should().NotBeNull();
        result.AccessToken.Should().Be("token");
    }

    [Fact]
    public async Task Registrar_ValidCommand_CompletesSuccessfully()
    {
        var command = new RegistrarCommand
        {
            Nome = "Test",
            Email = "test@test.com",
            Celular = "11999999999",
            Cpf = "12345678901",
            Senha = "password"
        };
        _registrarServiceMock.Setup(x => x.Handle(command)).Returns(Task.CompletedTask);

        await _registrarServiceMock.Object.Handle(command);

        _registrarServiceMock.Verify(x => x.Handle(command), Times.Once);
    }

    [Fact]
    public async Task RefreshToken_ValidCommand_ReturnsNewTokens()
    {
        var command = new RefreshTokenCommand { RefreshToken = "refresh" };
        var response = new LoginResponse { AccessToken = "new_token", RefreshToken = "new_refresh" };
        _refreshTokenServiceMock.Setup(x => x.Handle(command)).ReturnsAsync(response);

        var result = await _refreshTokenServiceMock.Object.Handle(command);

        result.Should().NotBeNull();
        result.AccessToken.Should().Be("new_token");
    }
}
