using ColinhoDaCa.Domain.Usuarios.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ColinhoDaCa.Infra.Data.Context.Configuration;

public class UsuarioPerfilConfiguration : IEntityTypeConfiguration<UsuarioPerfilDb>
{
    public void Configure(EntityTypeBuilder<UsuarioPerfilDb> builder)
    {
        builder.ToTable("UsuarioPerfis", "public");

        builder.HasKey(up => new { up.UsuarioId, up.PerfilId });

        builder.Property(up => up.UsuarioId)
            .IsRequired();

        builder.Property(up => up.PerfilId)
            .IsRequired();

        builder.HasOne<ColinhoDaCa.Domain.Perfis.Entities.PerfilDb>()
            .WithMany()
            .HasForeignKey(up => up.PerfilId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
