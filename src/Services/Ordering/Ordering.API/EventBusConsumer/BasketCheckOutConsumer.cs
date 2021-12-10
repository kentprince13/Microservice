using System;
using System.Threading.Tasks;
using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Applications.Features.Orders.Commands.CheckOutOrder;

namespace Ordering.API.EventBusConsumer
{
    public class BasketCheckOutConsumer:IConsumer<BasketCheckOutEvent>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<BasketCheckOutConsumer> _logger;

        public BasketCheckOutConsumer(IMediator mediator, IMapper mapper, ILogger<BasketCheckOutConsumer> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Consume(ConsumeContext<BasketCheckOutEvent> context)
        {
            var command = _mapper.Map<CheckOutOrderCommand>(context.Message);
            var result = await _mediator.Send(command);

            _logger.LogInformation("BasketCheckoutEvent consumed successfully. Created Order Id : {newOrderId}", result);
        }
    }
}
