using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.Faults.Dtos
{
    /// <summary>
    /// Data Transfer Object to update fault status
    /// </summary>
    public class UpdateFaultStatusDto:EntityDto
    {
        /// <summary>
        /// Integer value of fault status
        /// </summary>
        public virtual int FaultStatus { get; set; }
    }
}
