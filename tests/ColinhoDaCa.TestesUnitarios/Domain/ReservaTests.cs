using ColinhoDaCa.Domain.Reservas.Entities;
using ColinhoDaCa.Domain.Reservas.Enums;
using FluentAssertions;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.Domain;

public class ReservaTests
{
    [Fact]
    public void Create_DeveRetornarReservaValida()
    {
        // Arrange
        var clienteId = 1L;
        var dataInicial = DateTime.Today.AddDays(1);
        var dataFinal = DateTime.Today.AddDays(3);
        var quantidadeDiarias = 2;
        var quantidadePets = 1;
        var valorTotal = 100m;
        var valorDesconto = 0m;
        var valorFinal = 100m;
        var obs = "Teste";
        var petIds = new List<long> { 1 };
        var usuarioId = 1L;

        // Act
        var reserva = Reserva.Create(clienteId, dataInicial, dataFinal, quantidadeDiarias, quantidadePets, valorTotal, valorDesconto, valorFinal, null, obs, petIds, usuarioId);

        // Assert
        reserva.ClienteId.Should().Be(clienteId);
        reserva.DataInicial.Should().Be(dataInicial);
        reserva.DataFinal.Should().Be(dataFinal);
        reserva.QuantidadeDiarias.Should().Be(quantidadeDiarias);
        reserva.QuantidadePets.Should().Be(quantidadePets);
        reserva.ValorTotal.Should().Be(valorTotal);
        reserva.ValorDesconto.Should().Be(valorDesconto);
        reserva.ValorFinal.Should().Be(valorFinal);
        reserva.Observacoes.Should().Be(obs);
        reserva.Status.Should().Be(ReservaStatus.ReservaCriada);
    }

    [Fact]
    public void Create_ComObservacoesNulas_DeveLancarException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            Reserva.Create(1, DateTime.Today, DateTime.Today.AddDays(1), 1, 1, 100m, 0m, 100m, null, null!, new List<long>(), 1));
    }

    [Fact]
    public void ConfirmarReserva_DeveAlterarStatusEAdicionarHistorico()
    {
        // Arrange
        var reserva = Reserva.Create(1, DateTime.Today, DateTime.Today.AddDays(1), 1, 1, 100m, 0m, 100m, null, "Teste", new List<long>(), 1);
        var usuarioId = 1L;

        // Act
        reserva.ConfirmarReserva(usuarioId);

        // Assert
        reserva.Status.Should().Be(ReservaStatus.ReservaConfirmada);
        reserva.StatusHistorico.Should().HaveCount(1);
        reserva.StatusHistorico.First().Status.Should().Be(ReservaStatus.ReservaConfirmada);
    }

    [Fact]
    public void AplicarCupom_DeveDefinirCupomEDesconto()
    {
        // Arrange
        var reserva = Reserva.Create(1, DateTime.Today, DateTime.Today.AddDays(1), 1, 1, 100m, 0m, 100m, null, "Teste", new List<long>(), 1);
        var cupomId = 1L;
        var valorDesconto = 20m;

        // Act
        reserva.AplicarCupom(cupomId, valorDesconto);

        // Assert
        reserva.CupomId.Should().Be(cupomId);
        reserva.ValorDesconto.Should().Be(valorDesconto);
        reserva.ValorFinal.Should().Be(80m);
    }

    [Fact]
    public void EnviarComprovantePagamento_DeveDefinirComprovante()
    {
        // Arrange
        var reserva = Reserva.Create(1, DateTime.Today, DateTime.Today.AddDays(1), 1, 1, 100m, 0m, 100m, null, "Teste", new List<long>(), 1);
        var comprovante = "comprovante.jpg";
        var observacoes = "Pago via PIX";

        // Act
        reserva.EnviarComprovantePagamento(comprovante, observacoes);

        // Assert
        reserva.ComprovantePagamento.Should().Be(comprovante);
        reserva.ObservacoesPagamento.Should().Be(observacoes);
        reserva.DataPagamento.Should().NotBeNull();
    }

    [Fact]
    public void CancelarReserva_DeveAlterarStatusParaCancelada()
    {
        // Arrange
        var reserva = Reserva.Create(1, DateTime.Today, DateTime.Today.AddDays(1), 1, 1, 100m, 0m, 100m, null, "Teste", new List<long>(), 1);
        var usuarioId = 1L;

        // Act
        reserva.CancelarReserva(usuarioId);

        // Assert
        reserva.Status.Should().Be(ReservaStatus.ReservaCancelada);
        reserva.StatusHistorico.Should().HaveCount(1);
    }
}