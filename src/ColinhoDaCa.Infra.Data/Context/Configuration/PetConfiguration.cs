using ColinhoDaCa.Domain.Clientes.Entities;
using ColinhoDaCa.Domain.Pets.Entities;
using ColinhoDaCa.Domain.Racas.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ColinhoDaCa.Infra.Data.Context.Configuration;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("Pets", "public");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Nome)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.RacaId);

        builder.Property(p => p.Idade)
            .IsRequired();

        builder.Property(p => p.Peso)
            .IsRequired();

        builder.Property(p => p.Porte)
            .HasMaxLength(1);

        builder.Property(p => p.Observacoes)
            .HasMaxLength(1000);

        builder.Property(p => p.ClienteId)
            .IsRequired();

        builder.Property(p => p.DataInclusao)
            .IsRequired()
            .HasColumnType("timestamp without time zone");

        builder.Property(p => p.DataAlteracao)
            .IsRequired()
            .HasColumnType("timestamp without time zone");

        builder.HasOne<Cliente>()
            .WithMany()
            .HasForeignKey("ClienteId")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<RacaDb>()
            .WithMany()
            .HasForeignKey("RacaId")
            .OnDelete(DeleteBehavior.Restrict);
    }
}