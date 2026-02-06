using ColinhoDaCa.Domain.Clientes.Entities;
using ColinhoDaCa.Infra.Data.Context;
using Microsoft.Extensions.Logging;

namespace ColinhoDaCa.Application.UseCases.Clientes.CadastrarCliente;

public class CadastrarClienteService : ICadastrarClienteService
{
    private readonly ILogger<CadastrarClienteService> _logger;
    private readonly ColinhoDaCaContext _context;

    public CadastrarClienteService(ILogger<CadastrarClienteService> logger, ColinhoDaCaContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task Execute(CadastrarClienteCommand command)
    {
        var cliente = new ClienteDb
        {
            Nome = command.Nome,
            Email = command.Email,
            Celular = command.Celular,
            Cpf = command.Cpf,
            Endereco = command.Endereco,
            Observacoes = command.Observacoes
        };

        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();
    }
}
