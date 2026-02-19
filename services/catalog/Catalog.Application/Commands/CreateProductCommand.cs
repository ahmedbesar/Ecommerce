using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Commands
{
    public sealed record CreateProductCommand() : IRequest<ProductResponseDto>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string ImageFile { get; set; }
        public decimal Price { get; set; }
        public string BrandId { get; set; }
        public string TypeId { get; set; }
    }
}
