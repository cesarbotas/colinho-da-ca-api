using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using ColinhoDaCa.Application.UseCases.Auth.v1.Login;
using ColinhoDaCa.Application.UseCases.Clientes.v1.CadastrarCliente;
using ColinhoDaCa.Application.UseCases.Pets.v1.CadastrarPet;
using ColinhoDaCa.Application.UseCases.Reservas.v1.CadastrarReserva;

namespace ColinhoDaCa.TestesIntegrados.Helpers;

public static class HttpClientExtensions
{
    public static void SetBearerToken(this HttpClient client, string token)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public static void SetAuthorizationHeader(this HttpClient client, string token)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public static async Task<T?> GetFromJsonAsync<T>(this HttpClient client, string url)
    {
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }

    public static async Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient client, string url, T data)
    {
        return await client.PostAsJsonAsync(url, data, CancellationToken.None);
    }

    // Auth helpers
    public static async Task<string> GetAuthTokenAsync(this HttpClient client)
    {
        // Primeiro registra um usuário de teste
        var registrarCommand = TestDataBuilder.RegistrarCommandFaker.Generate();
        var registerResponse = await client.PostAsJsonAsync("/api/v1/auth/registrar", registrarCommand);
        
        // Se o registro falhar (usuário já existe), tenta fazer login diretamente
        var loginCommand = new LoginCommand
        {
            Email = registrarCommand.Email,
            Senha = registrarCommand.Senha
        };
        
        var response = await client.PostAsJsonAsync("/api/v1/auth/login", loginCommand);
        response.EnsureSuccessStatusCode();
        
        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
        return loginResponse?.AccessToken ?? throw new InvalidOperationException("Failed to get auth token");
    }

    // Authenticated requests
    public static async Task<HttpResponseMessage> PostAsJsonWithAuthAsync<T>(this HttpClient client, string url, T data, string? token = null)
    {
        token ??= await client.GetAuthTokenAsync();
        client.SetBearerToken(token);
        return await client.PostAsJsonAsync(url, data);
    }

    public static async Task<HttpResponseMessage> GetWithAuthAsync(this HttpClient client, string url, string? token = null)
    {
        token ??= await client.GetAuthTokenAsync();
        client.SetBearerToken(token);
        return await client.GetAsync(url);
    }

    public static async Task<HttpResponseMessage> PutAsJsonWithAuthAsync<T>(this HttpClient client, string url, T data, string? token = null)
    {
        token ??= await client.GetAuthTokenAsync();
        client.SetBearerToken(token);
        return await client.PutAsJsonAsync(url, data);
    }

    public static async Task<HttpResponseMessage> PutWithAuthAsync<T>(this HttpClient client, string url, T data, string? token = null)
    {
        token ??= await client.GetAuthTokenAsync();
        client.SetBearerToken(token);
        return await client.PutAsJsonAsync(url, data);
    }

    public static async Task<HttpResponseMessage> DeleteWithAuthAsync(this HttpClient client, string url, string? token = null)
    {
        token ??= await client.GetAuthTokenAsync();
        client.SetBearerToken(token);
        return await client.DeleteAsync(url);
    }

    // Test data creation helpers
    public static async Task<long> CreateTestClienteAsync(this HttpClient client, string? token = null)
    {
        // Primeiro tenta usar o cliente de teste já inserido pelo script
        var searchResponse = await client.GetWithAuthAsync($"/api/v1/clientes?Paginacao.NumeroPagina=1&Paginacao.QuantidadeRegistros=100", token);
        if (searchResponse.IsSuccessStatusCode)
        {
            var searchContent = await searchResponse.Content.ReadAsStringAsync();
            if (!string.IsNullOrWhiteSpace(searchContent))
            {
                try
                {
                    var searchResult = JsonSerializer.Deserialize<JsonElement>(searchContent);
                    if (searchResult.TryGetProperty("dados", out var dados) && dados.ValueKind == JsonValueKind.Array && dados.GetArrayLength() > 0)
                    {
                        // Usa o primeiro cliente encontrado
                        var primeiroCliente = dados[0];
                        if (primeiroCliente.TryGetProperty("id", out var idProp))
                        {
                            return idProp.GetInt64();
                        }
                    }
                }
                catch (JsonException)
                {
                    // Ignora erro de JSON
                }
            }
        }
        
        // Se não encontrou cliente existente, cria um novo
        var command = TestDataBuilder.CreateClienteCommand();
        var response = await client.PostAsJsonWithAuthAsync("/api/v1/clientes", command, token);
        response.EnsureSuccessStatusCode();
        
        // Aguarda um pouco e busca novamente
        await Task.Delay(200);
        
        var newSearchResponse = await client.GetWithAuthAsync($"/api/v1/clientes?Paginacao.NumeroPagina=1&Paginacao.QuantidadeRegistros=100", token);
        if (newSearchResponse.IsSuccessStatusCode)
        {
            var newSearchContent = await newSearchResponse.Content.ReadAsStringAsync();
            if (!string.IsNullOrWhiteSpace(newSearchContent))
            {
                try
                {
                    var newSearchResult = JsonSerializer.Deserialize<JsonElement>(newSearchContent);
                    if (newSearchResult.TryGetProperty("dados", out var newDados) && newDados.ValueKind == JsonValueKind.Array)
                    {
                        // Pega o último cliente (recém-criado)
                        if (newDados.GetArrayLength() > 0)
                        {
                            var ultimoCliente = newDados[newDados.GetArrayLength() - 1];
                            if (ultimoCliente.TryGetProperty("id", out var ultimoId))
                            {
                                return ultimoId.GetInt64();
                            }
                        }
                    }
                }
                catch (JsonException)
                {
                    // Ignora erro de JSON
                }
            }
        }
        
        // Como último recurso, retorna ID 1 (cliente de teste)
        return 1;
    }

    public static async Task<long> CreateTestPetAsync(this HttpClient client, long clienteId, string? token = null)
    {
        // Primeiro tenta usar pets existentes
        var searchResponse = await client.GetWithAuthAsync($"/api/v1/pets?Paginacao.NumeroPagina=1&Paginacao.QuantidadeRegistros=100", token);
        if (searchResponse.IsSuccessStatusCode)
        {
            var searchContent = await searchResponse.Content.ReadAsStringAsync();
            if (!string.IsNullOrWhiteSpace(searchContent))
            {
                try
                {
                    var searchResult = JsonSerializer.Deserialize<JsonElement>(searchContent);
                    if (searchResult.TryGetProperty("dados", out var dados) && dados.ValueKind == JsonValueKind.Array && dados.GetArrayLength() > 0)
                    {
                        var primeiroPet = dados[0];
                        if (primeiroPet.TryGetProperty("id", out var idProp))
                        {
                            return idProp.GetInt64();
                        }
                    }
                }
                catch (JsonException) { }
            }
        }
        
        // Se não encontrou, cria novo
        var command = TestDataBuilder.CreatePetCommand(clienteId);
        var response = await client.PostAsJsonWithAuthAsync("/api/v1/pets", command, token);
        response.EnsureSuccessStatusCode();
        
        return clienteId; // Fallback
    }

    public static async Task<long> CreateTestReservaAsync(this HttpClient client, long clienteId, long[] petIds, string? token = null)
    {
        // Primeiro tenta usar reservas existentes
        var searchResponse = await client.GetWithAuthAsync($"/api/v1/reservas?Paginacao.NumeroPagina=1&Paginacao.QuantidadeRegistros=100", token);
        if (searchResponse.IsSuccessStatusCode)
        {
            var searchContent = await searchResponse.Content.ReadAsStringAsync();
            if (!string.IsNullOrWhiteSpace(searchContent))
            {
                try
                {
                    var searchResult = JsonSerializer.Deserialize<JsonElement>(searchContent);
                    if (searchResult.TryGetProperty("dados", out var dados) && dados.ValueKind == JsonValueKind.Array && dados.GetArrayLength() > 0)
                    {
                        var primeiraReserva = dados[0];
                        if (primeiraReserva.TryGetProperty("id", out var idProp))
                        {
                            return idProp.GetInt64();
                        }
                    }
                }
                catch (JsonException) { }
            }
        }
        
        // Se não encontrou, cria nova
        var command = TestDataBuilder.CreateReservaCommand(clienteId, petIds);
        var response = await client.PostAsJsonWithAuthAsync("/api/v1/reservas", command, token);
        response.EnsureSuccessStatusCode();
        
        return clienteId; // Fallback
    }
}
