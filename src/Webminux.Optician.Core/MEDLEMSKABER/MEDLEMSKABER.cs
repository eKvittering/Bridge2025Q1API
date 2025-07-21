using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician
{
    public class MEDLEMSKABER : Entity
    {
        // ID
        public int? ID { get; set; }

        // Bridge Club ID (Foreign Key)
        public int? FKBRIDGEKLUBID { get; set; }

        // Member ID (Foreign Key)
        public int? FKMEDLEMSID { get; set; }

        // Membership Status
        public int? MEDLEMSSTATUS { get; set; }

        // Start Date
        public DateTime? STARTDATO { get; set; }

        // End Date
        public DateTime? SLUTDATO { get; set; }

    }


}
