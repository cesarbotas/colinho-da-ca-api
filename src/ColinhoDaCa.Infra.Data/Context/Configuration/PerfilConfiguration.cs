using ColinhoDaCa.Domain.Perfis.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ColinhoDaCa.Infra.Data.Context.Configuration;

public class PerfilConfiguration : IEntityTypeConfiguration<PerfilDb>
{
    public void Configure(EntityTypeBuilder<PerfilDb> builder)
    {
        builder.ToTable("Perfis", "public");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .UseIdentityAlwaysColumn();

        builder.Property(p => p.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Descricao)
            .HasMaxLength(500);
    }
}
