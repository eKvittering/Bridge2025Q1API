using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician
{
    /// <summary>
    /// Dto for ActivityArt Model
    /// </summary>
    public class ActivityArtDto : EntityDto
    {
        public virtual string Name { get; set; }
    }
}
