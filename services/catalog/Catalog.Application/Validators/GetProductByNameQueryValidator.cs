using Catalog.Application.Queries;
using FluentValidation;

namespace Catalog.Application.Validators;

public class GetProductByNameQueryValidator : AbstractValidator<GetProductByNameQuery>
{
    public GetProductByNameQueryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required");
    }
}
