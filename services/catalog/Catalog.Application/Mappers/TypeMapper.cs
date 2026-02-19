using Riok.Mapperly.Abstractions;
using Catalog.Core.Entities;
using Catalog.Application.Responses;

namespace Catalog.Application.Mappers
{
    [Mapper]
    public partial class TypeMapper
    {
        public partial ProductTypeResponseDto ToResponseDto(ProductType type);
        public partial IEnumerable<ProductTypeResponseDto> ToResponseListDto(IEnumerable<ProductType> types);
    }
}
