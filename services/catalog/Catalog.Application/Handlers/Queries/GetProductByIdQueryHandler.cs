using Catalog.Application.GrpcServices;
using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Interfaces;
using FluentResults;
using MediatR;

namespace Catalog.Application.Handlers.Queries;

public sealed class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<ProductResponseDto>>
{
    private readonly IProductRepository _repository;
    private readonly ProductMapper _mapper;
    private readonly DiscountGrpcService _discountGrpcService;

    public GetProductByIdQueryHandler(IProductRepository repository, ProductMapper mapper, DiscountGrpcService discountGrpcService)
    {
        _repository = repository;
        _mapper = mapper;
        _discountGrpcService = discountGrpcService;
    }

    public async Task<Result<ProductResponseDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetProductByIdAsync(request.Id, cancellationToken);

        if (product == null)
            return Result.Fail<ProductResponseDto>($"Product with ID '{request.Id}' was not found");

        var response = _mapper.ToResponseDto(product);

        var coupon = await _discountGrpcService.GetDiscount(response.Name);
        if (coupon != null && coupon.Amount > 0)
        {
            response.DiscountAmount = coupon.Amount;
            response.DiscountedPrice = response.Price - coupon.Amount;
        }
        else
        {
            response.DiscountedPrice = response.Price;
        }

        return Result.Ok(response);
    }
}
