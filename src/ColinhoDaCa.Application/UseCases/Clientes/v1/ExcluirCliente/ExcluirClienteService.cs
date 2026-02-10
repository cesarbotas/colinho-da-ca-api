using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain.Clientes.Repositories;
using Microsoft.Extensions.Logging;

namespace ColinhoDaCa.Application.UseCases.Clientes.v1.ExcluirCliente;

public class ExcluirClienteService : IExcluirClienteService
{
    private readonly ILogger<ExcluirClienteService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClienteRepository _clienteRepository;

    public ExcluirClienteService(ILogger<ExcluirClienteService> logger, 
        IUnitOfWork unitOfWork, 
        IClienteRepository clienteRepository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _clienteRepository = clienteRepository;
    }

    public async Task Handle(long id)
    {
        try
        {
            var cliente = await _clienteRepository.GetAsync(id);

            if (cliente == null)
            {
                throw new Exception("Cliente não encontrado");
            }

            _clienteRepository.Delete(cliente);

            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Problema na Alteração de Clientes");

            throw;
        }
    }
}