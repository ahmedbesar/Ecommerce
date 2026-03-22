using MediatR;
using FluentResults;

namespace Ordering.Application.Commands
{
    public class DeleteOrderCommand : IRequest<Result>
    {
        public int Id { get; set; }

        public DeleteOrderCommand(int id)
        {
            Id = id;
        }
    }
}
