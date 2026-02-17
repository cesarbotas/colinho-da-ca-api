using ColinhoDaCa.Domain.Cupons.Entities;
using ColinhoDaCa.Domain.Cupons.Enums;
using FluentAssertions;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.Domain;

public class CupomDbTests
{
    [Fact]
    public void Create_DeveRetornarCupomValido()
    {
        // Arrange
        var codigo = "DESC10";
        var descricao = "10% de desconto";
        var tipo = TipoCupom.PercentualSobreTotal;
        var percentual = 10m;

        // Act
        var cupom = CupomDb.Create(codigo, descricao, tipo, percentual, null, null, null, null, null, null);

        // Assert
        cupom.Codigo.Should().Be(codigo);
        cupom.Descricao.Should().Be(descricao);
        cupom.Tipo.Should().Be(tipo);
        cupom.Percentual.Should().Be(percentual);
        cupom.Ativo.Should().BeTrue();
    }

    [Fact]
    public void CalcularDesconto_PercentualSobreTotal_DeveRetornarDescontoCorreto()
    {
        // Arrange
        var cupom = CupomDb.Create("DESC10", "10% desconto", TipoCupom.PercentualSobreTotal, 10m, null, null, null, null, null, null);
        var valorTotal = 100m;

        // Act
        var desconto = cupom.CalcularDesconto(valorTotal, 1, 1);

        // Assert
        desconto.Should().Be(10m);
    }

    [Fact]
    public void CalcularDesconto_CupomInativo_DeveRetornarZero()
    {
        // Arrange
        var cupom = CupomDb.Create("DESC10", "10% desconto", TipoCupom.PercentualSobreTotal, 10m, null, null, null, null, null, null);
        cupom.Inativar();

        // Act
        var desconto = cupom.CalcularDesconto(100m, 1, 1);

        // Assert
        desconto.Should().Be(0m);
    }

    [Fact]
    public void CalcularDesconto_ValorFixoComMinimo_ComValorSuficiente_DeveRetornarValorFixo()
    {
        // Arrange
        var cupom = CupomDb.Create("FIXO20", "R$ 20 desconto", TipoCupom.ValorFixoComMinimo, 0m, 20m, 50m, null, null, null, null);

        // Act
        var desconto = cupom.CalcularDesconto(100m, 1, 1);

        // Assert
        desconto.Should().Be(20m);
    }

    [Fact]
    public void CalcularDesconto_ValorFixoComMinimo_ComValorInsuficiente_DeveRetornarZero()
    {
        // Arrange
        var cupom = CupomDb.Create("FIXO20", "R$ 20 desconto", TipoCupom.ValorFixoComMinimo, 0m, 20m, 100m, null, null, null, null);

        // Act
        var desconto = cupom.CalcularDesconto(50m, 1, 1);

        // Assert
        desconto.Should().Be(0m);
    }

    [Fact]
    public void CalcularDesconto_PercentualPorPetComMinimo_ComPetsSuficientes_DeveRetornarDesconto()
    {
        // Arrange
        var cupom = CupomDb.Create("PET10", "10% por pet", TipoCupom.PercentualPorPetComMinimo, 10m, null, null, 2, null, null, null);

        // Act
        var desconto = cupom.CalcularDesconto(100m, 2, 1);

        // Assert
        desconto.Should().Be(10m); // (100/2) * (10/100) * 2 = 50 * 0.1 * 2 = 10
    }

    [Fact]
    public void Inativar_DeveDefinirAtivoComoFalse()
    {
        // Arrange
        var cupom = CupomDb.Create("DESC10", "10% desconto", TipoCupom.PercentualSobreTotal, 10m, null, null, null, null, null, null);

        // Act
        cupom.Inativar();

        // Assert
        cupom.Ativo.Should().BeFalse();
    }

    [Fact]
    public void Alterar_DeveAtualizarPropriedades()
    {
        // Arrange
        var cupom = CupomDb.Create("DESC10", "10% desconto", TipoCupom.PercentualSobreTotal, 10m, null, null, null, null, null, null);
        var novoPercentual = 15m;

        // Act
        cupom.Alterar("DESC15", "15% desconto", TipoCupom.PercentualSobreTotal, novoPercentual, null, null, null, null, null, null);

        // Assert
        cupom.Codigo.Should().Be("DESC15");
        cupom.Descricao.Should().Be("15% desconto");
        cupom.Percentual.Should().Be(novoPercentual);
    }
}