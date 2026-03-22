using MediatR;
using FluentResults;
using Ordering.Application.Responses;
using Ordering.Application.Mappers;
using Ordering.Application.Queries;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using Ordering.Core.Specifications;
using Ordering.Core.Specifications.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Handlers.Queries
{
    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, Result<Pagination<OrderResponseDto>>>
    {
        private readonly IAsyncRepository<Order> _orderRepository;
        private readonly OrderMapper _mapper;

        public GetOrdersQueryHandler(IAsyncRepository<Order> orderRepository, OrderMapper mapper)
        {
            _orderRepository = orderRepository ;
            _mapper = mapper ;
        }

        public async Task<Result<Pagination<OrderResponseDto>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var spec = new OrderSpecification(request.SpecParams);
            var orders = await _orderRepository.GetAllAsync(
                predicate: spec.Criteria,
                orderBy: spec.GetOrderBy(),
                includes: spec.Includes,
                disableTracking: true
            );
            
            var totalItems = await _orderRepository.CountAsync(predicate: spec.Criteria);

            var data = _mapper.ToResponseListDto(orders);

            var pagination = new Pagination<OrderResponseDto>(
                request.SpecParams.PageIndex,
                request.SpecParams.PageSize,
                totalItems,
                data);
                
            return Result.Ok(pagination);
        }
    }
}
