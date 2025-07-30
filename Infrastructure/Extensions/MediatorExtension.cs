using Domain.Abstractions;
using MediatR;

namespace Infrastructure.Extensions;

public static class MediatorExtension
{
    public static async Task PublishDomainEventsAsync(this IMediator mediator, AggregateRoot? aggregateRoot, CancellationToken cancellationToken)
    {
        if (aggregateRoot is null)
        {
            return;
        }
        
        var domainEvents = aggregateRoot.DomainEvents;

        if (domainEvents.Any())
        {
            var tasks = domainEvents.Select(async (domainEvent) =>
            {
                await mediator.Publish(domainEvent, cancellationToken);
            });
            await Task.WhenAll(tasks);
            domainEvents.Clear();
        }
    }
}