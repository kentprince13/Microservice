using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Basket.Api.Entities;
using Basket.Api.Grpc;
using Basket.Api.Repositories;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Basket.Api.Controllers
{
    [ApiController]
    [Route("api/v1/basket")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly ILogger<BasketController> _logger;
        private readonly IDiscountGrpcService _discountGrpcService;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public BasketController(IBasketRepository basketRepository, ILogger<BasketController> logger,
            IDiscountGrpcService discountGrpcService, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
            _logger = logger;
            _discountGrpcService = discountGrpcService;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }
        
        [HttpGet("{userName}")]
        public async Task<IActionResult> GetBasket(string userName)
        {
            _logger.LogInformation("Getting Basket");
            var basket = await _basketRepository.GetBasket(userName);
            return Ok(basket?? new ShoppingCart(userName));
        }
        
        [HttpPost]
        public async Task<IActionResult> UpdateBasket(ShoppingCart shoppingCart)
        {
            _logger.LogInformation("Posting Basket");
            foreach (var item in shoppingCart.Items)
            {
                var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
                item.Price -= Convert.ToDecimal(coupon.Amount);
            }

            return Ok(await _basketRepository.UpdateBasket(shoppingCart));
        }
        
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [HttpDelete("{userName}")]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            _logger.LogInformation("Deleting Basket");
             var basket = await _basketRepository.DeleteBasket(userName);
             if (!basket)
                 return NotFound();
            return NoContent();
        }

        [HttpPost("checkout")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CheckOut([FromBody] BasketCheckout basketCheckout)
        {
            // get existing basket with total price 
            // Create basketCheckoutEvent -- Set TotalPrice on basketCheckout eventMessage
            // send checkout event to rabbitmq
            // remove the basket

            // // get existing basket with total price
            var basket = await _basketRepository.GetBasket(basketCheckout.UserName);
            if (basket == null)
            {
                return BadRequest();
            }

            // var shoppingCart = new ShoppingCart
            // {
            //     UserName = "lexxy",
            //     Items = new List<ShoppingCartItem>
            //     {
            //         new ShoppingCartItem
            //             { Color = "blue", Price = 140, ProductId = "1", ProductName = "IPhone13", Quantity = 2 }
            //     }
            // };

            // send checkout event to rabbitmq
            var eventMessage = _mapper.Map<BasketCheckOutEvent>(basketCheckout);
            //eventMessage.TotalPrice = shoppingCart.TotalPrice;
            eventMessage.TotalPrice = basket.TotalPrice;
            await _publishEndpoint.Publish(eventMessage);

            // remove the basket
            await _basketRepository.DeleteBasket(basket.UserName);

            return Accepted();
        }

    }
}