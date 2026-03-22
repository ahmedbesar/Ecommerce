using FluentValidation;
using Ordering.Application.Commands;
using Ordering.Core.Constants;

namespace Ordering.Application.Validators
{
    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("{Id} is required.")
                .GreaterThan(0).WithMessage("{Id} must be greater than zero.");

            RuleFor(p => p.UserName)
                .NotEmpty().WithMessage("{UserName} is required.")
                .NotNull()
                .MaximumLength(OrderConstants.UserNameMaxLength).WithMessage("{UserName} must not exceed " + OrderConstants.UserNameMaxLength + " characters.");

            RuleFor(p => p.EmailAddress)
                .NotEmpty().WithMessage("{EmailAddress} is required.")
                .EmailAddress().WithMessage("A valid email is required.")
                .MaximumLength(OrderConstants.EmailMaxLength).WithMessage("{EmailAddress} must not exceed " + OrderConstants.EmailMaxLength + " characters.");

            RuleFor(p => p.TotalPrice)
                .NotEmpty().WithMessage("{TotalPrice} is required.")
                .GreaterThan(0).WithMessage("{TotalPrice} should be greater than zero.");
                
            RuleFor(p => p.FirstName)
                .NotEmpty().WithMessage("{FirstName} is required.")
                .MaximumLength(OrderConstants.NameMaxLength).WithMessage("{FirstName} must not exceed " + OrderConstants.NameMaxLength + " characters.");
                
            RuleFor(p => p.LastName)
                .NotEmpty().WithMessage("{LastName} is required.")
                .MaximumLength(OrderConstants.NameMaxLength).WithMessage("{LastName} must not exceed " + OrderConstants.NameMaxLength + " characters.");
        }
    }
}
