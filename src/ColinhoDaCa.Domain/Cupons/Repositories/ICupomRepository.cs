using ColinhoDaCa.Domain.Cupons.Entities;

namespace ColinhoDaCa.Domain.Cupons.Repositories;

public interface ICupomRepository
{
    Task<Cupom?> GetByCodigoAsync(string codigo);
    Task<Cupom?> GetAsync(Func<Cupom, bool> predicate);
    Task<List<Cupom>> GetAllAsync();
    Task InsertAsync(Cupom cupom);
    void Update(Cupom cupom);
}