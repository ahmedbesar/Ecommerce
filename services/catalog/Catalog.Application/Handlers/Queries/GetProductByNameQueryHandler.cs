using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Interfaces;
using MediatR;

namespace Catalog.Application.Handlers.Queries
{
    public sealed class GetProductByNameQueryHandler : IRequestHandler<GetProductByNameQuery, IEnumerable<ProductResponseDto>>
    {
        private readonly IProductRepository _repository;
        private readonly ProductMapper _mapper;

        public GetProductByNameQueryHandler(IProductRepository repository, ProductMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductResponseDto>> Handle(GetProductByNameQuery request, CancellationToken cancellationToken)
        {
            var products = await _repository.GetAllProductsByNameAsync(request.Name, cancellationToken);
            var response = _mapper.ToResponseListDto(products);
            return response;
        }
    }
}
