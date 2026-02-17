using ColinhoDaCa.Domain.Racas.Entities;

namespace ColinhoDaCa.Domain.Racas.Repositories;

public interface IRacaRepository
{
    Task<List<Raca>> GetAllAsync();
    Task<Raca> GetByIdAsync(long id);
}