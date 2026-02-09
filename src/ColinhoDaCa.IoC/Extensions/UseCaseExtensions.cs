using Microsoft.Extensions.DependencyInjection;
using ColinhoDaCa.Application.UseCases.Clientes.v1.CadastrarCliente;
using ColinhoDaCa.Application.UseCases.Clientes.v1.ListarCliente;
using ColinhoDaCa.Application.UseCases.Clientes.v1.AlterarCliente;
using ColinhoDaCa.Application.UseCases.Clientes.v1.ExcluirCliente;

namespace ColinhoDaCa.IoC.Extensions;

internal static class UseCaseExtensions
{
    internal static IServiceCollection AdicionarUseCases(this IServiceCollection services)
    {
        services
            .AddScoped<ICadastrarClienteService, CadastrarClienteService>()
            .AddScoped<IListarClienteService, ListarClienteService>()
            .AddScoped<IAlterarClienteService, AlterarClienteService>()
            .AddScoped<IExcluirClienteService, ExcluirClienteService>()
            //services.AddScoped<PesquisarCampusQueryHandler>();
            ;

        return services;
    }
}
