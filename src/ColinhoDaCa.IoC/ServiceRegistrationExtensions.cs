using System.Diagnostics.CodeAnalysis;
using ColinhoDaCa.IoC.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace ColinhoDaCa.IoC;

[ExcludeFromCodeCoverage]
public static class ServiceRegistrationExtensions
{
    public static IServiceCollection RegistraDependencias(this IServiceCollection services/*, IConfiguration configuration*/)
    {
        services
            // .AddSingleton(configuration)
            // .AdicionarConfiguracoes(configuration)
            // .AdicionarServicos(configuration)
            // .AdicionarUnitOfWork()
            // .AdicionarContextoDb(configuration)
            //.AdicionarSqlServer(configuration)
            //.AdicionarDatabricks(configuration)
            // .AdicionarRepositorios()
            // .AdicionarApplicationValidators()
            // .AdicionarDomainValidators()
            .AdicionarUseCases()
            ;

        return services;
    }
}