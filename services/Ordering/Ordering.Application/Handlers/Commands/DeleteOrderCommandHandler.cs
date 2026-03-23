using MediatR;
using FluentResults;
using Ordering.Application.Commands;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers.Commands
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, Result>
    {
        private readonly IOrderRepository _orderRepository;

        public DeleteOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Result> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var orderToDelete = await _orderRepository.GetByIdAsync(request.Id);
            if (orderToDelete == null)
            {
                return Result.Fail($"Order ({request.Id}) was not found.");
            }

            await _orderRepository.DeleteAsync(orderToDelete);

            return Result.Ok();
        }
    }
}
