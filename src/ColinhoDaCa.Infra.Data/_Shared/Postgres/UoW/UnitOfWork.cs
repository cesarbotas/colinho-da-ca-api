using ColinhoDaCa.Domain._Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace ColinhoDaCa.Infra.Data._Shared.Postgres.UoW;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _dbContext;

    public UnitOfWork(DbContext dbContext)
        => _dbContext = dbContext;

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        => await _dbContext.SaveChangesAsync(cancellationToken);
}