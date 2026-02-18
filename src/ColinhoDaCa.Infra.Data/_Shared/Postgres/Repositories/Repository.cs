using ColinhoDaCa.Domain._Shared.Entities;
using ColinhoDaCa.Domain._Shared.Exceptions;
using ColinhoDaCa.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

    public virtual async Task<List<TEntity>> GetAllAsync()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }

    public virtual async Task<TEntity> GetAsync(long key)
    {
        return await _context
            .Set<TEntity>()
            .FindAsync(key)
            ?? throw new EntityNotFoundException($"Entidade do tipo {typeof(TEntity).Name} com a chave {key} não encontrada.");
    }

    public virtual async Task<TEntity> GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool includeDetails = true)
    {
        var entity = await FindAsync(predicate, includeDetails);

        return entity ?? throw new EntityNotFoundException(typeof(TEntity));
    }

    public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _context
            .Set<TEntity>()
            .Where(predicate)
            //.ApplySoftDeleteFilter()
            .SingleOrDefaultAsync();
    }

    public virtual async Task<TEntity?> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool includeDetails = true)
    {
        IQueryable<TEntity> query = _context.Set<TEntity>();

        if (includeDetails)
        {
            var entityType = _context.Model.FindEntityType(typeof(TEntity));
            foreach (var navigation in entityType.GetNavigations())
            {
                query = query.Include(navigation.Name);
            }
        }

        return await query
            .Where(predicate)
            //.ApplySoftDeleteFilter()
            .SingleOrDefaultAsync();
    }

    public void Update(TEntity entity)
    {
        _context.Attach(entity);
        _context.Update(entity);
    }

    public void Update(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().UpdateRange(entities);
    }

    public void Delete(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
    }

    public void Delete(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().RemoveRange(entities);
    }
}