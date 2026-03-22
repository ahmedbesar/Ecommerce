using MediatR;
using FluentResults;
using Ordering.Application.Responses;
using System.Collections.Generic;

namespace Ordering.Application.Queries
{
    public class GetOrdersByUserNameQuery : IRequest<Result<IEnumerable<OrderResponseDto>>>
    {
        public string UserName { get; set; }

        public GetOrdersByUserNameQuery(string userName)
        {
            UserName = userName;
        }
    }
}
