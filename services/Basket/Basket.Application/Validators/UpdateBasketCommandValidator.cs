using Basket.Application.Commands;
using Basket.Application.Constants;
using FluentValidation;

namespace Basket.Application.Validators;

public class UpdateBasketCommandValidator : AbstractValidator<UpdateBasketCommand>
{
    public UpdateBasketCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("UserName is required")
            .MaximumLength(ValidationConstants.UserNameMaxLength).WithMessage($"UserName must not exceed {ValidationConstants.UserNameMaxLength} characters");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("At least one item is required in the basket");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.ProductId)
                .NotEmpty().WithMessage("ProductId is required");

            item.RuleFor(i => i.ProductName)
                .NotEmpty().WithMessage("ProductName is required")
                .MaximumLength(ValidationConstants.ProductNameMaxLength).WithMessage($"ProductName must not exceed {ValidationConstants.ProductNameMaxLength} characters");

            item.RuleFor(i => i.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0");

            item.RuleFor(i => i.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0");
        });
    }
}
