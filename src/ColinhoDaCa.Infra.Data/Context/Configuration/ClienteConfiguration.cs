using ColinhoDaCa.Domain.Clientes.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ColinhoDaCa.Infra.Data.Context.Configuration;

public class ClienteConfiguration : IEntityTypeConfiguration<ClienteDb>
{
    public void Configure(EntityTypeBuilder<ClienteDb> builder)
    {
        builder.ToTable("Clientes");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nome)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Celular)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(c => c.Cpf)
            .IsRequired()
            .HasMaxLength(11);

        builder.Property(c => c.Endereco)
            .HasMaxLength(500);

        builder.Property(c => c.Observacoes)
            .HasMaxLength(1000);
    }
}
