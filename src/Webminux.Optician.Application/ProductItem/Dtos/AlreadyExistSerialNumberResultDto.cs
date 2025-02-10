using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.ProductItem.Dtos
{
    public class AlreadyExistSerialNumberResultDto
    {
        public AlreadyExistSerialNumberResultDto(List<string> serialNumbers)
        {
            SerialNumbers = serialNumbers;
        }

        public List<string> SerialNumbers { get; set; }
    }
}
