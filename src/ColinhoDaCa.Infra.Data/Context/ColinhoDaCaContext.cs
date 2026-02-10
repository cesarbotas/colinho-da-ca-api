using ColinhoDaCa.Domain.Clientes.Entities;
using ColinhoDaCa.Domain.Pets.Entities;
using ColinhoDaCa.Domain.Reservas.Entities;
using ColinhoDaCa.Domain.Usuarios.Entities;
using ColinhoDaCa.Infra.Data.Context.Configuration;
using Microsoft.EntityFrameworkCore;

namespace ColinhoDaCa.Infra.Data.Context;

public class ColinhoDaCaContext : DbContext
{
    public ColinhoDaCaContext(DbContextOptions<ColinhoDaCaContext> options) 
        : base(options)
    {

    }

    public DbSet<ClienteDb> Clientes { get; set; }
    public DbSet<PetDb> Pets { get; set; }
    public DbSet<ReservaDb> Reservas { get; set; }
    public DbSet<ReservaPetDb> ReservaPets { get; set; }
    public DbSet<UsuarioDb> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("public");

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ClienteConfiguration).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}