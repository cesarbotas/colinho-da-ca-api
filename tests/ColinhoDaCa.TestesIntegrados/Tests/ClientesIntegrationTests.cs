using ColinhoDaCa.TestesIntegrados.Fixtures;
using ColinhoDaCa.TestesIntegrados.Helpers;
using FluentAssertions;
using System.Net;
using System.Text.Json;
using Xunit;

namespace ColinhoDaCa.TestesIntegrados.Tests;

public class ClientesIntegrationTests : IClassFixture<IntegrationTestFactory>
{
    private readonly IntegrationTestFactory _factory;
    private readonly HttpClient _client;

    public ClientesIntegrationTests(IntegrationTestFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task CadastrarCliente_ValidData_ShouldReturn200()
    {
        // Arrange
        var token = await _client.GetAuthTokenAsync();
        var cliente = TestDataBuilder.CreateClienteCommand();

        // Act
        var response = await _client.PostAsJsonWithAuthAsync("/api/v1/clientes", cliente, token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ListarClientes_WithFilters_ShouldReturn200()
    {
        // Arrange
        var token = await _client.GetAuthTokenAsync();

        // Act
        var response = await _client.GetWithAuthAsync("/api/v1/clientes?Paginacao.NumeroPagina=1&Paginacao.QuantidadeRegistros=10", token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeEmpty();
    }

    [Fact]
    public async Task AlterarCliente_ValidData_ShouldReturn200()
    {
        // Arrange
        var token = await _client.GetAuthTokenAsync();
        var clienteId = await _client.CreateTestClienteAsync(token);
        var alterarCommand = TestDataBuilder.CreateAlterarClienteCommand();

        // Act
        var response = await _client.PutAsJsonWithAuthAsync($"/api/v1/clientes/{clienteId}", alterarCommand, token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ExcluirCliente_ValidId_ShouldReturn200()
    {
        // Arrange
        var token = await _client.GetAuthTokenAsync();
        var clienteId = await _client.CreateTestClienteAsync(token);

        // Act
        var response = await _client.DeleteWithAuthAsync($"/api/v1/clientes/{clienteId}", token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}