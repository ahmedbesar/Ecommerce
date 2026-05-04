using Discount.Grpc.Protos;

namespace Catalog.Application.GrpcServices;

public class DiscountGrpcService
{
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;

    public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
    {
        _discountProtoServiceClient = discountProtoServiceClient;
    }

    public async Task<CouponModel?> GetDiscount(string productName)
    {
        try
        {
            var discountRequest = new GetDiscountRequest { ProductName = productName };
            return await _discountProtoServiceClient.GetDiscountAsync(discountRequest);
        }
        catch
        {
            // If discount service is unavailable, return null (no discount)
            return null;
        }
    }
}
