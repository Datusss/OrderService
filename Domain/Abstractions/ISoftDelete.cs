namespace Domain.Abstractions;

public interface ISoftDelete: IDeletedDate
{
    public bool IsDeleted { get; set; }
}