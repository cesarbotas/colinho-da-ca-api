using ColinhoDaCa.Domain.Usuarios.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ColinhoDaCa.Infra.Data.Context.Configuration;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios", "public");

        builder.HasKey(u => u.Id);

        builder.Property(c => c.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder.Property(u => u.SenhaHash)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(u => u.ClienteId)
            .HasColumnName("ClienteId")
            .IsRequired();

        builder.Property(u => u.Ativo)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasOne(u => u.Cliente)
            .WithMany()
            .HasForeignKey(u => u.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(u => u.ClienteId)
            .IsUnique();

        builder.Property(u => u.DataInclusao)
            .IsRequired()
            .HasColumnType("timestamp without time zone");

        builder.Property(u => u.DataAlteracao)
            .IsRequired()
            .HasColumnType("timestamp without time zone");

        // Configurar relacionamento com UsuarioPerfis
        builder.HasMany(u => u.UsuarioPerfis)
            .WithOne()
            .HasForeignKey(up => up.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}