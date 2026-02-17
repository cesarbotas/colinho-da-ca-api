using ColinhoDaCa.TestesIntegrados.Fixtures;
using ColinhoDaCa.TestesIntegrados.Helpers;
using FluentAssertions;
using System.Net;
using Xunit;

namespace ColinhoDaCa.TestesIntegrados.Tests;

public class ReservasIntegrationTests : IClassFixture<IntegrationTestFactory>
{
    private readonly IntegrationTestFactory _factory;
    private readonly HttpClient _client;

    public ReservasIntegrationTests(IntegrationTestFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task CadastrarReserva_ValidData_ShouldReturn200()
    {
        // Arrange
        var token = await _client.GetAuthTokenAsync();
        var clienteId = await _client.CreateTestClienteAsync(token);
        var petId = await _client.CreateTestPetAsync(clienteId, token);
        var reserva = TestDataBuilder.CreateReservaCommand(clienteId, new[] { petId });

        // Act
        var response = await _client.PostAsJsonWithAuthAsync("/api/v1/reservas", reserva, token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ListarReservas_WithFilters_ShouldReturn200()
    {
        // Arrange
        var token = await _client.GetAuthTokenAsync();

        // Act
        var response = await _client.GetWithAuthAsync("/api/v1/reservas?Paginacao.NumeroPagina=1&Paginacao.QuantidadeRegistros=10", token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ConfirmarReserva_ValidId_ShouldReturn405()
    {
        // Arrange
        var token = await _client.GetAuthTokenAsync();
        var clienteId = await _client.CreateTestClienteAsync(token);
        var petId = await _client.CreateTestPetAsync(clienteId, token);
        var reservaId = await _client.CreateTestReservaAsync(clienteId, new[] { petId }, token);

        // Act
        var request = new HttpRequestMessage(HttpMethod.Patch, $"/api/v1/reservas/{reservaId}/confirmar");
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _client.SendAsync(request);

        // Assert - Endpoint não implementado, espera 405
        response.StatusCode.Should().Be(HttpStatusCode.MethodNotAllowed);
    }

    [Fact]
    public async Task CancelarReserva_ValidId_ShouldReturn405()
    {
        // Arrange
        var token = await _client.GetAuthTokenAsync();
        var clienteId = await _client.CreateTestClienteAsync(token);
        var petId = await _client.CreateTestPetAsync(clienteId, token);
        var reservaId = await _client.CreateTestReservaAsync(clienteId, new[] { petId }, token);

        // Act
        var request = new HttpRequestMessage(HttpMethod.Patch, $"/api/v1/reservas/{reservaId}/cancelar");
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _client.SendAsync(request);

        // Assert - Endpoint não implementado, espera 405
        response.StatusCode.Should().Be(HttpStatusCode.MethodNotAllowed);
    }
}