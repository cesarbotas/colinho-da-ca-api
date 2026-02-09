using Microsoft.Extensions.DependencyInjection;
using ColinhoDaCa.Application.UseCases.Clientes.v1.CadastrarCliente;
using ColinhoDaCa.Application.UseCases.Clientes.v1.ListarCliente;
using ColinhoDaCa.Application.UseCases.Clientes.v1.AlterarCliente;
using ColinhoDaCa.Application.UseCases.Clientes.v1.ExcluirCliente;
using ColinhoDaCa.Application.UseCases.Pets.v1.CadastrarPet;
using ColinhoDaCa.Application.UseCases.Pets.v1.ListarPet;
using ColinhoDaCa.Application.UseCases.Pets.v1.AlterarPet;
using ColinhoDaCa.Application.UseCases.Pets.v1.ExcluirPet;

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
            .AddScoped<ICadastrarPetService, CadastrarPetService>()
            .AddScoped<IListarPetService, ListarPetService>()
            .AddScoped<IAlterarPetService, AlterarPetService>()
            .AddScoped<IExcluirPetService, ExcluirPetService>()
            ;

        return services;
    }
}
