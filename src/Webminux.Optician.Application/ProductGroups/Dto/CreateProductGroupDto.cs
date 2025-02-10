using System;

namespace Webminux.Optician.Products.Dto
{
    public class CreateProductGroupDto
    {
        public int ProductGroupNumber { get; set; }
        public string Name { get; set; }
        public string Domestic { get; set; }
        public string EU { get; set; }
        public string Abroad { get; set; }

    }
}
