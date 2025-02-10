using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.Orders.Dto
{
    /// <summary>
    /// Mapping Profile for Order
    /// </summary>
    public class OrderMapProfile : Profile
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public OrderMapProfile()
        {
            CreateMap<Order, OrderDto>();
            CreateMap<OrderDto, Order>();
            CreateMap<UpdateOrderDto, Order>()
                .ForMember(order => order.OrderDate, options => options.MapFrom(
                       order => DateTime.ParseExact(order.OrderDate, OpticianConsts.DateFormate, CultureInfo.InvariantCulture)));

            CreateMap<OrderLine, OrderLineListDto>();
            CreateMap<OrderLineListDto, OrderLine>();
            CreateMap<OrderLineDto, OrderLine>()
                .ForMember(orderLine=>orderLine.PromissedDate,options=>options.MapFrom(
                    orderLine=> DateTime.ParseExact(orderLine.PromissedDate, OpticianConsts.DateFormate, CultureInfo.InvariantCulture)));
            CreateMap<OrderLine, OrderLine>();
        }
    }
}
