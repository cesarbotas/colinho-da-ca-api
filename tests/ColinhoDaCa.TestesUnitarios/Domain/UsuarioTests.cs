using ColinhoDaCa.Domain.Usuarios.Entities;
using FluentAssertions;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.Domain;

public class UsuarioTests
{
    [Fact]
    public void Create_ValidData_ShouldCreateUsuario()
    {
        // Arrange
        var clienteId = 1L;
        var senhaHash = "hashedPassword123";

        // Act
        var usuario = Usuario.Create(clienteId, senhaHash);

        // Assert
        usuario.Should().NotBeNull();
        usuario.ClienteId.Should().Be(clienteId);
        usuario.SenhaHash.Should().Be(senhaHash);
        usuario.Ativo.Should().BeTrue();
        usuario.DataInclusao.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void AlterarSenha_ValidHash_ShouldUpdatePassword()
    {
        // Arrange
        var usuario = Usuario.Create(1, "oldHash");
        var newHash = "newHashedPassword";

        // Act
        usuario.AlterarSenha(newHash);

        // Assert
        usuario.SenhaHash.Should().Be(newHash);
        usuario.DataAlteracao.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Desativar_ActiveUser_ShouldSetAtivoToFalse()
    {
        // Arrange
        var usuario = Usuario.Create(1, "hash");

        // Act
        usuario.Desativar();

        // Assert
        usuario.Ativo.Should().BeFalse();
        usuario.DataAlteracao.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Ativar_InactiveUser_ShouldSetAtivoToTrue()
    {
        // Arrange
        var usuario = Usuario.Create(1, "hash");
        usuario.Desativar();

        // Act
        usuario.Ativar();

        // Assert
        usuario.Ativo.Should().BeTrue();
        usuario.DataAlteracao.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Create_NullSenhaHash_ShouldThrowException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Usuario.Create(1, null));
    }
}