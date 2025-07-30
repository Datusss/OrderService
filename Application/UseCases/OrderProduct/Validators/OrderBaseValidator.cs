using Application.UseCases.OrderProduct.DTOs;
using Domain.OrderProduct.Repositories;
using FluentValidation;

namespace Application.UseCases.OrderProduct.Validators;

public class OrderBaseValidator : AbstractValidator<AddOrderItemToOrderDTO>
{
    public OrderBaseValidator(int requiringQty)
    {
        RuleFor(x => x.Product.Quantity)
            .GreaterThan(0).WithMessage("Product is out of quantity.")
            .DependentRules(() =>
            {
                RuleFor(x => x.Product.Quantity)
                    .GreaterThanOrEqualTo(requiringQty)
                    .WithMessage(x => $"Product is out of quantity, Max is {x.Product.Quantity}.");
            });
    }
}