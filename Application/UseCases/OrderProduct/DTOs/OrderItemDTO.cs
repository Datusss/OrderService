namespace Application.UseCases.OrderProduct.DTOs;

public record OrderItemDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public int Quantity { get; set; }
    public double Price { get; set; }
}
