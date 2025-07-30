using Domain.Abstractions;
using Domain.OrderProduct.Events;

namespace Domain.OrderProduct.Aggregates;

public class Order: AggregateRoot, IName, IGuidId
{
    private Order() {}
    public Order(string name, int? customerId)
    {
        Id = Guid.NewGuid();
        Name = name;
        CustomerId = customerId;
    }
    
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public int? CustomerId { get; private set; }
    public Customer.Aggregates.Customer? Customer { get; private set; }
    public IList<OrderItem> OrderItems { get; private set; } = new List<OrderItem>();

    public void AddOrderItem(Guid orderId, int quantity, double price, int productId)
    {
        OrderItems.Add(new OrderItem(orderId, quantity, price, productId));
        Raise(new AdjustProductQtyAfterCompleteOrderDomainEvent(productId, quantity));
    }

}