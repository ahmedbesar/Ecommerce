using Catalog.Application.Commands;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Riok.Mapperly.Abstractions;

namespace Catalog.Application.Mappers
{
    [Mapper]
    public partial class ProductMapper
    {
        public partial ProductResponseDto ToResponseDto(Product product);
        public partial IEnumerable<ProductResponseDto> ToResponseListDto(IEnumerable<Product> products);
        public partial Product ToEntity(CreateProductCommand command);
        public partial Product ToEntity(UpdateProductCommand command);
    }
}
