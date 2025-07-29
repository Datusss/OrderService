namespace Domain.Abstractions;

public interface IBaseCommandRepository<TEntity, TKey>: IUnitOfWork where TEntity : Entity<TKey> where TKey : struct
{
    #region Find and Get methods
    Task<TEntity?> FindByQueryableAsync(IQueryable<TEntity> query, CancellationToken cancellationToken);
    #endregion
    
    #region Create methods
    Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken);
    #endregion

    #region Update methods
    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    #endregion

    #region Delete methods
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);
    #endregion
}