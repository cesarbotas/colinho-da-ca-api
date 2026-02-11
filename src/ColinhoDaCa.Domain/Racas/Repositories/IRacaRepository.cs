using ColinhoDaCa.Domain.Racas.Entities;

namespace ColinhoDaCa.Domain.Racas.Repositories;

public interface IRacaRepository
{
    Task<List<RacaDb>> GetAllAsync();
    Task<RacaDb> GetByIdAsync(long id);
}
