using ColinhoDaCa.Domain.Pets.Entities;
using FluentAssertions;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.Domain;

public class PetTests
{
    [Fact]
    public void Create_ValidData_ShouldCreatePet()
    {
        // Arrange
        var nome = "Rex";
        var racaId = 1L;
        var idade = 3;
        var peso = 15.5;
        var porte = "Médio";
        var observacoes = "Pet dócil";
        var clienteId = 1L;

        // Act
        var pet = Pet.Create(nome, racaId, idade, peso, porte, observacoes, clienteId);

        // Assert
        pet.Should().NotBeNull();
        pet.Nome.Should().Be(nome);
        pet.RacaId.Should().Be(racaId);
        pet.Idade.Should().Be(idade);
        pet.Peso.Should().Be(peso);
        pet.Porte.Should().Be(porte);
        pet.Observacoes.Should().Be(observacoes);
        pet.ClienteId.Should().Be(clienteId);
        pet.Ativo.Should().BeTrue();
    }

    [Fact]
    public void Inativar_ActivePet_ShouldSetAtivoToFalse()
    {
        // Arrange
        var pet = Pet.Create("Rex", 1, 3, 15.5, "Médio", "Test", 1);

        // Act
        pet.Inativar();

        // Assert
        pet.Ativo.Should().BeFalse();
        pet.DataAlteracao.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Alterar_ValidData_ShouldUpdatePet()
    {
        // Arrange
        var pet = Pet.Create("Rex", 1, 3, 15.5, "Médio", "Test", 1);
        var novoNome = "Max";
        var novaIdade = 4;

        // Act
        pet.Alterar(novoNome, 2, novaIdade, 16.0, "Grande", "Pet alterado", 1);

        // Assert
        pet.Nome.Should().Be(novoNome);
        pet.Idade.Should().Be(novaIdade);
        pet.DataAlteracao.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }
}