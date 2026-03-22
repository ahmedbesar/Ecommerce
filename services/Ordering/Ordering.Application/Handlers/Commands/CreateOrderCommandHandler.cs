using MediatR;
using FluentResults;
using Ordering.Application.Commands;
using Ordering.Application.Mappers;
using FluentResults;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Handlers.Commands
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<int>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly OrderMapper _mapper;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, OrderMapper mapper)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<int>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = _mapper.ToEntity(request);
            var newOrder = await _orderRepository.AddAsync(orderEntity);
            
            return Result.Ok(newOrder.Id);
        }
    }
}
