using ColinhoDaCa.Domain.Reservas.Entities;
using ColinhoDaCa.Domain.Reservas.Enums;
using FluentAssertions;
using Xunit;

namespace ColinhoDaCa.TestesUnitarios.Domain;

public class ReservaEntitiesTests
{
    [Fact]
    public void ReservaPet_Create_DeveRetornarReservaPetValida()
    {
        // Arrange
        var reservaId = 1L;
        var petId = 2L;

        // Act
        var reservaPet = ReservaPet.Create(reservaId, petId);

        // Assert
        reservaPet.ReservaId.Should().Be(reservaId);
        reservaPet.PetId.Should().Be(petId);
    }

    [Fact]
    public void ReservaStatusHistorico_Create_DeveRetornarHistoricoValido()
    {
        // Arrange
        var reservaId = 1L;
        var status = ReservaStatus.ReservaConfirmada;
        var usuarioId = 2L;

        // Act
        var historico = ReservaStatusHistorico.Create(reservaId, status, usuarioId);

        // Assert
        historico.ReservaId.Should().Be(reservaId);
        historico.Status.Should().Be(status);
        historico.UsuarioId.Should().Be(usuarioId);
        historico.DataAlteracao.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }
}