using Basket.Application.Queries;
using Basket.Application.Constants;
using FluentValidation;

namespace Basket.Application.Validators;

public class GetBasketQueryValidator : AbstractValidator<GetBasketQuery>
{
    public GetBasketQueryValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("UserName is required")
            .MaximumLength(ValidationConstants.UserNameMaxLength).WithMessage($"UserName must not exceed {ValidationConstants.UserNameMaxLength} characters");
    }
}
