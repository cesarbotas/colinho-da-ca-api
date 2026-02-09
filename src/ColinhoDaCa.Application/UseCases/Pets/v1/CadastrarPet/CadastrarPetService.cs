using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain.Pets.Entities;
using ColinhoDaCa.Domain.Pets.Repositories;
using Microsoft.Extensions.Logging;

namespace ColinhoDaCa.Application.UseCases.Pets.v1.CadastrarPet;

public class CadastrarPetService : ICadastrarPetService
{
    private readonly ILogger<CadastrarPetService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPetRepository _petRepository;

    public CadastrarPetService(ILogger<CadastrarPetService> logger,
        IUnitOfWork unitOfWork,
        IPetRepository petRepository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _petRepository = petRepository;
    }

    public async Task Handle(CadastrarPetCommand command)
    {
        try
        {
            var pet = PetDb.Create(command.Nome, command.Raca, command.Idade, command.Peso, command.Observacoes, command.ClienteId);

            await _petRepository.InsertAsync(pet);

            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Problema na Inclusão de Pets");

            throw;
        }
    }
}