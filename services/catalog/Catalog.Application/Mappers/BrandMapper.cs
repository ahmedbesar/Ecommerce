using Riok.Mapperly.Abstractions;
using Catalog.Core.Entities;
using Catalog.Application.Responses;

namespace Catalog.Application.Mappers
{
    [Mapper]
    public partial class BrandMapper
    {
        public partial ProductBrandResponseDto ToResponse(ProductBrand brand);
        public partial IEnumerable<ProductBrandResponseDto> ToResponseList(IEnumerable<ProductBrand> brands);
    }
}
