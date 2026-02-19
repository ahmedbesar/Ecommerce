using Catalog.Application.Commands;
using Catalog.Core.Interfaces;
using MediatR;

namespace Catalog.Application.Handlers.Commands
{
    public sealed class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IProductRepository _repository;

        public DeleteProductCommandHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DeleteProductAsync(request.Id, cancellationToken);
        }
    }
}
