using ColinhoDaCa.Domain.Cupons.Entities;
using ColinhoDaCa.Domain.Cupons.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ColinhoDaCa.Infra.Data.Context.Repositories.Cupons;

public class CupomRepository : ICupomRepository
{
    private readonly ColinhoDaCaContext _context;

    public CupomRepository(ColinhoDaCaContext context)
    {
        _context = context;
    }

    public async Task<Cupom?> GetByCodigoAsync(string codigo)
    {
        return await _context.Cupons
            .FirstOrDefaultAsync(c => c.Codigo == codigo);
    }

    public async Task<Cupom?> GetAsync(Func<Cupom, bool> predicate)
    {
        return await Task.FromResult(_context.Cupons.FirstOrDefault(predicate));
    }

    public async Task<List<Cupom>> GetAllAsync()
    {
        return await _context.Cupons.ToListAsync();
    }

    public async Task InsertAsync(Cupom cupom)
    {
        await _context.Cupons.AddAsync(cupom);
    }

    public void Update(Cupom cupom)
    {
        _context.Cupons.Update(cupom);
    }
}
