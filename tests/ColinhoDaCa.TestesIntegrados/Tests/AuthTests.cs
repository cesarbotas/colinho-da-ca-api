using ColinhoDaCa.TestesIntegrados.Fixtures;
using ColinhoDaCa.TestesIntegrados.Helpers;
using ColinhoDaCa.TestesIntegrados.Models;
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

        var loginCommand = TestDataBuilder.CreateLoginCommand(registerCommand.Email, registerCommand.Senha);

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/auth/login", loginCommand);
        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result.AccessToken.Should().NotBeNullOrEmpty();
        result.RefreshToken.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_ComCredenciaisInvalidas_DeveRetornar400()
    {
        // Arrange
        var loginCommand = TestDataBuilder.CreateLoginCommand("invalido@test.com", "senhaerrada");

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/auth/login", loginCommand);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Registrar_ComCpfInvalido_DeveRetornar500()
    {
        // Arrange
        var command = TestDataBuilder.RegistrarCommandFaker.Generate();
        command.Cpf = "12345678901"; // CPF inválido

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/auth/registrar", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task Registrar_ComCpfDuplicado_DeveRetornar500()
    {
        // Arrange
        var command1 = TestDataBuilder.RegistrarCommandFaker.Generate();
        var command2 = TestDataBuilder.RegistrarCommandFaker.Generate();
        command2.Cpf = command1.Cpf; // Mesmo CPF
        command2.Email = "outro@email.com"; // Email diferente

        // Act
        await _client.PostAsJsonAsync("/api/v1/auth/registrar", command1);
        var response = await _client.PostAsJsonAsync("/api/v1/auth/registrar", command2);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
}
