using AutoMapper;
using Ordering.Applications.Features.Orders.Commands.CheckOutOrder;
using Ordering.Applications.Features.Orders.Commands.UpdateOrder;
using Ordering.Applications.Features.Orders.Queries.GetOrdersList;
using Ordering.Domains.Entities;

namespace Ordering.Applications.Mappings
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Order, OrdersVm>().ReverseMap();
            CreateMap<Order, CheckOutOrderCommand>().ReverseMap();
            CreateMap<UpdateOrderCommand, Order>().ReverseMap();
        }
    }
}
