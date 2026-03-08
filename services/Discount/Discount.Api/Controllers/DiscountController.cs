using Discount.Api.Extensions;
using Discount.Application.Commands;
using Discount.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Discount.Api.Controllers;

public class DiscountController : BaseApiController
{
    private readonly IMediator _mediator;

    public DiscountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{productName}", Name = "GetDiscount")]
    public async Task<ActionResult> GetDiscount(string productName)
    {
        var result = await _mediator.Send(new GetDiscountQuery { ProductName = productName });
        return result.ToHttpResponse();
    }

    [HttpPost]
    public async Task<ActionResult> CreateDiscount([FromBody] CreateDiscountCommand command)
    {
        var result = await _mediator.Send(command);
        return result.ToHttpResponse();
    }

    [HttpPut]
    public async Task<ActionResult> UpdateDiscount([FromBody] UpdateDiscountCommand command)
    {
        var result = await _mediator.Send(command);
        return result.ToHttpResponse();
    }

    [HttpDelete("{productName}")]
    public async Task<ActionResult> DeleteDiscount(string productName)
    {
        var result = await _mediator.Send(new DeleteDiscountCommand { ProductName = productName });
        return result.ToHttpResponse();
    }
}
