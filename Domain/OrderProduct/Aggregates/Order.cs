using Domain.Abstractions;

namespace Domain.OrderProduct.Aggregates;

public class Order: AggregateRoot<Guid>
{
    public Order(string name, Guid? customerId)
    {
        Name = name;
        CustomerId = customerId;
    }

    public string Name { get; private set; }
    public Guid? CustomerId { get; private set; }
}