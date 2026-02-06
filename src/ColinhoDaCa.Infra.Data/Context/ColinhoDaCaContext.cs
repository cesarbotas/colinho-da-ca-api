using ColinhoDaCa.Domain.Clientes.Entities;
using Microsoft.EntityFrameworkCore;

namespace ColinhoDaCa.Infra.Data.Context;

public class ColinhoDaCaContext : DbContext
{
    public ColinhoDaCaContext(DbContextOptions<ColinhoDaCaContext> options) : base(options)
    {

    }

    public DbSet<ClienteDb> Clientes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ColinhoDaCaContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}