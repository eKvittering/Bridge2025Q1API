﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.SynTableData
{
    public interface ISyncTableDataAppService
    {
        Task<bool> InitializeSync();
    }
}
