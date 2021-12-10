using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EventBus.Messages.Events;
using Ordering.Applications.Features.Orders.Commands.CheckOutOrder;

namespace Ordering.API.Mapping
{
    public class OrderingProfile:Profile
    {
        public OrderingProfile()
        {
            CreateMap<BasketCheckOutEvent, CheckOutOrderCommand>().ReverseMap();
        }
    }
}
