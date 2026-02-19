using Catalog.Application.Commands;
using Catalog.Application.Mappers;
using Catalog.Application.Responses;
using Catalog.Core.Interfaces;
using MediatR;

namespace Catalog.Application.Handlers.Commands
{
    public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductResponseDto>
    {
        private readonly IProductRepository _repository;
        private readonly ProductMapper _mapper;

        public CreateProductCommandHandler(IProductRepository repository, ProductMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProductResponseDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = _mapper.ToEntity(request);
            var createdProduct = await _repository.CreateProductAsync(product, cancellationToken);
            return _mapper.ToResponseDto(createdProduct);
        }
    }
}
