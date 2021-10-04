using System.Collections.Generic;
using MediatR;

namespace Ordering.Applications.Features.Orders.Queries.GetOrdersList
{
    public class GetOrderListQuery : IRequest<List<OrdersVm>>
    {
        public string UserName { get; set; }

        public GetOrderListQuery(string userName)
        {
            UserName = userName;
        }
    }
}
