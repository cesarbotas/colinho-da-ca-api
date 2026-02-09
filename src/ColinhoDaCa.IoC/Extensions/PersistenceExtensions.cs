using Microsoft.Extensions.DependencyInjection;

namespace ColinhoDaCa.IoC.Extensions;

internal static class PersistenceExtensions
{
    internal static IServiceCollection AdicionarContextoDb(this IServiceCollection services/*, IConfiguration configuration*/)
    {
        // var connectionString = configuration.GetConnectionString("ColinhoDaCaRender");
        // services.AddDbContext<DbContext, ColinhodaCaContext>(options =>
        //     options.UseNpgsql(connectionString, npgsqlOptions =>
        //     {
        //         npgsqlOptions.EnableRetryOnFailure(             // Habilita o retry automático em caso de falhas
        //             maxRetryCount: 5,                           // Número máximo de tentativas
        //             maxRetryDelay: TimeSpan.FromSeconds(10),    // Tempo máximo de espera entre as tentativas
        //             errorCodesToAdd: null                       // Lista opcional de códigos de erro adicionais para considerar no retry
        //         );
        //     })
        // );

        return services;
    }

    internal static IServiceCollection AdicionarUnitOfWork(this IServiceCollection services)
    {
        // services
            // .AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    internal static IServiceCollection AdicionarRepositorios(this IServiceCollection services)
    {
        #region Repositórios Postgres

        // services.AddScoped<IVagasRepository, VagaRepository>();

        #endregion

        return services;
    }
}
