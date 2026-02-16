using ColinhoDaCa.TestesIntegrados.Fixtures;
using ColinhoDaCa.TestesIntegrados.Helpers;
using FluentAssertions;
using System.Net;
using Xunit;

namespace ColinhoDaCa.TestesIntegrados.Tests;

public class PetsIntegrationTests : IClassFixture<IntegrationTestFactory>
{
    private readonly IntegrationTestFactory _factory;
    private readonly HttpClient _client;

    public PetsIntegrationTests(IntegrationTestFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task CadastrarPet_ValidData_ShouldReturn200()
    {
        // Arrange
        var token = await _client.GetAuthTokenAsync();
        var clienteId = await _client.CreateTestClienteAsync(token);
        var pet = TestDataBuilder.CreatePetCommand(clienteId);

        // Act
        var response = await _client.PostAsJsonWithAuthAsync("/api/v1/pets", pet, token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ListarPets_WithFilters_ShouldReturn200()
    {
        // Arrange
        var token = await _client.GetAuthTokenAsync();

        // Act
        var response = await _client.GetWithAuthAsync("/api/v1/pets?Paginacao.NumeroPagina=1&Paginacao.QuantidadeRegistros=10", token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ExcluirPet_WithoutReservations_ShouldReturn200()
    {
        // Arrange
        var token = await _client.GetAuthTokenAsync();
        var clienteId = await _client.CreateTestClienteAsync(token);
        var petId = await _client.CreateTestPetAsync(token, clienteId);

        // Act
        var response = await _client.DeleteWithAuthAsync($"/api/v1/pets/{petId}", token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}