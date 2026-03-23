using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;

namespace Ordering.Application.Consumers;

public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<BasketCheckoutConsumer> _logger;

    public BasketCheckoutConsumer(IMediator mediator, ILogger<BasketCheckoutConsumer> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        var command = new CreateOrderCommand
        {
            UserName = context.Message.UserName,
            TotalPrice = context.Message.TotalPrice,
            FirstName = context.Message.FirstName,
            LastName = context.Message.LastName,
            EmailAddress = context.Message.EmailAddress,
            AddressLine = context.Message.AddressLine,
            Country = context.Message.Country,
            State = context.Message.State,
            ZipCode = context.Message.ZipCode,
            CardName = context.Message.CardName,
            CardNumber = context.Message.CardNumber,
            Expiration = context.Message.Expiration,
            Cvv = context.Message.Cvv,
            PaymentMethod = context.Message.PaymentMethod
        };
        
        var result = await _mediator.Send(command);
        if (result.IsSuccess)
        {
            _logger.LogInformation("BasketCheckoutEvent consumed successfully. Created Order Id: {newOrderId}", result.Value);
        }
        else
        {
            _logger.LogError("BasketCheckoutEvent consumption failed.");
        }
    }
}
