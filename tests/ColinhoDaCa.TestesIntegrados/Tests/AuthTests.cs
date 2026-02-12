using ColinhoDaCa.TestesIntegrados.Fixtures;
using ColinhoDaCa.TestesIntegrados.Helpers;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace ColinhoDaCa.TestesIntegrados.Tests;

public class AuthTests : IClassFixture<IntegrationTestFactory>
{
    private readonly HttpClient _client;

    public AuthTests(IntegrationTestFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Registrar_ComDadosValidos_DeveRetornar200()
    {
        // Arrange
        var command = TestDataBuilder.RegistrarCommandFaker.Generate();

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/auth/registrar", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Login_ComCredenciaisValidas_DeveRetornarToken()
    {
        // Arrange
        var registerCommand = TestDataBuilder.RegistrarCommandFaker.Generate();
        await _client.PostAsJsonAsync("/api/v1/auth/registrar", registerCommand);

        var loginCommand = new { email = registerCommand.Email, senha = registerCommand.Senha };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/auth/login", loginCommand);
        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_ComCredenciaisInvalidas_DeveRetornar400()
    {
        // Arrange
        var loginCommand = new { email = "invalido@test.com", senha = "senhaerrada" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/auth/login", loginCommand);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}

public class LoginResponse
{
    public string Token { get; set; }
}
