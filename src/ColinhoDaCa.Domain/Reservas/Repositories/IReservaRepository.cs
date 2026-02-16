using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain.Reservas.Entities;

namespace ColinhoDaCa.Domain.Reservas.Repositories;

public interface IReservaRepository : IRepository<Reserva>
{
    Task<Reserva?> GetWithRelationsAsync(long id);
}
