using MediatR;
using FluentResults;
using Ordering.Application.Responses;
using Ordering.Core.Specifications;
using Ordering.Core.Specifications.Orders;

namespace Ordering.Application.Queries
{
    public class GetOrdersQuery : IRequest<Result<Pagination<OrderResponseDto>>>
    {
        public OrderSpecificationParams SpecParams { get; set; }

        public GetOrdersQuery(OrderSpecificationParams specParams)
        {
            SpecParams = specParams;
        }
    }
}
