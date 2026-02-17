using ColinhoDaCa.TestesIntegrados.Fixtures;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Xunit;

namespace ColinhoDaCa.TestesIntegrados.Tests;

public class BasicIntegrationTest : IClassFixture<IntegrationTestFactory>
{
    private readonly IntegrationTestFactory _factory;
    private readonly HttpClient _client;

    public BasicIntegrationTest(IntegrationTestFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task RacasEndpoint_ShouldReturn200()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/racas");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeEmpty();
        content.Should().Contain("SRD"); // Verificar se contém dados dos scripts
    }
}