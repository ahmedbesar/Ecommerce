using MediatR;
using FluentResults;
using Ordering.Application.Responses;
using Ordering.Application.Mappers;
using Ordering.Application.Queries;
using Ordering.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Handlers.Queries
{
    public class GetOrdersByUserNameQueryHandler : IRequestHandler<GetOrdersByUserNameQuery, Result<IEnumerable<OrderResponseDto>>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly OrderMapper _mapper;

        public GetOrdersByUserNameQueryHandler(IOrderRepository orderRepository, OrderMapper mapper)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<IEnumerable<OrderResponseDto>>> Handle(GetOrdersByUserNameQuery request, CancellationToken cancellationToken)
        {
            var orderList = await _orderRepository.GetOrdersByUserName(request.UserName);
            var mappedOrders = _mapper.ToResponseListDtoFallback(orderList);
            return Result.Ok(mappedOrders);
        }
    }
}
