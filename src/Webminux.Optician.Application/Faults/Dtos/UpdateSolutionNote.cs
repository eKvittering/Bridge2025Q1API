﻿using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.Faults.Dtos
{
    public class UpdateSolutionNote:EntityDto
    {
        public string SolutionNote { get; set; }
    }
}
