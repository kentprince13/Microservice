using System.Collections.Generic;
using System.Threading.Tasks;
using Ordering.Domain.Entitties;

namespace Ordering.Application.Contracts.Persistence
{
    public interface IOrderRepository : IAsyncRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByUserName(string userName);
    }
}