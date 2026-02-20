using Catalog.Application.Commands;
using Catalog.Application.Constants;
using FluentValidation;

namespace Catalog.Application.Validators;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(ValidationConstants.NameMaxLength).WithMessage($"Name must not exceed {ValidationConstants.NameMaxLength} characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(ValidationConstants.DescriptionMaxLength).WithMessage($"Description must not exceed {ValidationConstants.DescriptionMaxLength} characters");

        RuleFor(x => x.Summary)
            .NotEmpty().WithMessage("Summary is required")
            .MaximumLength(ValidationConstants.SummaryMaxLength).WithMessage($"Summary must not exceed {ValidationConstants.SummaryMaxLength} characters");

        RuleFor(x => x.ImageFile)
            .NotEmpty().WithMessage("ImageFile is required")
            .MaximumLength(ValidationConstants.ImageFileMaxLength).WithMessage($"ImageFile must not exceed {ValidationConstants.ImageFileMaxLength} characters");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");

        RuleFor(x => x.BrandId)
            .NotEmpty().WithMessage("BrandId is required");

        RuleFor(x => x.TypeId)
            .NotEmpty().WithMessage("TypeId is required");
    }
}
