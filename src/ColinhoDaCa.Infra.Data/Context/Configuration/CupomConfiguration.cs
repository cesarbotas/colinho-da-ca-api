using ColinhoDaCa.Domain.Cupons.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ColinhoDaCa.Infra.Data.Context.Configuration;

public class CupomConfiguration : IEntityTypeConfiguration<CupomDb>
{
    public void Configure(EntityTypeBuilder<CupomDb> builder)
    {
        builder.ToTable("Cupons");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Codigo)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.Descricao)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Tipo)
            .IsRequired();

        builder.Property(c => c.Percentual)
            .IsRequired()
            .HasPrecision(5, 2);

        builder.Property(c => c.ValorFixo)
            .HasPrecision(10, 2);

        builder.Property(c => c.MinimoValorTotal)
            .HasPrecision(10, 2);

        builder.Property(c => c.MinimoPets);

        builder.Property(c => c.MinimoDiarias);

        builder.Property(c => c.DataInicio)
            .HasColumnType("timestamp without time zone");

        builder.Property(c => c.DataFim)
            .HasColumnType("timestamp without time zone");

        builder.Property(c => c.Ativo)
            .IsRequired();

        builder.Property(c => c.DataInclusao)
            .IsRequired()
            .HasColumnType("timestamp without time zone");

        builder.Property(c => c.DataAlteracao)
            .IsRequired()
            .HasColumnType("timestamp without time zone");

        builder.HasIndex(c => c.Codigo)
            .IsUnique();
    }
}
