using ColinhoDaCa.Domain.Reservas.Entities;
using ColinhoDaCa.Domain.Usuarios.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ColinhoDaCa.Infra.Data.Context.Configuration;

public class ReservaStatusHistoricoConfiguration : IEntityTypeConfiguration<ReservaStatusHistoricoDb>
{
    public void Configure(EntityTypeBuilder<ReservaStatusHistoricoDb> builder)
    {
        builder.ToTable("ReservaStatusHistorico");
        builder.HasKey(h => h.Id);
        builder.Property(h => h.ReservaId)
            .HasColumnName("ReservaId")
            .IsRequired();
        builder.Property(h => h.Status).IsRequired().HasConversion<int>();
        builder.Property(h => h.UsuarioId).IsRequired();
        builder.Property(h => h.DataAlteracao).IsRequired().HasColumnType("timestamp without time zone");

        builder.HasOne<ReservaDb>()
            .WithMany()
            .HasForeignKey(h => h.ReservaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<UsuarioDb>()
            .WithMany()
            .HasForeignKey(h => h.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
