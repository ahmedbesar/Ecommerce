using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Interfaces;
using MediatR;

namespace Catalog.Application.Handlers.Queries
{
    public sealed class GetAllBrandsQueryHandler : IRequestHandler<GetAllBrandsQuery, IEnumerable<ProductBrandResponseDto>>
    {
        private readonly IBrandRepository _repository;
        private readonly BrandMapper _mapper;
        public GetAllBrandsQueryHandler(IBrandRepository repository, BrandMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ProductBrandResponseDto>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
        {
            var brands = await _repository.GetAllBrandsAsync(cancellationToken);
            var response = _mapper.ToResponseList(brands);
            return response;
        }
    }
}
