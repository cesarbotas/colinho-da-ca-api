using Microsoft.Extensions.DependencyInjection;
using ColinhoDaCa.Application.UseCases.Clientes.v1.CadastrarCliente;
using ColinhoDaCa.Application.UseCases.Clientes.v1.ListarCliente;
using ColinhoDaCa.Application.UseCases.Clientes.v1.AlterarCliente;
using ColinhoDaCa.Application.UseCases.Clientes.v1.ExcluirCliente;
using ColinhoDaCa.Application.UseCases.Pets.v1.CadastrarPet;
using ColinhoDaCa.Application.UseCases.Pets.v1.ListarPet;
using ColinhoDaCa.Application.UseCases.Pets.v1.AlterarPet;
using ColinhoDaCa.Application.UseCases.Pets.v1.ExcluirPet;
using ColinhoDaCa.Application.UseCases.Reservas.v1.CadastrarReserva;
using ColinhoDaCa.Application.UseCases.Reservas.v1.ListarReserva;
using ColinhoDaCa.Application.UseCases.Reservas.v1.AlterarReserva;
using ColinhoDaCa.Application.UseCases.Reservas.v1.ExcluirReserva;
using ColinhoDaCa.Application.UseCases.Sobre.v1.EnviarEmail;
using ColinhoDaCa.Application.UseCases.Auth.v1.Login;
using ColinhoDaCa.Application.UseCases.Auth.v1.Registrar;
using ColinhoDaCa.Application.Services.Email;
using ColinhoDaCa.Application.Services.Auth;
using ColinhoDaCa.Application.Services.Validation;

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
            .AddScoped<ICadastrarReservaService, CadastrarReservaService>()
            .AddScoped<IListarReservaService, ListarReservaService>()
            .AddScoped<IAlterarReservaService, AlterarReservaService>()
            .AddScoped<IExcluirReservaService, ExcluirReservaService>()
            .AddScoped<IEnviarEmailService, EnviarEmailService>()
            .AddScoped<ILoginService, LoginService>()
            .AddScoped<IRegistrarService, RegistrarService>()
            .AddScoped<IEmailService, EmailService>()
            .AddScoped<IPasswordService, PasswordService>()
            .AddScoped<IJwtService, JwtService>()
            .AddScoped<ICpfValidationService, CpfValidationService>()
            ;

        return services;
    }
}
