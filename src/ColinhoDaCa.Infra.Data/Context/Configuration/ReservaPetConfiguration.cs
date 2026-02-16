using ColinhoDaCa.Domain.Pets.Entities;
using ColinhoDaCa.Domain.Reservas.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ColinhoDaCa.Infra.Data.Context.Configuration;

public class ReservaPetConfiguration : IEntityTypeConfiguration<ReservaPetDb>
{
    public void Configure(EntityTypeBuilder<ReservaPetDb> builder)
    {
        builder.ToTable("ReservaPets", "public");

        builder.HasKey(rp => new { rp.ReservaId, rp.PetId });

        builder.Property(rp => rp.ReservaId)
            .HasColumnName("ReservaId")
            .IsRequired();

        builder.Property(rp => rp.PetId)
            .HasColumnName("PetId")
            .IsRequired();

        builder.HasOne<ReservaDb>()
            .WithMany()
            .HasForeignKey(rp => rp.ReservaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<PetDb>()
            .WithMany()
            .HasForeignKey(rp => rp.PetId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}