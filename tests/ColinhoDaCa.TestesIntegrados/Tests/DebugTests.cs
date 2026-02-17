using ColinhoDaCa.TestesIntegrados.Fixtures;
using ColinhoDaCa.TestesIntegrados.Helpers;
using FluentAssertions;
using System.Net;
using System.Text.Json;
using Xunit;

namespace ColinhoDaCa.TestesIntegrados.Tests;

public class DebugTests : IClassFixture<IntegrationTestFactory>
{
    private readonly HttpClient _client;

    public DebugTests(IntegrationTestFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Debug_CriarClienteEPet_DeveRetornar200()
    {
        // Arrange - Obter token
        var token = await _client.GetAuthTokenAsync();

        // Act 1 - Criar cliente
        var clienteCommand = TestDataBuilder.CreateClienteCommand();
        var clienteResponse = await _client.PostAsJsonWithAuthAsync("/api/v1/clientes", clienteCommand, token);
        
        // Assert 1
        clienteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // Act 2 - Listar clientes para pegar o ID real
        await Task.Delay(500); // Aguarda persistir
        var listResponse = await _client.GetWithAuthAsync("/api/v1/clientes?Paginacao.NumeroPagina=1&Paginacao.QuantidadeRegistros=10", token);
        listResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var listContent = await listResponse.Content.ReadAsStringAsync();
        var listResult = JsonSerializer.Deserialize<JsonElement>(listContent);
        
        // Pegar o primeiro cliente da lista
        long clienteId = 1; // Fallback
        if (listResult.TryGetProperty("dados", out var dados) && dados.ValueKind == JsonValueKind.Array && dados.GetArrayLength() > 0)
        {
            var primeiroCliente = dados[0];
            if (primeiroCliente.TryGetProperty("id", out var idProp))
            {
                clienteId = idProp.GetInt64();
            }
        }

        // Act 3 - Criar pet com ID real do cliente
        var petCommand = TestDataBuilder.CreatePetCommand(clienteId);
        var petResponse = await _client.PostAsJsonWithAuthAsync("/api/v1/pets", petCommand, token);

        // Assert 3
        petResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}