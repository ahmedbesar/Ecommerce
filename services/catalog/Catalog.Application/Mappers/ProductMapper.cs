using Riok.Mapperly.Abstractions;
using Catalog.Core.Entities;
using Catalog.Application.Responses;

namespace Catalog.Application.Mappers
{
    [Mapper]
    public partial class ProductMapper
    {
        public partial ProductResponse ToResponse(Product product);
        public partial IEnumerable<ProductResponse> ToResponseList(IEnumerable<Product> products);
    }
}
