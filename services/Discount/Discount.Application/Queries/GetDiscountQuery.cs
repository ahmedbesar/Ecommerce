using Discount.Grpc.Protos;
using FluentResults;
using MediatR;

namespace Discount.Application.Queries;

public sealed record GetDiscountQuery : IRequest<Result<CouponModel>>
{
    public string ProductName { get; set; }
}
