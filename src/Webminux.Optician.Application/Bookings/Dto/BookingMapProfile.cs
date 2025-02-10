using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webminux.Optician.Core.Helpers;

namespace Webminux.Optician.Bookings.Dto
{
    /// <summary>
    /// Provide Mapping profiles for Booking DTOs and Booking 
    /// </summary>
    public class BookingMapProfile : Profile
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public BookingMapProfile()
        {
            CreateMap<CreateBookingDto, Booking>()
                .ForMember(b => b.ActivityId, options => options.Ignore())
                .ForMember(b => b.BookingEmployees, options => options.Ignore());

            CreateMap<BookingEmployee, BookingEmployeeDto>()
                .ForMember(e => e.Status, options => options.MapFrom(e => e.Status.GetEnumValueAsString()))
                .ForMember(e => e.EmployeeName, options => options.MapFrom(e => e.Employee.FullName));

            CreateMap<Booking, BookingDto>()
                .ForMember(b => b.BookingStatus, options => options.MapFrom(b => b.BookingStatus.GetEnumValueAsString()));

        }
    }
}
