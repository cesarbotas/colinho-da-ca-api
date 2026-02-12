using ColinhoDaCa.Domain.Cupons.Entities;

namespace ColinhoDaCa.Domain.Cupons.Repositories;

public interface ICupomRepository
{
    Task<CupomDb?> GetByCodigoAsync(string codigo);
    Task<CupomDb?> GetAsync(Func<CupomDb, bool> predicate);
    Task<List<CupomDb>> GetAllAsync();
    Task InsertAsync(CupomDb cupom);
    void Update(CupomDb cupom);
}
