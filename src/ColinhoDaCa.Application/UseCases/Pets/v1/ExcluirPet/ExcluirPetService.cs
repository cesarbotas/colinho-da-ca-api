using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain.Pets.Repositories;
using ColinhoDaCa.Domain.Reservas.Entities;
using Microsoft.Extensions.Logging;

namespace ColinhoDaCa.Application.UseCases.Pets.v1.ExcluirPet;

public class ExcluirPetService : IExcluirPetService
{
    private readonly ILogger<ExcluirPetService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPetRepository _petRepository;

    public ExcluirPetService(ILogger<ExcluirPetService> logger, 
        IUnitOfWork unitOfWork, 
        IPetRepository petRepository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _petRepository = petRepository;
    }

    public async Task Handle(long id)
    {
        try
        {
            var pet = await _petRepository.GetAsync(id);

            if (pet == null)
            {
                throw new Exception("Pet não encontrado");
            }

            // Check if pet has any reservations
            var hasReservations = await _petRepository.HasReservationsAsync(id);

            if (hasReservations)
            {
                // Inactivate pet if has reservations
                pet.Inativar();
                _petRepository.Update(pet);
            }
            else
            {
                // Delete pet if no reservations
                _petRepository.Delete(pet);
            }

            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Problema na Exclusão de Pets");

            throw;
        }
    }
}
