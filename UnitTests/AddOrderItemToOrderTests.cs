using Application.Mappers;
using Application.UseCases.OrderProduct.Commands;
using Application.UseCases.OrderProduct.DTOs;
using AutoMapper;
using Domain.OrderProduct.Aggregates;
using Domain.OrderProduct.Repositories;
using FluentValidation;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace UnitTests;

public class AddOrderItemToOrderTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IOrderCommandRepository> _orderCommandRepository;
    private readonly Mock<IOrderItemCommandRepository> _orderItemCommandRepository;

    public AddOrderItemToOrderTests()
    {
        var logger = NullLoggerFactory.Instance;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MapOrderItemToOrderItemDTO>();
            cfg.AddProfile<MapOrderToOrderDTO>();
        }, logger);
        _mapper = config.CreateMapper();
        
        _orderCommandRepository = new Mock<IOrderCommandRepository>();
        _orderCommandRepository.Setup(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _orderCommandRepository.Setup(u => u.CommitAsync(null, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _orderItemCommandRepository = new Mock<IOrderItemCommandRepository>();
        _orderItemCommandRepository.Setup(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _orderItemCommandRepository.Setup(u => u.CommitAsync(null, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
    }
    
    [Fact]
    public async Task AddOrderItem_ProductQtyLessThanOrEqualZero_ShouldThrowValidationException()
    {
        var method = new AddOrderItemToOrderCommandHandler(_orderCommandRepository.Object, _orderItemCommandRepository.Object, _mapper);
        var exception = await Assert.ThrowsAsync<ValidationException>(async () => await method.Handle(new AddOrderItemToOrderCommand(
            new AddOrderItemToOrderDTO(
                Guid.NewGuid(), new ProductDTO(1, "Pen", 0, 20), 15)), CancellationToken.None));
        
        Assert.Single(exception.Errors);
        Assert.Equal("Product is out of quantity.", exception.Errors.ElementAt(0).ErrorMessage);
    }
    
    [Fact]
    public async Task AddOrderItem_ProductQtyLessThanRequiringQty_ShouldThrowValidationException()
    {
        var method = new AddOrderItemToOrderCommandHandler(_orderCommandRepository.Object, _orderItemCommandRepository.Object, _mapper);
        var exception = await Assert.ThrowsAsync<ValidationException>(async () => await method.Handle(new AddOrderItemToOrderCommand(
            new AddOrderItemToOrderDTO(Guid.NewGuid(), new ProductDTO(1, "Pen", 10, 20), 15)), CancellationToken.None));
        
        Assert.Single(exception.Errors);
        Assert.Equal("Product is out of quantity, Max is 10.", exception.Errors.ElementAt(0).ErrorMessage);
    }
    
    [Fact]
    public async Task AddOrderItem_MapCorrectOrder()
    {
        var method = new AddOrderItemToOrderCommandHandler(_orderCommandRepository.Object, _orderItemCommandRepository.Object, _mapper);
        var order = new Order(Guid.NewGuid().ToString(), 1);
        _orderItemCommandRepository.Setup(r => r.CreateAsync(It.IsAny<OrderItem>(), CancellationToken.None))
            .Returns(Task.FromResult(new OrderItem(order.Id,15, 10000, 1)));
        _orderCommandRepository.Setup(r => r.Update(It.IsAny<Order>()))
            .Returns(order);
        _orderCommandRepository.Setup(r => r.FindByIdAsync(It.IsAny<Guid>(), CancellationToken.None))
            .Returns(Task.FromResult(order)!);
        
        await method.Handle(new AddOrderItemToOrderCommand(
            new AddOrderItemToOrderDTO(
                order.Id, 
                new ProductDTO(1, "Pen", 200, 10000), 15)), CancellationToken.None);
        var orderDTO = await method.Handle(new AddOrderItemToOrderCommand(
            new AddOrderItemToOrderDTO(
                order.Id, 
                new ProductDTO(2, "Book", 100, 20000), 10)), CancellationToken.None);
        
        Assert.Equal(350000, orderDTO.Total);
        Assert.Equal(25, orderDTO.TotalQty);
    }
}