using Domain.Abstractions;

namespace Domain.Customer.Aggregates;

public class Customer: AggregateRoot<int>, IName
{
    public Customer(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}