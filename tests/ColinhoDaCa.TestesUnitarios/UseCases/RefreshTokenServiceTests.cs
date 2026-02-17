using ColinhoDaCa.Application.Services.Auth;
using ColinhoDaCa.Application.UseCases.Auth.v1.RefreshToken;
using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain._Shared.Exceptions;
using ColinhoDaCa.Domain.RefreshTokens.Entities;
using ColinhoDaCa.Domain.RefreshTokens.Repositories;
using ColinhoDaCa.Domain.Usuarios.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.UseCases;

public class RefreshTokenServiceTests
{
    private readonly Mock<ILogger<RefreshTokenService>> _loggerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
    private readonly Mock<IJwtService> _jwtServiceMock;
    private readonly RefreshTokenService _service;

    public RefreshTokenServiceTests()
    {
        _loggerMock = new Mock<ILogger<RefreshTokenService>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _jwtServiceMock = new Mock<IJwtService>();

        _service = new RefreshTokenService(
            _loggerMock.Object,
            _unitOfWorkMock.Object,
            _refreshTokenRepositoryMock.Object,
            _usuarioRepositoryMock.Object,
            _jwtServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRefreshToken_ShouldReturnNewTokens()
    {
        // Arrange
        var command = new RefreshTokenCommand
        {
            AccessToken = "expired_access_token",
            RefreshToken = "valid_refresh_token"
        };

        var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1")
        }));

        var refreshToken = RefreshTokenDb.Create(1, "valid_refresh_token", DateTime.UtcNow.AddDays(7));

        _jwtServiceMock.Setup(x => x.GetPrincipalFromExpiredToken(command.AccessToken))
            .Returns(principal);
        _refreshTokenRepositoryMock.Setup(x => x.GetByTokenAsync(command.RefreshToken))
            .ReturnsAsync(refreshToken);
        _jwtServiceMock.Setup(x => x.GenerateAccessToken(It.IsAny<UsuarioResponse>()))
            .Returns("new_access_token");
        _jwtServiceMock.Setup(x => x.GenerateRefreshToken())
            .Returns("new_refresh_token");

        // Act
        var result = await _service.Handle(command);

        // Assert
        result.Should().NotBeNull();
        result.AccessToken.Should().Be("new_access_token");
        result.RefreshToken.Should().Be("new_refresh_token");
        _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once());
    }

    [Fact]
    public async Task Handle_InvalidRefreshToken_ShouldThrowValidationException()
    {
        // Arrange
        var command = new RefreshTokenCommand
        {
            AccessToken = "access_token",
            RefreshToken = "invalid_refresh_token"
        };

        var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1")
        }));

        _jwtServiceMock.Setup(x => x.GetPrincipalFromExpiredToken(command.AccessToken))
            .Returns(principal);
        _refreshTokenRepositoryMock.Setup(x => x.GetByTokenAsync(command.RefreshToken))
            .ReturnsAsync((RefreshTokenDb?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(() => _service.Handle(command));
        exception.Message.Should().Be("Token de refresh inválido");
    }
}