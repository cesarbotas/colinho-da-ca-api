using ColinhoDaCa.Domain.Usuarios.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ColinhoDaCa.Infra.Data.Context.Configuration;

public class UsuarioConfiguration : IEntityTypeConfiguration<UsuarioDb>
{
    public void Configure(EntityTypeBuilder<UsuarioDb> builder)
    {
        builder.ToTable("Usuarios", "public");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .UseIdentityAlwaysColumn();

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Nome)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.SenhaHash)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(u => u.ClienteId);

        builder.HasOne<ColinhoDaCa.Domain.Clientes.Entities.ClienteDb>()
            .WithMany()
            .HasForeignKey(u => u.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(u => u.DataInclusao)
            .IsRequired()
            .HasColumnType("timestamp without time zone");

        builder.Property(u => u.DataAlteracao)
            .IsRequired()
            .HasColumnType("timestamp without time zone");
    }
}