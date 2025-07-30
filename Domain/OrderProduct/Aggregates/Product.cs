using Domain.Abstractions;

namespace Domain.OrderProduct.Aggregates;

public class Product: Entity, IName, IIntId
{
    private Product(){}
    public Product(string name, int quantity, double price)
    {
        Name = name;
        Quantity = quantity;
        Price = price;
    }
    
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Quantity { get; private set; }
    public double Price { get; private set; }

    public void ReduceQuantity(int quantity)
    {
        Quantity -= quantity;
    }

}