namespace Domain.Abstractions;

public abstract class AggregateRoot<TKey> : Entity<TKey> where TKey : struct
{
    protected AggregateRoot()
    { }

    protected AggregateRoot(TKey id)
        : base(id)
    { }
}