using Basket.Api.Extensions;
using Basket.Application.Commands;
using Basket.Application.Mappers;
using Basket.Application.Queries;
using Basket.Core.Entities;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Basket.Api.Controllers;

public class BasketController : BaseApiController
{
    private readonly IMediator _mediator;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly BasketMapper _mapper;
    private readonly ILogger<BasketController> _logger;

    public BasketController(IMediator mediator, IPublishEndpoint publishEndpoint, BasketMapper mapper, ILogger<BasketController> logger)
    {
        _mediator = mediator;
        _publishEndpoint = publishEndpoint;
        _mapper = mapper;
        _logger = logger;
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

    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
    {
        //get basket by user name
        var query = new GetBasketQuery { UserName = basketCheckout.UserName };
        var basketResult = await _mediator.Send(query);

        if (basketResult.IsFailed || basketResult.Value == null)
        {
            return BadRequest();
        }

        var basket = basketResult.Value;

        var eventMsg = _mapper.Map(basketCheckout);
        eventMsg.TotalPrice = basketCheckout.TotalPrice;
        await _publishEndpoint.Publish(eventMsg);

        _logger.LogInformation($"Basket Published for {basket.UserName}");
        
        //remove from basket
        var deletedcmd = new DeleteBasketCommand { UserName = basketCheckout.UserName };
        await _mediator.Send(deletedcmd);
        return Accepted();
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
