using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain.Clientes.Entities;
using ColinhoDaCa.Domain.Clientes.Repositories;
using Microsoft.Extensions.Logging;

namespace ColinhoDaCa.Application.UseCases.Clientes.v1.CadastrarCliente;

public class CadastrarClienteService : ICadastrarClienteService
{
    private readonly ILogger<CadastrarClienteService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClienteRepository _clienteRepository;

    public CadastrarClienteService(ILogger<CadastrarClienteService> logger,
        IUnitOfWork unitOfWork,
        IClienteRepository clienteRepository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _clienteRepository = clienteRepository;
    }

    public async Task Handle(CadastrarClienteCommand command)
    {
        try
        {
            var cliente = ClienteDb.Create(command.Nome, command.Email, command.Celular, command.Cpf, command.Endereco, command.Observacoes);

            await _clienteRepository.InsertAsync(cliente);

            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Problema na Inclusão de Clientes");

            throw;
        }
    }
}