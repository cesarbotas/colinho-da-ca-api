using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain._Shared.Exceptions;
using ColinhoDaCa.Domain.Clientes.Repositories;
using Microsoft.Extensions.Logging;

namespace ColinhoDaCa.Application.UseCases.Clientes.v1.AlterarCliente;

public class AlterarClienteService : IAlterarClienteService
{
    private readonly ILogger<AlterarClienteService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClienteRepository _clienteRepository;

    public AlterarClienteService(ILogger<AlterarClienteService> logger, 
        IUnitOfWork unitOfWork, 
        IClienteRepository clienteRepository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _clienteRepository = clienteRepository;
    }

    public async Task Handle(long id, AlterarClienteCommand command)
    {
        try
        {
            var cliente = await _clienteRepository.GetAsync(c => c.Id == id);

            if (cliente == null)
            {
                throw new EntityNotFoundException("Cliente não encontrado");
            }

            var clienteExistenteEmail = await _clienteRepository.GetByEmailAsync(command.Email);
            if (clienteExistenteEmail != null && clienteExistenteEmail.Id != id)
            {
                throw new ValidationException("Email já cadastrado para outro cliente");
            }

            var clienteExistenteCpf = await _clienteRepository.GetByCpfAsync(command.Cpf);
            if (clienteExistenteCpf != null && clienteExistenteCpf.Id != id)
            {
                throw new ValidationException("CPF já cadastrado para outro cliente");
            }

             cliente.Alterar(command.Nome, command.Email, command.Celular, command.Cpf, command.Observacoes);

             _clienteRepository.Update(cliente);
            
            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Problema na Alteração de Clientes");

            throw;
        }
    }
}