using System;
using System.Net;
using System.Threading.Tasks;
using Basket.Api.Entities;
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

        public BasketController(IBasketRepository basketRepository, ILogger<BasketController> logger)
        {
            _basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
            _logger = logger;
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
            var basket = await _basketRepository.UpdateBasket(shoppingCart);
            return Ok(basket);
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