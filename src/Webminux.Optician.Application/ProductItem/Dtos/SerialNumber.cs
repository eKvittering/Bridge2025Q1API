using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.ProductItem.Dtos
{
    public class SerialNumber
    {
        public int? Id { get; set; }
        public string Value { get; set; }
        public string Code { get; set; }
        public bool IsRepeated { get; set; }
        public string Description { get; set; }
    }
}
