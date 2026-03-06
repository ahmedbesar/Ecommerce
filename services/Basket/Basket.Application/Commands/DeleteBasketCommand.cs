using FluentResults;
using MediatR;

namespace Basket.Application.Commands;

public sealed record DeleteBasketCommand : IRequest<Result>
{
    public string UserName { get; set; } = default!;
}
