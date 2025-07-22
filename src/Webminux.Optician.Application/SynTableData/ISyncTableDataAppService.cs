using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.SynTableData
{
    /// <summary>
    /// This is an application service class for <see cref="SyncTableDataAppService"/> entity.
    /// </summary>
    public interface ISyncTableDataAppService : IApplicationService
    {
        /// <summary>
        /// This is an application service class for Import Data for Clubs and Members.
        /// </summary>
        Task<bool> ImportData();
    }
}
