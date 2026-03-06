using Basket.Application.Responses;
using FluentResults;
using MediatR;

namespace Basket.Application.Queries;

public sealed record GetBasketQuery : IRequest<Result<ShoppingCartResponseDto>>
{
    public string UserName { get; set; } = default!;
}
