using FluentResults;
using MediatR;

namespace Discount.Application.Commands;

public sealed record DeleteDiscountCommand : IRequest<Result>
{
    public string ProductName { get; set; }
}
