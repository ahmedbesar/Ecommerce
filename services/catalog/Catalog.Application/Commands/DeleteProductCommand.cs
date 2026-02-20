using FluentResults;
using MediatR;

namespace Catalog.Application.Commands;

public sealed record DeleteProductCommand() : IRequest<Result>
{
    public string Id { get; set; }
}
