using ColinhoDaCa.Domain.Clientes.Entities;
using ColinhoDaCa.Domain.Reservas.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ColinhoDaCa.Infra.Data.Context.Configuration;

public class ReservaConfiguration : IEntityTypeConfiguration<ReservaDb>
{
    public void Configure(EntityTypeBuilder<ReservaDb> builder)
    {
        builder.ToTable("Reservas", "public");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .UseIdentityAlwaysColumn();

        builder.Property(r => r.ClienteId)
            .IsRequired();

        builder.Property(r => r.DataInicial)
            .IsRequired()
            .HasColumnType("timestamp without time zone");

        builder.Property(r => r.DataFinal)
            .IsRequired()
            .HasColumnType("timestamp without time zone");

        builder.Property(r => r.QuantidadeDiarias)
            .IsRequired();

        builder.Property(r => r.QuantidadePets)
            .IsRequired();

        builder.Property(r => r.ValorTotal)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(r => r.ValorDesconto)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(r => r.ValorFinal)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(r => r.Observacoes)
            .HasMaxLength(1000);

        builder.Property(r => r.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(r => r.ComprovantePagamento)
            .HasColumnType("text");

        builder.Property(r => r.DataPagamento)
            .HasColumnType("timestamp without time zone");

        builder.Property(r => r.ObservacoesPagamento)
            .HasColumnType("text");

        builder.Property(r => r.DataInclusao)
            .IsRequired()
            .HasColumnType("timestamp without time zone");

        builder.Property(r => r.DataAlteracao)
            .IsRequired()
            .HasColumnType("timestamp without time zone");

        builder.HasOne<ClienteDb>()
            .WithMany()
            .HasForeignKey(r => r.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}