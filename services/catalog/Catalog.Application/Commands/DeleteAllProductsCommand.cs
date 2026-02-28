using FluentResults;
using MediatR;

namespace Catalog.Application.Commands;

public sealed record DeleteAllProductsCommand() : IRequest<Result>;
