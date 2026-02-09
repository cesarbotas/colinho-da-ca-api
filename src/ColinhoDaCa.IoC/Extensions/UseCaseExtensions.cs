using Microsoft.Extensions.DependencyInjection;

using ColinhoDaCa.Application.UseCases.Clientes.CadastrarCliente;

namespace ColinhoDaCa.IoC.Extensions;

internal static class UseCaseExtensions
{
    internal static IServiceCollection AdicionarUseCases(this IServiceCollection services)
    {
        services
            .AddScoped<ICadastrarClienteService, CadastrarClienteService>();
        //services.AddScoped<PesquisarCampusQueryHandler>();

        return services;
    }
}
