using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.EyeTools.Dtos
{
    /// <summary>
    /// Dto to filter eyetools.
    /// </summary>
    public class EyeToolRequestDto
    {
        /// <summary>
        /// The Id of activity the eyetool  .
        /// </summary>
        public virtual int? ActivityId { get; set; }
    }
}
