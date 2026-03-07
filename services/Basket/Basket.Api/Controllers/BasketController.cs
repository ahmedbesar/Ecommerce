using Basket.Api.Extensions;
using Basket.Application.Commands;
using Basket.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Controllers;

public class BasketController : BaseApiController
{
    private readonly IMediator _mediator;

    public BasketController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{userName}")]
    public async Task<ActionResult> GetBasket(string userName)
    {
        var result = await _mediator.Send(new GetBasketQuery { UserName = userName });
        return result.ToHttpResponse();
    }

    [HttpPost]
    public async Task<ActionResult> CreateBasket([FromBody] CreateBasketCommand command)
    {
        var result = await _mediator.Send(command);
        return result.ToHttpResponse();
    }

    [HttpPut]
    public async Task<ActionResult> UpdateBasket([FromBody] UpdateBasketCommand command)
    {
        var result = await _mediator.Send(command);
        return result.ToHttpResponse();
    }

    [HttpDelete("{userName}")]
    public async Task<ActionResult> DeleteBasket(string userName)
    {
        var result = await _mediator.Send(new DeleteBasketCommand { UserName = userName });
        return result.ToHttpResponse();
    }
}
