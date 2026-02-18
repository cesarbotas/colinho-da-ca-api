using ColinhoDaCa.Domain.LoginHistoricos.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ColinhoDaCa.Infra.Data.Context.Configuration;

public class LoginHistoricoConfiguration : IEntityTypeConfiguration<LoginHistorico>
{
    public void Configure(EntityTypeBuilder<LoginHistorico> builder)
    {
        builder.ToTable("LoginHistorico");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();
            
        builder.Property(x => x.UsuarioId)
            .HasColumnName("UsuarioId")
            .IsRequired();
            
        builder.Property(x => x.Email)
            .HasColumnName("Email")
            .HasMaxLength(255)
            .IsRequired();
            
        builder.Property(x => x.UserAgent)
            .HasColumnName("UserAgent");
            
        builder.Property(x => x.Platform)
            .HasColumnName("Platform")
            .HasMaxLength(100);
            
        builder.Property(x => x.Language)
            .HasColumnName("Language")
            .HasMaxLength(10);
            
        builder.Property(x => x.ScreenResolution)
            .HasColumnName("ScreenResolution")
            .HasMaxLength(20);
            
        builder.Property(x => x.Timezone)
            .HasColumnName("Timezone")
            .HasMaxLength(50);
            
        builder.Property(x => x.ClientIP)
            .HasColumnName("ClientIP")
            .HasMaxLength(45);
            
        builder.Property(x => x.DataLogin)
            .HasColumnName("DataLogin")
            .IsRequired();
    }
}