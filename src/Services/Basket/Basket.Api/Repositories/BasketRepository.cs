using System;
using System.Threading.Tasks;
using Basket.Api.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.Api.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _distributedCache;

        public BasketRepository(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
        }
        public async Task<ShoppingCart> GetBasket(string userName)
        {
            var basket = await _distributedCache.GetStringAsync(userName);
            
            return string.IsNullOrEmpty(basket) ? null : JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            await _distributedCache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));

            return await GetBasket(basket.UserName);
        }

        public async Task<bool> DeleteBasket(string userName)
        {
            var basket =  await _distributedCache.GetStringAsync(userName);
            if (string.IsNullOrEmpty(basket))
                return false;
            await _distributedCache.RemoveAsync(userName);
            return true;
        }
    }
}