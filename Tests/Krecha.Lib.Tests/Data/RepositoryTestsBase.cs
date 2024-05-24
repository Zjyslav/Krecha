using AutoFixture;
using Krecha.Lib.Data;
using Krecha.Lib.Tests.Helpers;

namespace Krecha.Lib.Tests.Data;

public abstract class RepositoryTestsBase
{
    protected SettlementsDbContext DbContext { get; }
    protected Fixture Fixture { get; } = new();

    public RepositoryTestsBase()
    {
        DbContext = EFHelpers.SetupInMemoryDbContext();
    }

    protected void AddEntitiesToInMemoryDb<TEntity>(List<TEntity> entities)
        where TEntity : class
    {
        DbContext
            .Set<TEntity>()
            .AddRange(entities);

        DbContext.SaveChanges();
    }
}