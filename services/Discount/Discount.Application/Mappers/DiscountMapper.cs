using Discount.Core.Entities;
using Discount.Grpc.Protos;
using Riok.Mapperly.Abstractions;

namespace Discount.Application.Mappers
{
    [Mapper]
    public partial class DiscountMapper
    {
        public partial CouponModel ToModel(Coupon coupon);
        public partial Coupon ToEntity(CouponModel model);
    }
}
