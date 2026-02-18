using ColinhoDaCa.Domain.Clientes.Entities;
using ColinhoDaCa.Infra.Data.Context.Repositories.Clientes;
using FluentAssertions;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.Infra.Data.Repositories;

public class ClienteRepositoryTests : RepositoryTestBase
{
    private readonly ClienteRepository _repository;

    public ClienteRepositoryTests()
    {
        _repository = new ClienteRepository(Context);
    }

    [Fact]
    public async Task InsertAsync_ShouldAddCliente()
    {
        var cliente = Cliente.Create("Test", "test@test.com", "11999999999", "12345678901", "Obs");

        await _repository.InsertAsync(cliente);
        await Context.SaveChangesAsync();

        var result = await _repository.GetAsync(c => c.Email == "test@test.com");
        result.Should().NotBeNull();
        result!.Nome.Should().Be("Test");
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnCliente()
    {
        var cliente = Cliente.Create("Test", "test@test.com", "11999999999", "12345678901", "Obs");
        await _repository.InsertAsync(cliente);
        await Context.SaveChangesAsync();

        var result = await _repository.GetByEmailAsync("test@test.com");

        result.Should().NotBeNull();
        result!.Email.Should().Be("test@test.com");
    }

    [Fact]
    public async Task GetByCpfAsync_ShouldReturnCliente()
    {
        var cliente = Cliente.Create("Test", "test@test.com", "11999999999", "12345678901", "Obs");
        await _repository.InsertAsync(cliente);
        await Context.SaveChangesAsync();

        var result = await _repository.GetByCpfAsync("12345678901");

        result.Should().NotBeNull();
        result!.Cpf.Should().Be("12345678901");
    }

    [Fact]
    public async Task Update_ShouldModifyCliente()
    {
        var cliente = Cliente.Create("Test", "test@test.com", "11999999999", "12345678901", "Obs");
        await _repository.InsertAsync(cliente);
        await Context.SaveChangesAsync();

        cliente.Alterar("Updated", "updated@test.com", "11988888888", "12345678901", "New Obs");
        _repository.Update(cliente);
        await Context.SaveChangesAsync();

        var result = await _repository.GetAsync(c => c.Id == cliente.Id);
        result!.Nome.Should().Be("Updated");
    }

    [Fact]
    public async Task Delete_ShouldRemoveCliente()
    {
        var cliente = Cliente.Create("Test", "test@test.com", "11999999999", "12345678901", "Obs");
        await _repository.InsertAsync(cliente);
        await Context.SaveChangesAsync();

        _repository.Delete(cliente);
        await Context.SaveChangesAsync();

        var allClientes = await _repository.GetAllAsync();
        allClientes.Should().BeEmpty();
    }
}
