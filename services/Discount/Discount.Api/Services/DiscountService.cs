using Discount.Application.Commands;
using Discount.Application.Queries;
using Discount.Grpc.Protos;
using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Discount.Api.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<DiscountService> _logger;

        public DiscountService(IMediator mediator, ILogger<DiscountService> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var query = new GetDiscountQuery { ProductName = request.ProductName };
            var result = await _mediator.Send(query);

            if (result.IsFailed)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found."));
            }

            _logger.LogInformation("Discount is retrieved for ProductName : {productName}, Amount : {amount}", result.Value.ProductName, result.Value.Amount);
            return result.Value;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var command = new CreateDiscountCommand
            {
                ProductName = request.Coupon.ProductName,
                Description = request.Coupon.Description,
                Amount = request.Coupon.Amount
            };

            var result = await _mediator.Send(command);

            if (result.IsFailed)
            {
                throw new RpcException(new Status(StatusCode.Internal, "Failed to create discount."));
            }

            _logger.LogInformation("Discount is successfully created. ProductName : {ProductName}", result.Value.ProductName);
            return result.Value;
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var command = new UpdateDiscountCommand
            {
                Id = request.Coupon.Id,
                ProductName = request.Coupon.ProductName,
                Description = request.Coupon.Description,
                Amount = request.Coupon.Amount
            };

            var result = await _mediator.Send(command);

            if (result.IsFailed)
            {
                throw new RpcException(new Status(StatusCode.Internal, "Failed to update discount."));
            }

            _logger.LogInformation("Discount is successfully updated. ProductName : {ProductName}", result.Value.ProductName);
            return result.Value;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var command = new DeleteDiscountCommand { ProductName = request.ProductName };
            var result = await _mediator.Send(command);

            if (result.IsFailed)
            {
                throw new RpcException(new Status(StatusCode.Internal, "Failed to delete discount."));
            }

            _logger.LogInformation("Discount is successfully deleted. ProductName : {ProductName}", request.ProductName);
            return new DeleteDiscountResponse { Success = true };
        }
    }
}
