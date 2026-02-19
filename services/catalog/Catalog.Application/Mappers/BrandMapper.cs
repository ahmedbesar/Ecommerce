using Riok.Mapperly.Abstractions;
using Catalog.Core.Entities;
using Catalog.Application.Responses;

namespace Catalog.Application.Mappers
{
    [Mapper]
    public partial class BrandMapper
    {
        public partial ProductBrandResponse ToResponse(ProductBrand brand);
        public partial IEnumerable<ProductBrandResponse> ToResponseList(IEnumerable<ProductBrand> brands);
    }
}
