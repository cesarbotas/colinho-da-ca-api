using ColinhoDaCa.Domain.RefreshTokens.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ColinhoDaCa.Infra.Data.Context.Configuration;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshTokenDb>
{
    public void Configure(EntityTypeBuilder<RefreshTokenDb> builder)
    {
        builder.ToTable("RefreshTokens");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();
            
        builder.Property(x => x.UsuarioId)
            .HasColumnName("UsuarioId")
            .IsRequired();
            
        builder.Property(x => x.Token)
            .HasColumnName("Token")
            .HasMaxLength(500)
            .IsRequired();
            
        builder.Property(x => x.ExpiresAt)
            .HasColumnName("ExpiresAt")
            .IsRequired();
            
        builder.Property(x => x.IsRevoked)
            .HasColumnName("IsRevoked")
            .IsRequired();
            
        builder.Property(x => x.CreatedAt)
            .HasColumnName("CreatedAt")
            .IsRequired();
            
        builder.Property(x => x.RevokedAt)
            .HasColumnName("RevokedAt");
    }
}