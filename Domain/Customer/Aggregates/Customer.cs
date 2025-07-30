using Domain.Abstractions;

namespace Domain.Customer.Aggregates;

public class Customer: AggregateRoot, IName, IIntId
{
    private Customer(){}
    public Customer(string name)
    {
        Name = name;
    }

    public string Name { get; set; } = null!;
    public int Id { get; set; }
}