using Catalog.Application.Commands;
using Catalog.Application.Mappers;
using Catalog.Core.Interfaces;
using MediatR;

namespace Catalog.Application.Handlers.Commands
{
    public sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IProductRepository _repository;
        private readonly ProductMapper _mapper;

        public UpdateProductCommandHandler(IProductRepository repository, ProductMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = _mapper.ToEntity(request);
            return await _repository.UpdateProductAsync(product, cancellationToken);
        }
    }
}
