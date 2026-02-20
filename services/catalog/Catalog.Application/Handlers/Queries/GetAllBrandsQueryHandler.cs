using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Interfaces;
using FluentResults;
using MediatR;

namespace Catalog.Application.Handlers.Queries;

public sealed class GetAllBrandsQueryHandler : IRequestHandler<GetAllBrandsQuery, Result<IEnumerable<ProductBrandResponseDto>>>
{
    private readonly IBrandRepository _repository;
    private readonly BrandMapper _mapper;

    public GetAllBrandsQueryHandler(IBrandRepository repository, BrandMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<ProductBrandResponseDto>>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
    {
        var brands = await _repository.GetAllBrandsAsync(cancellationToken);
        var response = _mapper.ToResponseListDto(brands);
        return Result.Ok(response);
    }
}
