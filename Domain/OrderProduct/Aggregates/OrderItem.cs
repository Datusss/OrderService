using Domain.Abstractions;

namespace Domain.OrderProduct.Aggregates;

public class OrderItem: Entity<Guid>
{
    public OrderItem(int productId, Guid orderId, int quantity, double price)
    {
        ProductId = productId;
        OrderId = orderId;
        Quantity = quantity;
        Price = price;
    }

    public int ProductId { get; private set; }
    public Guid OrderId { get; private set; }
    public int Quantity { get; private set; }
    public double Price { get; private set; }
}