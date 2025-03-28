﻿using Abp.Application.Services.Dto;

namespace Webminux.Optician.Products.Dto
{
    /// <summary>
    /// update product dto
    /// </summary>
    public class UpdateProductGroupDto : EntityDto
    {
        public int ProductGroupNumber { get; set; }
        public string Name { get; set; }
        public string Domestic { get; set; }
        public string EU { get; set; }
        public string Abroad { get; set; }


    }
}
