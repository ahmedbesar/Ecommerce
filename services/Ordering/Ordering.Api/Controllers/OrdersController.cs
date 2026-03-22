using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.Api.Extensions;
using Ordering.Application.Responses;
using Ordering.Application.Commands;
using Ordering.Application.Queries;
using Ordering.Core.Specifications;
using Ordering.Core.Specifications.Orders;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ordering.Api.Controllers
{
    public class OrdersController : ApiController
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> GetOrders([FromQuery] OrderSpecificationParams specParams)
        {
            var query = new GetOrdersQuery(specParams);
            var result = await _mediator.Send(query);
            return result.ToHttpResponse();
        }

        [HttpGet("{userName}", Name = "GetOrdersByUserName")]
        public async Task<ActionResult> GetOrdersByUserName(string userName)
        {
            var query = new GetOrdersByUserNameQuery(userName);
            var result = await _mediator.Send(query);
            return result.ToHttpResponse();
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrder([FromBody] CreateOrderCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ToHttpResponse();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateOrder([FromBody] UpdateOrderCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ToHttpResponse();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            var command = new DeleteOrderCommand(id);
            var result = await _mediator.Send(command);
            return result.ToHttpResponse();
        }
    }
}
