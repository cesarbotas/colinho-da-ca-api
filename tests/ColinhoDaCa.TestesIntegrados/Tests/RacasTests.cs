using ColinhoDaCa.TestesIntegrados.Fixtures;
using ColinhoDaCa.TestesIntegrados.Helpers;
using ColinhoDaCa.TestesIntegrados.Models;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace ColinhoDaCa.TestesIntegrados.Tests;

public class RacasTests : IClassFixture<IntegrationTestFactory>
{
    private readonly HttpClient _client;

    public RacasTests(IntegrationTestFactory factory)
    {
        _client = factory.CreateClient();
    }

    private async Task<string> ObterToken()
    {
        var registerCommand = TestDataBuilder.RegistrarCommandFaker.Generate();
        await _client.PostAsJsonAsync("/api/v1/auth/registrar", registerCommand);

        var loginCommand = TestDataBuilder.CreateLoginCommand(registerCommand.Email, registerCommand.Senha);
        var loginResponse = await _client.PostAsJsonAsync("/api/v1/auth/login", loginCommand);
        var result = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
        
        return result.AccessToken;
    }

    [Fact]
    public async Task ListarRacas_DeveRetornarTodasAsRacas()
    {
        // Arrange
        var token = await ObterToken();
        _client.SetBearerToken(token);

        // Act
        var response = await _client.GetAsync("/api/v1/racas");
        var racas = await response.Content.ReadFromJsonAsync<List<RacaDto>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        racas.Should().HaveCountGreaterThan(30);
    }

    [Fact]
    public async Task ListarRacas_DeveConterRacasPequenas()
    {
        // Arrange
        var token = await ObterToken();
        _client.SetBearerToken(token);

        // Act
        var racas = await _client.GetFromJsonAsync<List<RacaDto>>("/api/v1/racas");

        // Assert
        racas.Should().Contain(r => r.Nome == "Shih-tzu" && r.Porte == "P");
        racas.Should().Contain(r => r.Nome == "Pug" && r.Porte == "P");
    }

    [Fact]
    public async Task ListarRacas_DeveConterRacasSRD()
    {
        // Arrange
        var token = await ObterToken();
        _client.SetBearerToken(token);

        // Act
        var racas = await _client.GetFromJsonAsync<List<RacaDto>>("/api/v1/racas");

        // Assert
        racas.Should().Contain(r => r.Nome == "SRD (Sem Raça Definida)" && r.Porte == null);
    }
}

public class RacaDto
{
    public long Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Porte { get; set; }
}
