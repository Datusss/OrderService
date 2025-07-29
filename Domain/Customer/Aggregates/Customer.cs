using Domain.Abstractions;

namespace Domain.Customer.Aggregates;

public class Customer: AggregateRoot<int>
{
    public Customer(string name)
    {
        Name = name;
    }

    public string Name { get; private set; }
}