using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain.Pets.Entities;

namespace ColinhoDaCa.Domain.Pets.Repositories;

public interface IPetRepository : IRepository<Pet>
{
    Task<bool> HasReservationsAsync(long petId);
}
