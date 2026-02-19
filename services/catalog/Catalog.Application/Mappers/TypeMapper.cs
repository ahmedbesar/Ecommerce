using Riok.Mapperly.Abstractions;
using Catalog.Core.Entities;
using Catalog.Application.Responses;

namespace Catalog.Application.Mappers
{
    [Mapper]
    public partial class TypeMapper
    {
        public partial ProductTypeResponse ToResponse(ProductType type);
        public partial IEnumerable<ProductTypeResponse> ToResponseList(IEnumerable<ProductType> types);
    }
}
