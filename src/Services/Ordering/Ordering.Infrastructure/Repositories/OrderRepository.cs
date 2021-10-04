using System.Collections.Generic;
using System.Threading.Tasks;
using Ordering.Applications.Contracts.Persistence;
using Ordering.Domains.Entities;
using Ordering.Infrastructure.Persistence;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(OrderContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName)
        {
            return await GetAsync(c => c.UserName == userName);
        }

        
    }
}
