using Catalog.Application.Responses;
using FluentResults;
using MediatR;

namespace Catalog.Application.Queries;

public sealed record GetAllTypesQuery() : IRequest<Result<IEnumerable<ProductTypeResponseDto>>>;
