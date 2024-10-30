﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class ProblemSolutionViewModel
    {
        [AllowNull]
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public string[] Problem { get; set; }
        public string[] Solution { get; set; }

    }
}
