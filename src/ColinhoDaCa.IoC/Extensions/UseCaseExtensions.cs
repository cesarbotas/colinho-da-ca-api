using Microsoft.Extensions.DependencyInjection;
using ColinhoDaCa.Application.UseCases.Clientes.v1.CadastrarCliente;
using ColinhoDaCa.Application.UseCases.Clientes.v1.ListarCliente;

namespace ColinhoDaCa.IoC.Extensions;

internal static class UseCaseExtensions
{
    internal static IServiceCollection AdicionarUseCases(this IServiceCollection services)
    {
        services
            .AddScoped<ICadastrarClienteService, CadastrarClienteService>()
            .AddScoped<IListarClienteService, ListarClienteService>()
            //services.AddScoped<PesquisarCampusQueryHandler>();
            ;

        return services;
    }
}
