using Domain.Abstractions;

namespace Domain.OrderProduct.Aggregates;

public class Product: Entity<int>, IName
{
    private Product(){}
    public Product(string name, int quantity, double price)
    {
        Name = name;
        Quantity = quantity;
        Price = price;
    }

    public string Name { get; set; } = null!;
    public int Quantity { get; private set; }
    public double Price { get; private set; }
}