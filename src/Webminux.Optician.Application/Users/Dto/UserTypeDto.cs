using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace Webminux.Optician
{
    /// <summary>
    /// Dto for UserType Model
    /// </summary>
  public  class UserTypeDto:EntityDto,ILookupDto<int>
    {
        /// <summary>
        /// Name of UserType
        /// </summary>
        public virtual string Name { get; set; }
    }
}
