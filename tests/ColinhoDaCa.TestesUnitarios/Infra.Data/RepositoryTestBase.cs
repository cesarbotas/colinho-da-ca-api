using ColinhoDaCa.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ColinhoDaCa.TestesUnitarios.Infra.Data;

public abstract class RepositoryTestBase : IDisposable
{
    protected readonly ColinhoDaCaContext Context;

    protected RepositoryTestBase()
    {
        var options = new DbContextOptionsBuilder<ColinhoDaCaContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        Context = new ColinhoDaCaContext(options);
    }

    public void Dispose()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }
}
