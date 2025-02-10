using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.Companies.Dtos
{
    /// <summary>
    /// Dto to transfer company information
    /// </summary>
    public class CompanyDto
    {
        /// <summary>
        /// Name of company
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Logo of company
        /// </summary>
        public virtual string LogoPath { get; set; }
        
        /// <summary>
        /// Primary color of theme
        /// </summary>
        public virtual string PrimaryColor { get; set; }
        
        /// <summary>
        /// Secondary color of theme.
        /// </summary>
        public virtual string SecondaryColor { get; set; }
    }
}
