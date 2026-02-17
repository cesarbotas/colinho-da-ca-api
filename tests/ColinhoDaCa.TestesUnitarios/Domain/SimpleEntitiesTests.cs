using ColinhoDaCa.Domain.Perfis.Entities;
using ColinhoDaCa.Domain.Usuarios.Entities;
using ColinhoDaCa.Domain.Racas.Entities;
using FluentAssertions;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.Domain;

public class SimpleEntitiesTests
{
    [Fact]
    public void Perfil_DeveDefinirPropriedadesCorretamente()
    {
        // Arrange
        var perfil = new Perfil();
        var id = 1L;
        var nome = "Admin";
        var descricao = "Administrador do sistema";

        // Act
        perfil.Id = id;
        perfil.Nome = nome;
        perfil.Descricao = descricao;

        // Assert
        perfil.Id.Should().Be(id);
        perfil.Nome.Should().Be(nome);
        perfil.Descricao.Should().Be(descricao);
    }

    [Fact]
    public void UsuarioPerfil_Create_DeveRetornarUsuarioPerfilValido()
    {
        // Arrange
        var usuarioId = 1L;
        var perfilId = 2L;

        // Act
        var usuarioPerfil = UsuarioPerfil.Create(usuarioId, perfilId);

        // Assert
        usuarioPerfil.UsuarioId.Should().Be(usuarioId);
        usuarioPerfil.PerfilId.Should().Be(perfilId);
    }

    [Fact]
    public void RacaDb_DeveDefinirPropriedadesCorretamente()
    {
        // Arrange
        var raca = new RacaDb();
        var id = 1L;
        var nome = "Golden Retriever";
        var porte = "Grande";

        // Act
        raca.Id = id;
        raca.Nome = nome;
        raca.Porte = porte;

        // Assert
        raca.Id.Should().Be(id);
        raca.Nome.Should().Be(nome);
        raca.Porte.Should().Be(porte);
    }
}