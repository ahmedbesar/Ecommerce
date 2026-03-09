using Discount.Grpc.Protos;
using FluentResults;
using MediatR;

namespace Discount.Application.Commands;

public sealed record CreateDiscountCommand : IRequest<Result<CouponModel>>
{
    public string ProductName { get; set; }
    public string Description { get; set; }
    public int Amount { get; set; }
}
