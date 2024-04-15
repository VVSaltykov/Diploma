using System.Linq.Expressions;
using Diploma.Common.Exceptions;
using Diploma.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Diploma.API.Repositories;

public class BaseCRUDRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    private readonly DbContext dbContext;
    protected readonly DbSet<TEntity> dbSet;

    public BaseCRUDRepository(DbContext dbContext)
    {
        this.dbContext = dbContext;
        dbSet = this.dbContext.Set<TEntity>();
    }

    public async Task Create(TEntity entity)
    {
        await Create(new[] { entity });
    }

    public async Task Create(TEntity[] entity)
    {
        await dbSet.AddRangeAsync(entity);
        await dbContext.SaveChangesAsync();
    }


    public async Task<TEntity[]> Read(Func<TEntity, bool> query = null!, Expression<Func<TEntity, object>> include = null!)
    {
        if (query == null)
            return include is null ? await dbSet.AsNoTracking().ToArrayAsync() : dbSet.AsNoTracking().Include(include).ToArray();
        else
            return include is null ? dbSet.AsNoTracking().Where(query).ToArray() : dbSet.AsNoTracking().Include(include).Where(query).ToArray();
    }

    public async Task<TEntity> ReadFirst(Expression<Func<TEntity, bool>> query)
    {
        return await dbSet.FirstOrDefaultAsync(query);
    }

    public async Task Update(TEntity entity)
    {
        var updatingEntity = await dbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id!.Equals(entity.Id));

        if (updatingEntity == null)
            throw new EntityNotFound(typeof(TEntity));

        dbSet.Update(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task Delete(TKey key)
    {
        var entity = await dbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id!.Equals(key));

        if (entity == null)
            throw new EntityNotFound(typeof(TEntity));

        dbSet.Remove(entity);
        await dbContext.SaveChangesAsync();
    }
}