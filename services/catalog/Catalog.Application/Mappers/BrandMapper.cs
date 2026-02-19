using Riok.Mapperly.Abstractions;
using Catalog.Core.Entities;
using Catalog.Application.Responses;

namespace Catalog.Application.Mappers
{
    [Mapper]
    public partial class BrandMapper
    {
        public partial ProductBrandResponseDto ToResponseDto(ProductBrand brand);
        public partial IEnumerable<ProductBrandResponseDto> ToResponseListDto(IEnumerable<ProductBrand> brands);
    }
}
