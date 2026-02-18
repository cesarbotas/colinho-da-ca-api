using ColinhoDaCa.Domain.Racas.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ColinhoDaCa.Infra.Data.Context.Configuration;

public class RacaConfiguration : IEntityTypeConfiguration<Raca>
{
    public void Configure(EntityTypeBuilder<Raca> builder)
    {
        builder.ToTable("Racas");

        builder.HasKey(r => r.Id);

        builder.Property(c => c.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder.Property(r => r.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.Porte)
            .HasMaxLength(1);
    }
}