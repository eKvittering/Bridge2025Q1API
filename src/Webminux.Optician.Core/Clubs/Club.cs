using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.Clubs
{
    public class Club
    {
        /// <summary>
        /// Unique identifier for the club (auto-incremented).
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Unique code assigned to the club.
        /// </summary>
        public string ClubCode { get; set; }

        /// <summary>
        /// Name of the club.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Address of the club (optional).
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Path or URL to the club's logo (optional).
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        /// Foreign key reference to the user-specific table or record.
        /// </summary>
        public int UserDbTableNameId { get; set; }
    }

}
