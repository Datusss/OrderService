namespace Domain.Abstractions;

public abstract class Entity: ISoftDelete, ICreatedDate, IModifiedDate 
{
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedDate { get; set; }
    public DateTime DeletedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}