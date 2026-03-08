using Discount.Application.Responses;
using FluentResults;
using MediatR;

namespace Discount.Application.Queries;

public sealed record GetDiscountQuery : IRequest<Result<CouponResponseDto>>
{
    public string ProductName { get; set; }
}
