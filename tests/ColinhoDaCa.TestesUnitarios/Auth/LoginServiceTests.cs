using ColinhoDaCa.Application.Services.Auth;
using ColinhoDaCa.Application.UseCases.Auth.v1.Login;
using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain._Shared.Exceptions;
using ColinhoDaCa.Domain.Clientes.Entities;
using ColinhoDaCa.Domain.Clientes.Repositories;
using ColinhoDaCa.Domain.LoginHistorico.Repositories;
using ColinhoDaCa.Domain.RefreshTokens.Repositories;
using ColinhoDaCa.Domain.Usuarios.Entities;
using ColinhoDaCa.Domain.Usuarios.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.Auth;

public class LoginServiceTests
{
    private readonly Mock<ILogger<LoginService>> _loggerMock;
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
    private readonly Mock<IClienteRepository> _clienteRepositoryMock;
    private readonly Mock<IPasswordService> _passwordServiceMock;
    private readonly Mock<IJwtService> _jwtServiceMock;
    private readonly Mock<ILoginHistoricoRepository> _loginHistoricoRepositoryMock;
    private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly LoginService _service;

    public LoginServiceTests()
    {
        _loggerMock = new Mock<ILogger<LoginService>>();
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _clienteRepositoryMock = new Mock<IClienteRepository>();
        _passwordServiceMock = new Mock<IPasswordService>();
        _jwtServiceMock = new Mock<IJwtService>();
        _loginHistoricoRepositoryMock = new Mock<ILoginHistoricoRepository>();
        _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _service = new LoginService(
            _loggerMock.Object,
            _usuarioRepositoryMock.Object,
            _clienteRepositoryMock.Object,
            _passwordServiceMock.Object,
            _jwtServiceMock.Object,
            _loginHistoricoRepositoryMock.Object,
            _refreshTokenRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCredentials_ShouldReturnTokens()
    {
        // Arrange
        var command = new LoginCommand { Email = "test@test.com", Senha = "password123" };
        var cliente = Cliente.Create("Test", "test@test.com", "11999999999", "12345678901", "Test");
        var usuario = Usuario.Create(1, "hashedPassword");

        _clienteRepositoryMock.Setup(x => x.GetByEmailAsync(command.Email))
            .ReturnsAsync(cliente);
        _usuarioRepositoryMock.Setup(x => x.GetPerfisUsuarioAsync(usuario.Id))
            .ReturnsAsync(new List<PerfilUsuarioDto>());
        _usuarioRepositoryMock.Setup(x => x.GetByClienteIdWithPerfisAsync(cliente.Id))
            .ReturnsAsync(usuario);
        _passwordServiceMock.Setup(x => x.VerifyPassword(command.Senha, usuario.SenhaHash))
            .Returns(true);
        _jwtServiceMock.Setup(x => x.GenerateAccessToken(It.IsAny<UsuarioResponse>()))
            .Returns("access_token");
        _jwtServiceMock.Setup(x => x.GenerateRefreshToken())
            .Returns("refresh_token");

        // Act
        var result = await _service.Handle(command);

        // Assert
        result.Should().NotBeNull();
        result.AccessToken.Should().Be("access_token");
        result.RefreshToken.Should().Be("refresh_token");
    }

    [Fact]
    public async Task Handle_InvalidEmail_ShouldThrowValidationException()
    {
        // Arrange
        var command = new LoginCommand { Email = "invalid@test.com", Senha = "password123" };

        _clienteRepositoryMock.Setup(x => x.GetByEmailAsync(command.Email))
            .ReturnsAsync((Cliente)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(() => _service.Handle(command));
        exception.Message.Should().Be("Email ou senha inválidos");
    }
}