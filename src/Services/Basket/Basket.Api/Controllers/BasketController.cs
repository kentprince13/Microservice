using System;
using System.Net;
using System.Threading.Tasks;
using Basket.Api.Entities;
using Basket.Api.Grpc;
using Basket.Api.Repositories;
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

        public BasketController(IBasketRepository basketRepository, ILogger<BasketController> logger, IDiscountGrpcService discountGrpcService)
        {
            _basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
            _logger = logger;
            _discountGrpcService = discountGrpcService;
        }
        
        [HttpGet("{userName}")]
        public  async Task<IActionResult> GetBasket(string userName)
        {
            _logger.LogInformation("Getting Basket");
            var basket = await _basketRepository.GetBasket(userName);
            return Ok(basket?? new ShoppingCart(userName));
        }
        
        [HttpPost]
        public  async Task<IActionResult> UpdateBasket(ShoppingCart shoppingCart)
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
        public  async Task<IActionResult> DeleteBasket(string userName)
        {
            _logger.LogInformation("Deleting Basket");
             var basket = await _basketRepository.DeleteBasket(userName);
             if (!basket)
                 return NotFound();
            return NoContent();
        }
    }
}