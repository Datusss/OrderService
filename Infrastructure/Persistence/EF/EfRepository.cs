using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.EF;

public class EfRepository<TEntity, TKey>: UnitOfWork, IBaseCommandRepository<TEntity, TKey> where TEntity : Entity<TKey> where TKey : struct
{
    protected readonly AppDbContext DbContext;
    protected readonly DbSet<TEntity> DbSet;

    protected EfRepository(AppDbContext dbContext)
        :base(dbContext)
    {
        DbContext = dbContext;
        DbSet = dbContext.Set<TEntity>();
    }
    
    #region Find and Get methods
    public async Task<TEntity?> FindByQueryableAsync(IQueryable<TEntity> query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        query = PresetGetOrFind(query);
        return await query.FirstOrDefaultAsync(cancellationToken);
    }
    #endregion
    
    #region Create methods
    public async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        PresetCreate(entity);
        await DbSet.AddAsync(entity, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }
    #endregion
    
    #region Update methods
    public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        PresetUpdate(entity);
        DbSet.Entry(entity).State = EntityState.Modified;
        await DbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }
    #endregion
    
    #region Delete methods
    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
    {
        if (IsSoftDelete())
        {
            await SoftDeleteAsync(entity, cancellationToken);
        }
        else
        {
            throw new System.NotImplementedException();
        }
    }
    #endregion
    
    
    
    #region Private methods
    private bool IsSoftDelete()
    {
        return typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity));
    }

    private void PresetSoftDelete(TEntity entity)
    {
        ((ISoftDelete) entity).IsDeleted = true;
        ((ISoftDelete) entity).DeletedDate = DateTime.UtcNow;
        
        if (typeof(IName).IsAssignableFrom(typeof(TEntity)))
        {
            ((IName)entity).Name += "{DEL}";
        }
    }

    private async Task SoftDeleteAsync(TEntity entity, CancellationToken cancellationToken)
    {
        PresetSoftDelete(entity);
        await UpdateAsync(entity, cancellationToken);
    }

    private void PresetCreate(TEntity entity)
    {
        if (entity is ICreatedDate entityCreatedDate)
        {
            entityCreatedDate.CreatedDate = DateTime.UtcNow;
        }
    }

    private void PresetUpdate(TEntity entity)
    {
        if (entity is IModifiedDate entityModifiedDate)
        {
            entityModifiedDate.ModifiedDate = DateTime.UtcNow;
        }
    }

    private IQueryable<TEntity> PresetGetOrFind(IQueryable<TEntity> query)
    {
        if (IsSoftDelete())
        {
            query = query.Where(e => !e.IsDeleted);
        }

        return query;
    }
    #endregion
}