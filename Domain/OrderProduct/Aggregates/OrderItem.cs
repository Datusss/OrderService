using Domain.Abstractions;

namespace Domain.OrderProduct.Aggregates;

public class OrderItem: Entity<Guid>
{
    private OrderItem(){}
    public OrderItem(Guid id, Guid orderId, int quantity, double price, Product product)
        :base(id)
    {
        OrderId = orderId;
        Quantity = quantity;
        Price = price;
        Product = product;
    }

    public int ProductId { get; private set; }
    public Product Product { get; private set; } = null!;
    public Guid OrderId { get; private set; }
    public int Quantity { get; private set; }
    public double Price { get; private set; }
}