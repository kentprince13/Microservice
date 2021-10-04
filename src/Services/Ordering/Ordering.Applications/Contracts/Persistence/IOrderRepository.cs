using System.Collections.Generic;
using System.Threading.Tasks;
using Ordering.Domains.Entities;

namespace Ordering.Applications.Contracts.Persistence
{
    public interface IOrderRepository : IAsyncRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByUserName(string userName);
    }
}