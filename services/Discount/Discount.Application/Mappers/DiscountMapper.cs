using Discount.Application.Responses;
using Discount.Core.Entities;
using Riok.Mapperly.Abstractions;

namespace Discount.Application.Mappers;

[Mapper]
public partial class DiscountMapper
{
    public partial CouponResponseDto ToDto(Coupon coupon);
    public partial Coupon ToEntity(CouponResponseDto dto);
}
