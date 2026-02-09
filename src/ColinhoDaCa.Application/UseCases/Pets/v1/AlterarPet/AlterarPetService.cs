using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain.Pets.Repositories;
using Microsoft.Extensions.Logging;

namespace ColinhoDaCa.Application.UseCases.Pets.v1.AlterarPet;

public class AlterarPetService : IAlterarPetService
{
    private readonly ILogger<AlterarPetService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPetRepository _petRepository;

    public AlterarPetService(ILogger<AlterarPetService> logger, 
        IUnitOfWork unitOfWork, 
        IPetRepository petRepository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _petRepository = petRepository;
    }

    public async Task Handle(long id, AlterarPetCommand command)
    {
        try
        {
            var pet = await _petRepository.GetAsync(c => c.Id == id);

            if (pet == null)
            {
                throw new Exception("Pet não encontrado");
            }

             pet.Alterar(command.Nome, command.Raca, command.Idade, command.Peso, command.Porte, command.Observacoes, command.ClienteId);

             _petRepository.Update(pet);
            
            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Problema na Alteração de Pets");

            throw;
        }
    }
}