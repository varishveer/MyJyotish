using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public int memberId { get; set; }

        public string[] Problem { get; set; }

        [DataType(DataType.Html)]
        public string Solution { get; set; }

    }
}
