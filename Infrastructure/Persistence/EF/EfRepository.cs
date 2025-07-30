using Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.EF;

public class EfRepository<TEntity>: UnitOfWork, IBaseCommandRepository<TEntity> where TEntity : Entity
{
    protected readonly AppDbContext DbContext;
    protected readonly DbSet<TEntity> DbSet;

    protected EfRepository(AppDbContext dbContext, IMediator mediator)
        :base(dbContext, mediator)
    {
        DbContext = dbContext;
        DbSet = dbContext.Set<TEntity>();
    }
    
    #region Find and Get methods
    public async Task<TEntity?> FindByIdAsync(object id, CancellationToken cancellationToken)
    {
        if (typeof(IIntId).IsAssignableFrom(typeof(TEntity)))
        {
            return await FindByIntIdAsync((int)id, cancellationToken);
        }
        
        return await FindByGuidIdAsync((Guid) id, cancellationToken);
    }
    
    private async Task<TEntity?> FindByIntIdAsync(int id, CancellationToken cancellationToken)
    {
        var query = DbSet.Where(e => ((IIntId)e).Id == id);
        query = PresetGetOrFind(query);

        return await query.FirstOrDefaultAsync(cancellationToken);
    }
    
    private async Task<TEntity?> FindByGuidIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var query = DbSet.Where(e => ((IGuidId)e).Id == id);
        query = PresetGetOrFind(query);

        return await query.FirstOrDefaultAsync(cancellationToken);
    }
    
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
        return entity;
    }
    #endregion
    
    #region Update methods
    public TEntity Update(TEntity entity)
    {
        PresetUpdate(entity);
        DbSet.Entry(entity).State = EntityState.Modified;
        return entity;
    }
    #endregion
    
    #region Delete methods
    public void Delete(TEntity entity)
    {
        if (IsSoftDelete())
        {
            SoftDelete(entity);
        }
        else
        {
            DbSet.Entry(entity).State = EntityState.Deleted;
        }
    }
    #endregion
    
    #region Any methods
    public async Task<bool> AnyByQueryableAsync(IQueryable<TEntity> query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        query = PresetGetOrFind(query);
        return await query.AnyAsync(cancellationToken);
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

    private void SoftDelete(TEntity entity)
    {
        PresetSoftDelete(entity); 
        Update(entity);
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