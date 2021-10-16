using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Applications.Contracts.Persistence;
using Ordering.Applications.Models;
using Ordering.Domains.Entities;

namespace Ordering.Applications.Features.Orders.Commands.CheckOutOrder
{
    public class CheckOutCommandHandler: IRequestHandler<CheckOutOrderCommand,long>
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        private readonly IEmailService _emailService;
        private readonly ILogger<CheckOutCommandHandler> _logger;

        public CheckOutCommandHandler(IMapper mapper, IOrderRepository orderRepository, IEmailService emailService,
            ILogger<CheckOutCommandHandler> logger)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _emailService = emailService;
            _logger = logger;
        }
        public async Task<long> Handle(CheckOutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = _mapper.Map<Order>(request);
            var newOrder = await _orderRepository.AddAsync(orderEntity);

            _logger.LogInformation($"Order {newOrder.Id} is successfully created.");

            await SendEmail(newOrder);

            return newOrder.Id;
        }

        public async Task SendEmail(Order order)
        {
            var email = new Email() { To = "kentprince13@gmail.com", Body = $"Order was created.", Subject = "Order was created" };

            try
            {
                await _emailService.SendEmail(email);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Order {order.Id} failed due to an error with the mail service: {ex.Message}");
            }
        }
    }
}