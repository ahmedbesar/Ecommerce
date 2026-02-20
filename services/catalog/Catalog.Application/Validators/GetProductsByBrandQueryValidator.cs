using Catalog.Application.Queries;
using FluentValidation;

namespace Catalog.Application.Validators;

public class GetProductsByBrandQueryValidator : AbstractValidator<GetProductsByBrandQuery>
{
    public GetProductsByBrandQueryValidator()
    {
        RuleFor(x => x.BrandName)
            .NotEmpty().WithMessage("BrandName is required");
    }
}
