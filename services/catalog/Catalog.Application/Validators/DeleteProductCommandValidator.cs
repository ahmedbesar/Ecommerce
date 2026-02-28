using Catalog.Application.Commands;
using Catalog.Application.Constants;
using FluentValidation;

namespace Catalog.Application.Validators;

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required")
            .Matches(ValidationConstants.MongoObjectIdPattern)
            .WithMessage("Id must be a valid MongoDB ObjectId (24 hex characters)");
    }
}
