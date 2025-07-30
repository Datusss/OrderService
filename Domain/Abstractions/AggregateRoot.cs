using MediatR;

namespace Domain.Abstractions;

public abstract class AggregateRoot : Entity
{
    public readonly IList<INotification> DomainEvents = new List<INotification>();
    
    protected void Raise<T>(T domainEvent) where T : INotification
    {
        DomainEvents.Add(domainEvent);
    }
}