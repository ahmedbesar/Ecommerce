using Basket.Application.Commands;
using Basket.Application.Constants;
using FluentValidation;

namespace Basket.Application.Validators;

public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
{
    public DeleteBasketCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("UserName is required")
            .MaximumLength(ValidationConstants.UserNameMaxLength).WithMessage($"UserName must not exceed {ValidationConstants.UserNameMaxLength} characters");
    }
}
