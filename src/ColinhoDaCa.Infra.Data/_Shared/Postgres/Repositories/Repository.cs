using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Infra.Data.Context;

namespace ColinhoDaCa.Infra.Data._Shared.Postgres.Repositories;

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    private readonly ColinhoDaCaContext _context;

    public Repository(ColinhoDaCaContext context)
    {
        _context = context;
    }

    public async Task InsertAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
    }

    public async Task InsertAsync(IEnumerable<TEntity> entities)
    {
        await _context.Set<TEntity>().AddRangeAsync(entities);
    }
}