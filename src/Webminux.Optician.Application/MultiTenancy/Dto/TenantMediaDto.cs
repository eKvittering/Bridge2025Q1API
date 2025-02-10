using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.MultiTenancy.Dto
{
    public class TenantMediaDto
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public string HomeVideo { get; set; }
        public string HomeImage1 { get; set; }
        public string HomeImage2 { get; set; }
        public string HomeImage3 { get; set; }
        public string HomeImage4 { get; set; }
        public string HomeImage5 { get; set; }
        public string HomeImage6 { get; set; }
        public string HomeImage7 { get; set; }
    }
}
