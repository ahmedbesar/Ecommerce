using System.Security.Claims;
using Basket.Api.Extensions;
using Basket.Application.Commands;
using Basket.Application.Mappers;
using Basket.Application.Queries;
using Basket.Core.Entities;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Basket.Api.Controllers;

[Authorize]
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

    private string? CurrentUserName =>
        User.FindFirst(ClaimTypes.Name)?.Value ?? User.Identity?.Name;

    private IActionResult? ForbidUnlessSelfOrAdmin(string userName)
    {
        if (User.IsInRole("Admin")) return null;
        if (!string.Equals(CurrentUserName, userName, StringComparison.Ordinal))
            return Forbid();
        return null;
    }

    [HttpGet("{userName}")]
    public async Task<ActionResult> GetBasket(string userName)
    {
        if (ForbidUnlessSelfOrAdmin(userName) is { } denied) return (ActionResult)denied;
        var result = await _mediator.Send(new GetBasketQuery { UserName = userName });
        return result.ToHttpResponse();
    }

    [HttpPost]
    public async Task<ActionResult> CreateBasket([FromBody] CreateBasketCommand command)
    {
        if (ForbidUnlessSelfOrAdmin(command.UserName) is { } denied) return (ActionResult)denied;
        var result = await _mediator.Send(command);
        return result.ToHttpResponse();
    }

    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
    {
        if (ForbidUnlessSelfOrAdmin(basketCheckout.UserName) is { } denied) return (ActionResult)denied;

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

        var deletedcmd = new DeleteBasketCommand { UserName = basketCheckout.UserName };
        await _mediator.Send(deletedcmd);
        return Accepted();
    }

    [HttpPut]
    public async Task<ActionResult> UpdateBasket([FromBody] UpdateBasketCommand command)
    {
        if (ForbidUnlessSelfOrAdmin(command.UserName) is { } denied) return (ActionResult)denied;
        var result = await _mediator.Send(command);
        return result.ToHttpResponse();
    }

    [HttpDelete("{userName}")]
    public async Task<ActionResult> DeleteBasket(string userName)
    {
        if (ForbidUnlessSelfOrAdmin(userName) is { } denied) return (ActionResult)denied;
        var result = await _mediator.Send(new DeleteBasketCommand { UserName = userName });
        return result.ToHttpResponse();
    }
}

