using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basket.Api.Entities;
using EventBus.Messages.Events;
using Ordering.Applications.Features.Orders.Commands.CheckOutOrder;


namespace Basket.Api.Profile
{
    public class MappingProfile: AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<BasketCheckout, BasketCheckOutEvent>().ReverseMap();
        }
    }
}
