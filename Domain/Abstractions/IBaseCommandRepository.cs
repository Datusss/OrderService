namespace Domain.Abstractions;

public interface IBaseCommandRepository<TEntity>: IUnitOfWork where TEntity : Entity
{
    #region Find and Get methods

    Task<TEntity?> FindByIdAsync(object id, CancellationToken cancellationToken);
    Task<TEntity?> FindByQueryableAsync(IQueryable<TEntity> query, CancellationToken cancellationToken);
    #endregion
    
    #region Create methods
    Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken);
    #endregion

    #region Update methods
    TEntity Update(TEntity entity);
    #endregion

    #region Delete methods
    void Delete(TEntity entity);
    #endregion
}