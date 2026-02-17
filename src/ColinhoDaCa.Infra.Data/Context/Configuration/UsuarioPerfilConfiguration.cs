using ColinhoDaCa.Domain.Perfis.Entities;
using ColinhoDaCa.Domain.Usuarios.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ColinhoDaCa.Infra.Data.Context.Configuration;

public class UsuarioPerfilConfiguration : IEntityTypeConfiguration<UsuarioPerfil>
{
    public void Configure(EntityTypeBuilder<UsuarioPerfil> builder)
    {
        builder.ToTable("UsuarioPerfis", "public");

        builder.HasKey(up => new { up.UsuarioId, up.PerfilId });

        builder.Property(up => up.UsuarioId)
            .IsRequired();

        builder.Property(up => up.PerfilId)
            .IsRequired();

        builder.HasOne<Perfil>()
            .WithMany()
            .HasForeignKey(up => up.PerfilId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}