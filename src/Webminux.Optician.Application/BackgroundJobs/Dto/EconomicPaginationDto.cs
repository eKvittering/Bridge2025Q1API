﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician
{
    public class EconomicPaginationDto
    {
        public int maxPageSizeAllowed { get; set; }
        public int skipPages { get; set; }
        public int pageSize { get; set; }
        public int results { get; set; }
        public int resultsWithoutFilter { get; set; }
        public string firstPage { get; set; }
        public string nextPage { get; set; }
        public string lastPage { get; set; }
    }
}
