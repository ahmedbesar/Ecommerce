using Catalog.Application.Constants;
using Catalog.Application.Queries;
using FluentValidation;

namespace Catalog.Application.Validators;

public class GetProductByIdQueryValidator : AbstractValidator<GetProductByIdQuery>
{
    public GetProductByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required")
            .Matches(ValidationConstants.MongoObjectIdPattern)
            .WithMessage("Id must be a valid MongoDB ObjectId (24 hex characters)");
    }
}
