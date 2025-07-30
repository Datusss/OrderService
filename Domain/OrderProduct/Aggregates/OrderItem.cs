using Domain.Abstractions;

namespace Domain.OrderProduct.Aggregates;

public class OrderItem: Entity, IGuidId
{
    private OrderItem(){}
    public OrderItem(Guid orderId, int quantity, double price, int productId)
    {
        Id = Guid.NewGuid();
        OrderId = orderId;
        Quantity = quantity;
        Price = price;
        ProductId = productId;
    }

    public int ProductId { get; private set; }
    public Product Product { get; private set; } = null!;
    public Guid OrderId { get; private set; }
    public int Quantity { get; private set; }
    public double Price { get; private set; }
    public Guid Id { get; set; }
}