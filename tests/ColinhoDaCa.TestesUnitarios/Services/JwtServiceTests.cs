using ColinhoDaCa.Application.Services.Auth;
using ColinhoDaCa.Application.UseCases.Auth.v1.Login;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.Services;

public class JwtServiceTests
{
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly JwtService _jwtService;

    public JwtServiceTests()
    {
        _configurationMock = new Mock<IConfiguration>();
        _configurationMock.Setup(x => x["Jwt:Secret"])
            .Returns("super-secret-key-for-jwt-token-generation-minimum-256-bits");

        _jwtService = new JwtService(_configurationMock.Object);
    }

    [Fact]
    public void GenerateAccessToken_ValidUsuario_ShouldReturnToken()
    {
        // Arrange
        var usuario = new UsuarioResponse
        {
            Id = 1,
            ClienteId = 1,
            Nome = "João Silva",
            Email = "joao@test.com",
            Celular = "11999999999",
            Cpf = "12345678901",
            Perfis = new List<PerfilResponse>
            {
                new PerfilResponse { Id = 1, Nome = "Cliente" }
            }
        };

        // Act
        var token = _jwtService.GenerateAccessToken(usuario);

        // Assert
        token.Should().NotBeNullOrEmpty();
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var jsonToken = tokenHandler.ReadJwtToken(token);
        
        jsonToken.Claims.Should().Contain(c => c.Type == ClaimTypes.NameIdentifier && c.Value == "1");
        jsonToken.Claims.Should().Contain(c => c.Type == ClaimTypes.Email && c.Value == "joao@test.com");
    }

    [Fact]
    public void GenerateRefreshToken_ShouldReturnBase64String()
    {
        // Act
        var refreshToken = _jwtService.GenerateRefreshToken();

        // Assert
        refreshToken.Should().NotBeNullOrEmpty();
        refreshToken.Length.Should().BeGreaterThan(0);
        
        // Should be valid base64
        var bytes = Convert.FromBase64String(refreshToken);
        bytes.Length.Should().Be(64);
    }

    [Fact]
    public void GetPrincipalFromExpiredToken_ValidToken_ShouldReturnPrincipal()
    {
        // Arrange
        var usuario = new UsuarioResponse
        {
            Id = 1,
            ClienteId = 1,
            Nome = "João Silva",
            Email = "joao@test.com",
            Celular = "11999999999",
            Cpf = "12345678901",
            Perfis = new List<PerfilResponse>()
        };

        var token = _jwtService.GenerateAccessToken(usuario);

        // Act
        var principal = _jwtService.GetPrincipalFromExpiredToken(token);

        // Assert
        principal.Should().NotBeNull();
        principal.FindFirst(ClaimTypes.NameIdentifier)?.Value.Should().Be("1");
        principal.FindFirst(ClaimTypes.Email)?.Value.Should().Be("joao@test.com");
    }

    [Fact]
    public void GetPrincipalFromExpiredToken_InvalidToken_ShouldReturnNull()
    {
        // Arrange
        var invalidToken = "invalid.token.here";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _jwtService.GetPrincipalFromExpiredToken(invalidToken));
    }
}