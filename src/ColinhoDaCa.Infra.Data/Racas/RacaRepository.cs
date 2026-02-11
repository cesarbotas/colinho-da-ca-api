using ColinhoDaCa.Domain.Racas.Entities;
using ColinhoDaCa.Domain.Racas.Repositories;
using ColinhoDaCa.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ColinhoDaCa.Infra.Data.Racas;

public class RacaRepository : IRacaRepository
{
    private readonly ColinhoDaCaContext _context;

    public RacaRepository(ColinhoDaCaContext context)
    {
        _context = context;
    }

    public async Task<List<RacaDb>> GetAllAsync()
    {
        return await _context.Racas.OrderBy(r => r.Nome).ToListAsync();
    }

    public async Task<RacaDb> GetByIdAsync(long id)
    {
        return await _context.Racas.FirstOrDefaultAsync(r => r.Id == id);
    }
}
