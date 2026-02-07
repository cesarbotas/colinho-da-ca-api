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
        try
        {
            var cliente = new ClienteDb(command);

            _context.Clientes.Add(cliente);

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Problema na Inclusão de Clientes");

            throw;
        }
    }
}