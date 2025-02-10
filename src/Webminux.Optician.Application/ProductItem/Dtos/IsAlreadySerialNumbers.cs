using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.ProductItem.Dtos
{
    public class IsAlreadySerialNumbers
    {
        public int? Id { get; set; }
        public List<SerialNumber> SerialNumber { get; set; }
    }
}
