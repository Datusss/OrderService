namespace Application.UseCases.OrderProduct.DTOs;

public class OrderDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? CustomerName { get; set; }
    public long Total { get; set; }
    public int TotalQty { get; set; }
}