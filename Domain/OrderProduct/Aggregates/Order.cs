using Domain.Abstractions;

namespace Domain.OrderProduct.Aggregates;

public class Order: AggregateRoot<Guid>, IName
{
    private Order() {}
    public Order(Guid id, string name, int? customerId)
        :base(id)
    {
        Name = name;
        CustomerId = customerId;
    }

    public string Name { get; set; } = null!;
    public int? CustomerId { get; private set; }
    public Customer.Aggregates.Customer Customer { get; private set; } = null!;
    public IList<OrderItem> OrderItems { get; private set; } = new List<OrderItem>();
}