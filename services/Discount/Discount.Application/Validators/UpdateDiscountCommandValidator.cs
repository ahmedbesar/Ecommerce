using FluentValidation;

namespace Discount.Application.Validators;

public class UpdateDiscountCommandValidator : AbstractValidator<Commands.UpdateDiscountCommand>
{
    public UpdateDiscountCommandValidator()
    {
        RuleFor(p => p.Id)
            .GreaterThan(0).WithMessage("Id must be greater than zero.");

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
