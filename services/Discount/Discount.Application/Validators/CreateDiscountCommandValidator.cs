using FluentValidation;

namespace Discount.Application.Validators;

public class CreateDiscountCommandValidator : AbstractValidator<Commands.CreateDiscountCommand>
{
    public CreateDiscountCommandValidator()
    {
        RuleFor(p => p.ProductName)
            .NotEmpty().WithMessage("ProductName is required.")
            .NotNull()
            .MaximumLength(200).WithMessage("ProductName must not exceed 200 characters.");

        RuleFor(p => p.Description)
            .NotEmpty().WithMessage("Description is required.");

        RuleFor(p => p.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");
    }
}
