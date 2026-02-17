using ColinhoDaCa.Domain.Clientes.Entities;
using FluentAssertions;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.Domain;

public class ClienteTests
{
    [Fact]
    public void Create_ValidData_ShouldCreateCliente()
    {
        // Arrange
        var nome = "João Silva";
        var email = "joao@test.com";
        var celular = "11999999999";
        var cpf = "12345678901";
        var observacoes = "Cliente VIP";

        // Act
        var cliente = Cliente.Create(nome, email, celular, cpf, observacoes);

        // Assert
        cliente.Should().NotBeNull();
        cliente.Nome.Should().Be(nome);
        cliente.Email.Should().Be(email);
        cliente.Celular.Should().Be(celular);
        cliente.Cpf.Should().Be(cpf);
        cliente.Observacoes.Should().Be(observacoes);
        cliente.DataInclusao.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Alterar_ValidData_ShouldUpdateCliente()
    {
        // Arrange
        var cliente = Cliente.Create("João", "joao@test.com", "11999999999", "12345678901", "Test");
        var novoNome = "João Silva";
        var novoEmail = "joao.silva@test.com";

        // Act
        cliente.Alterar(novoNome, novoEmail, "11888888888", "98765432100", "Cliente Premium");

        // Assert
        cliente.Nome.Should().Be(novoNome);
        cliente.Email.Should().Be(novoEmail);
        cliente.DataAlteracao.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Create_NullNome_ShouldThrowException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            Cliente.Create(null, "test@test.com", "11999999999", "12345678901", "Test"));
    }
}