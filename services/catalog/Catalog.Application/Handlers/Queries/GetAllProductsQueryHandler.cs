using Catalog.Application.GrpcServices;
using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Interfaces;
using Catalog.Core.Specifications;
using Catalog.Core.Specifications.Products;
using FluentResults;
using MediatR;

namespace Catalog.Application.Handlers.Queries;

public sealed class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, Result<Pagination<ProductResponseDto>>>
{
    private readonly IProductRepository _repository;
    private readonly ProductMapper _mapper;
    private readonly DiscountGrpcService _discountGrpcService;

    public GetAllProductsQueryHandler(IProductRepository repository, ProductMapper mapper, DiscountGrpcService discountGrpcService)
    {
        _repository = repository;
        _mapper = mapper;
        _discountGrpcService = discountGrpcService;
    }

    public async Task<Result<Pagination<ProductResponseDto>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var spec = new ProductSpecification(request.SpecParams);

        var paginatedProducts = await _repository.GetProductsAsync(spec, cancellationToken);

        var response = _mapper.ToResponseListDto(paginatedProducts.Data).ToList();

        // Enrich each product with discount info from Discount gRPC service
        foreach (var product in response)
        {
            var coupon = await _discountGrpcService.GetDiscount(product.Name);
            if (coupon != null && coupon.Amount > 0)
            {
                product.DiscountAmount = coupon.Amount;
                product.DiscountedPrice = product.Price - coupon.Amount;
            }
            else
            {
                product.DiscountedPrice = product.Price;
            }
        }

        var pagination = new Pagination<ProductResponseDto>(
            paginatedProducts.PageIndex,
            paginatedProducts.PageSize,
            paginatedProducts.TotalCount,
            response
        );

        return Result.Ok(pagination);
    }
}
