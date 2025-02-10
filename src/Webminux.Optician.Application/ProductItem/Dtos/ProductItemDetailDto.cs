using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Webminux.Optician.Application.Customers.Dtos;

namespace Webminux.Optician.Application.ProductItem.Dtos
{
    /// <summary>
    /// This is a DTO class for Group Detail.
    /// </summary>
    public class ProducItemDetailDto : EntityDto
    {
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public bool IsSold { get; set; } = false;

        public int RecieverEmployee { get; set; }
        public int ReceiverEmployeeDate { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}