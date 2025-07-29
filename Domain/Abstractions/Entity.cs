namespace Domain.Abstractions;

public abstract class Entity<TKey>: ISoftDelete, ICreatedDate, IDeletedDate, IModifiedDate where TKey : struct
{
    protected Entity()
    { }

    protected Entity(TKey id) => Id = id;

    public TKey Id { get; protected set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedDate { get; set; }
    public DateTime DeletedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}