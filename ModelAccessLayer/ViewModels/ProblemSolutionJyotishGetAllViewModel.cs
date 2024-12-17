using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class ProblemSolutionJyotishGetAllViewModel
    {
        [AllowNull]
        public int? Id { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public int AppointmentId {get; set;}

    }
}
