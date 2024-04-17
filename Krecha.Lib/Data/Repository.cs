using Microsoft.EntityFrameworkCore;

namespace Krecha.Lib.Data;
public class Repository<T>
    where T : class
{
    private readonly SettlementsDbContext _dbContext;

    public Repository(SettlementsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<T> GetAll() => _dbContext.Set<T>().AsQueryable();
    public async Task<T?> GetById<U>(U id) => await _dbContext.Set<T>().FindAsync(id);

    public async Task<T?> Create(T entity)
    {
        _dbContext.Set<T>().Add(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<T?> Update<U>(U id, Action<T> update)
    {
        var toUpdate = await GetById(id);
        if (toUpdate is null)
            return null;

        update.Invoke(toUpdate);
        await _dbContext.SaveChangesAsync();

        return toUpdate;
    }

    public async Task<T?> Delete<U>(U id)
    {
        var toDelete = await GetById(id);
        if (toDelete is null)
            return null;

        _dbContext.Set<T>().Remove(toDelete);
        await _dbContext.SaveChangesAsync();

        return toDelete;
    }
}
