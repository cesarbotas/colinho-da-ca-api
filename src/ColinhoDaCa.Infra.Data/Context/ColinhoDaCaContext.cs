using ColinhoDaCa.Domain.Clientes.Entities;
using ColinhoDaCa.Domain.Cupons.Entities;
using ColinhoDaCa.Domain.LoginHistorico.Entities;
using ColinhoDaCa.Domain.Pets.Entities;
using ColinhoDaCa.Domain.Racas.Entities;
using ColinhoDaCa.Domain.RefreshTokens.Entities;
using ColinhoDaCa.Domain.Reservas.Entities;
using ColinhoDaCa.Domain.Usuarios.Entities;
using ColinhoDaCa.Domain.Perfis.Entities;
using ColinhoDaCa.Infra.Data.Context.Configuration;
using Microsoft.EntityFrameworkCore;

namespace ColinhoDaCa.Infra.Data.Context;

public class ColinhoDaCaContext : DbContext
{
    public ColinhoDaCaContext(DbContextOptions<ColinhoDaCaContext> options) 
        : base(options)
    {

    }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Pet> Pets { get; set; }
    public DbSet<RacaDb> Racas { get; set; }
    public DbSet<Reserva> Reservas { get; set; }
    public DbSet<ReservaPet> ReservaPets { get; set; }
    public DbSet<ReservaStatusHistorico> ReservaStatusHistorico { get; set; }
    public DbSet<CupomDb> Cupons { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Perfil> Perfis { get; set; }
    public DbSet<UsuarioPerfil> UsuarioPerfis { get; set; }
    public DbSet<LoginHistoricoDb> LoginHistorico { get; set; }
    public DbSet<RefreshTokenDb> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("public");

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ClienteConfiguration).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}