using ColinhoDaCa.Application.Services.Auth;
using ColinhoDaCa.Application.UseCases.Auth.v1.Login;
using ColinhoDaCa.Application.UseCases.Auth.v1.RefreshTokens;
using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain._Shared.Exceptions;
using ColinhoDaCa.Domain.Clientes.Entities;
using ColinhoDaCa.Domain.Clientes.Repositories;
using ColinhoDaCa.Domain.RefreshTokens.Entities;
using ColinhoDaCa.Domain.RefreshTokens.Repositories;
using ColinhoDaCa.Domain.Usuarios.Entities;
using ColinhoDaCa.Domain.Usuarios.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.Application.UseCases.Auth;

public class RefreshTokenServiceTests
{
    private readonly Mock<ILogger<RefreshTokenService>> _loggerMock;
    private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
    private readonly Mock<IClienteRepository> _clienteRepositoryMock;
    private readonly Mock<IJwtService> _jwtServiceMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly RefreshTokenService _service;

    public RefreshTokenServiceTests()
    {
        _loggerMock = new Mock<ILogger<RefreshTokenService>>();
        _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _clienteRepositoryMock = new Mock<IClienteRepository>();
        _jwtServiceMock = new Mock<IJwtService>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _service = new RefreshTokenService(_loggerMock.Object, _refreshTokenRepositoryMock.Object, _usuarioRepositoryMock.Object, _clienteRepositoryMock.Object, _jwtServiceMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_TokenValido_DeveRetornarNovosTokens()
    {
        var refreshToken = new RefreshToken { UsuarioId = 1, Token = "token", ExpiresAt = DateTime.UtcNow.AddDays(7) };
        var usuario = Usuario.Create(1, "senhaHash");
        var cliente = Cliente.Create("João", "joao@email.com", "11999999999", "12345678900", "Obs");
        var perfis = new List<PerfilUsuarioDto> { new PerfilUsuarioDto { Id = 1, Nome = "Admin" } };
        var command = new RefreshTokenCommand { RefreshToken = "token" };

        _refreshTokenRepositoryMock.Setup(x => x.GetByTokenAsync("token")).ReturnsAsync(refreshToken);
        _usuarioRepositoryMock.Setup(x => x.GetAsync(It.IsAny<long>())).ReturnsAsync(usuario);
        _clienteRepositoryMock.Setup(x => x.GetAsync(It.IsAny<long>())).ReturnsAsync(cliente);
        _usuarioRepositoryMock.Setup(x => x.GetPerfisUsuarioAsync(It.IsAny<long>())).ReturnsAsync(perfis);
        _jwtServiceMock.Setup(x => x.GenerateAccessToken(It.IsAny<UsuarioResponse>())).Returns("newAccessToken");
        _jwtServiceMock.Setup(x => x.GenerateRefreshToken()).Returns("newRefreshToken");

        var result = await _service.Handle(command);

        Assert.NotNull(result);
        Assert.Equal("newAccessToken", result.AccessToken);
        Assert.Equal("newRefreshToken", result.RefreshToken);
    }

    [Fact]
    public async Task Handle_TokenInvalido_DeveLancarExcecao()
    {
        var command = new RefreshTokenCommand { RefreshToken = "invalid" };
        _refreshTokenRepositoryMock.Setup(x => x.GetByTokenAsync("invalid")).ReturnsAsync((RefreshToken?)null);

        await Assert.ThrowsAsync<ValidationException>(() => _service.Handle(command));
    }
}
