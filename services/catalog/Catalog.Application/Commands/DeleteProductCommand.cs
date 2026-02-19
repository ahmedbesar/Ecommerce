using MediatR;

namespace Catalog.Application.Commands
{
    public sealed record DeleteProductCommand() : IRequest<bool>
    {
        public string Id { get; set; }
    }
}
