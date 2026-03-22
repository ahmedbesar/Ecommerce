using MediatR;
using FluentResults;
using Ordering.Application.Commands;
using Ordering.Application.Mappers;
using FluentResults;
using Ordering.Application.Exceptions;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Handlers.Commands
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Result>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly OrderMapper _mapper;

        public UpdateOrderCommandHandler(IOrderRepository orderRepository, OrderMapper mapper)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderToUpdate = await _orderRepository.GetByIdAsync(request.Id);
            if (orderToUpdate == null)
            {
                return Result.Fail($"Order ({request.Id}) was not found.");
            }

            _mapper.Map(request, orderToUpdate);

            await _orderRepository.UpdateAsync(orderToUpdate);

            return Result.Ok();
        }
    }
}
