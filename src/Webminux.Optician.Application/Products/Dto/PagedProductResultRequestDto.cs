using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.Products.Dto
{
    public class PagedProductResultRequestDto : PagedResultRequestDto
    {
        /// <summary>
        /// Wild Card Keyword
        /// </summary>
        public string Keyword { get; set; }
        public int? categoryId { get; set; }
        public int? brandId { get; set; }
        public bool? IsMedicalDevice { get; set; }
        public long? userId { get; set; }
    }
}
