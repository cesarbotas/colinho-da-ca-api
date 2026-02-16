using ColinhoDaCa.Domain.Reservas.Entities;
using ColinhoDaCa.Domain.Usuarios.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ColinhoDaCa.Infra.Data.Context.Configuration;

public class ReservaStatusHistoricoConfiguration : IEntityTypeConfiguration<ReservaStatusHistorico>
{
    public void Configure(EntityTypeBuilder<ReservaStatusHistorico> builder)
    {
        builder.ToTable("ReservaStatusHistorico");

        builder.HasKey(h => h.Id);

        builder.Property(h => h.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder.Property(h => h.ReservaId)
            .HasColumnName("ReservaId")
            .IsRequired();

        builder.Property(h => h.Status)
            .IsRequired()
            .HasConversion<int>();
        
        builder.Property(h => h.UsuarioId)
            .HasColumnName("UsuarioId")
            .IsRequired();
        
        builder.Property(h => h.DataAlteracao)
            .IsRequired()
            .HasColumnType("timestamp without time zone");

        builder.HasOne(h => h.Reserva)
            .WithMany(r => r.StatusHistorico)
            .HasForeignKey(h => h.ReservaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(h => h.Usuario)
            .WithMany()
            .HasForeignKey(h => h.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}