using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain.Clientes.Repositories;
using ColinhoDaCa.Domain.Pets.Repositories;
using ColinhoDaCa.Domain.Racas.Repositories;
using ColinhoDaCa.Domain.Reservas.Repositories;
using ColinhoDaCa.Domain.Usuarios.Repositories;
using ColinhoDaCa.Infra.Data._Shared.Postgres.UoW;
using ColinhoDaCa.Infra.Data.Context;
using ColinhoDaCa.Infra.Data.Context.Repositories.Clientes;
using ColinhoDaCa.Infra.Data.Context.Repositories.Pets;
using ColinhoDaCa.Infra.Data.Context.Repositories.Reservas;
using ColinhoDaCa.Infra.Data.Context.Repositories.Usuarios;
using ColinhoDaCa.Infra.Data.Racas;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ColinhoDaCa.IoC.Extensions;

internal static class PersistenceExtensions
{
    internal static IServiceCollection AdicionarContextoDb(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ColinhoDaCaRender");
        services.AddDbContext<DbContext, ColinhoDaCaContext>(options =>
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.EnableRetryOnFailure(             // Habilita o retry automático em caso de falhas
                    maxRetryCount: 5,                           // Número máximo de tentativas
                    maxRetryDelay: TimeSpan.FromSeconds(10),    // Tempo máximo de espera entre as tentativas
                    errorCodesToAdd: null                       // Lista opcional de códigos de erro adicionais para considerar no retry
                );
            })
        );

        return services;
    }

    internal static IServiceCollection AdicionarUnitOfWork(this IServiceCollection services)
    {
        services
            .AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    internal static IServiceCollection AdicionarRepositorios(this IServiceCollection services)
    {
        #region Repositórios Postgres

        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IClienteReadRepository, ClienteRepository>();

        services.AddScoped<IPetRepository, PetRepository>();
        services.AddScoped<IPetReadRepository, PetRepository>();

        services.AddScoped<IRacaRepository, RacaRepository>();

        services.AddScoped<IReservaRepository, ReservaRepository>();
        services.AddScoped<IReservaReadRepository, ReservaRepository>();

        services.AddScoped<IUsuarioRepository, UsuarioRepository>();

        #endregion

        return services;
    }
}
