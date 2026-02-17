using ColinhoDaCa.Domain.Reservas.Entities;
using ColinhoDaCa.Domain.Reservas.Enums;
using FluentAssertions;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.Domain;

public class ReservaDbTests
{
    [Fact]
    public void Create_ValidData_ShouldCreateReserva()
    {
        // Arrange
        var clienteId = 1L;
        var dataInicial = DateTime.Today.AddDays(1);
        var dataFinal = DateTime.Today.AddDays(3);
        var quantidadeDiarias = 2;
        var quantidadePets = 1;
        var valorTotal = 200m;
        var valorDesconto = 0m;
        var valorFinal = 200m;
        var observacoes = "Reserva teste";
        var petIds = new List<long> { 1 };
        var usuarioId = 1L;

        // Act
        var reserva = ReservaDb.Create(clienteId, dataInicial, dataFinal, quantidadeDiarias, quantidadePets, valorTotal, valorDesconto, valorFinal, null, observacoes, petIds, usuarioId);

        // Assert
        reserva.Should().NotBeNull();
        reserva.ClienteId.Should().Be(clienteId);
        reserva.DataInicial.Should().Be(dataInicial);
        reserva.DataFinal.Should().Be(dataFinal);
        reserva.ValorTotal.Should().Be(valorTotal);
        reserva.Status.Should().Be(ReservaStatus.ReservaCriada);
        reserva.DataInclusao.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void ConfirmarReserva_ValidReserva_ShouldChangeStatus()
    {
        // Arrange
        var reserva = ReservaDb.Create(1, DateTime.Today.AddDays(1), DateTime.Today.AddDays(3), 2, 1, 200m, 0m, 200m, null, "Test", new List<long> { 1 }, 1);
        var usuarioId = 1L;

        // Act
        reserva.ConfirmarReserva(usuarioId);

        // Assert
        reserva.Status.Should().Be(ReservaStatus.ReservaConfirmada);
        reserva.DataAlteracao.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void CancelarReserva_ValidReserva_ShouldChangeStatus()
    {
        // Arrange
        var reserva = ReservaDb.Create(1, DateTime.Today.AddDays(1), DateTime.Today.AddDays(3), 2, 1, 200m, 0m, 200m, null, "Test", new List<long> { 1 }, 1);
        var usuarioId = 1L;

        // Act
        reserva.CancelarReserva(usuarioId);

        // Assert
        reserva.Status.Should().Be(ReservaStatus.ReservaCancelada);
        reserva.DataAlteracao.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void ConcederDesconto_ValidValue_ShouldUpdateValues()
    {
        // Arrange
        var reserva = ReservaDb.Create(1, DateTime.Today.AddDays(1), DateTime.Today.AddDays(3), 2, 1, 200m, 0m, 200m, null, "Test", new List<long> { 1 }, 1);
        var desconto = 50m;

        // Act
        reserva.ConcederDesconto(desconto);

        // Assert
        reserva.ValorDesconto.Should().Be(desconto);
        reserva.ValorFinal.Should().Be(150m);
        reserva.DataAlteracao.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void AplicarCupom_ValidCupom_ShouldUpdateValues()
    {
        // Arrange
        var reserva = ReservaDb.Create(1,
            DateTime.Today.AddDays(1),
            DateTime.Today.AddDays(3),
            2, 1, 200m, 0m, 200m, null, "Test", new List<long> { 1 }, 1);
        var cupomId = 1L;
        var desconto = 30m;

        // Act
        reserva.AplicarCupom(cupomId, desconto);

        // Assert
        reserva.CupomId.Should().Be(cupomId);
        reserva.ValorDesconto.Should().Be(desconto);
        reserva.ValorFinal.Should().Be(170m);
    }
}