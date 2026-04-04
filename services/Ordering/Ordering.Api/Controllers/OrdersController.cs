using Common.Authentication.Consts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ordering.Api.Extensions;
using Ordering.Application.Responses;
using Ordering.Application.Commands;
using Ordering.Application.Queries;
using Ordering.Core.Specifications;
using Ordering.Core.Specifications.Orders;
using System.Threading.Tasks;

namespace Ordering.Api.Controllers
{
    [Authorize]
    public class OrdersController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IAuthorizationService _authorizationService;

        public OrdersController(IMediator mediator, IAuthorizationService authorizationService)
        {
            _mediator = mediator;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<ActionResult> GetOrders([FromQuery] OrderSpecificationParams specParams)
        {
            if (!User.IsInRole(RolesConsts.Admin))
                specParams.UserName = User.Identity?.Name;

            var query = new GetOrdersQuery(specParams);
            var result = await _mediator.Send(query);
            return result.ToHttpResponse();
        }

        [HttpGet("{userName}", Name = "GetOrdersByUserName")]
        public async Task<ActionResult> GetOrdersByUserName(string userName)
        {
            if (!(await _authorizationService.AuthorizeAsync(User, userName, AppPoliciesConsts.SelfUserOrAdmin)).Succeeded)
                return Forbid();

            var query = new GetOrdersByUserNameQuery(userName);
            var result = await _mediator.Send(query);
            return result.ToHttpResponse();
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrder([FromBody] CreateOrderCommand command)
        {
            if (!(await _authorizationService.AuthorizeAsync(User, command.UserName, AppPoliciesConsts.SelfUserOrAdmin)).Succeeded)
                return Forbid();

            var result = await _mediator.Send(command);
            return result.ToHttpResponse();
        }

        [Authorize(Policy = AppPoliciesConsts.Admin)]
        [HttpPut]
        public async Task<ActionResult> UpdateOrder([FromBody] UpdateOrderCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ToHttpResponse();
        }

        [Authorize(Policy = AppPoliciesConsts.Admin)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            var command = new DeleteOrderCommand(id);
            var result = await _mediator.Send(command);
            return result.ToHttpResponse();
        }
    }
}
