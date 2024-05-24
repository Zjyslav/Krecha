using Krecha.Lib.Interfaces.Data;
using Microsoft.EntityFrameworkCore;

namespace Krecha.Lib.Data;
public class Repository<TEntity, UDbContext> : IRepository<TEntity>
    where TEntity : class
    where UDbContext : DbContext
{
    private readonly UDbContext _dbContext;

    public Repository(UDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<TEntity> GetAll() => _dbContext.Set<TEntity>().AsQueryable();
    public async Task<TEntity?> GetById<VId>(VId id) => await _dbContext.Set<TEntity>().FindAsync(id);

    public async Task<TEntity?> Create(TEntity entity)
    {
        _dbContext.Set<TEntity>().Add(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<TEntity?> Update<VId>(VId id, Action<TEntity> update)
    {
        var toUpdate = await GetById(id);
        if (toUpdate is null)
            return null;

        update.Invoke(toUpdate);
        await _dbContext.SaveChangesAsync();

        return toUpdate;
    }

    public async Task<TEntity?> Delete<VId>(VId id)
    {
        var toDelete = await GetById(id);
        if (toDelete is null)
            return null;

        _dbContext.Set<TEntity>().Remove(toDelete);
        await _dbContext.SaveChangesAsync();

        return toDelete;
    }
}
