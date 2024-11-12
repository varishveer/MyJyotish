using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class ClientMembers
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public string? Name { get; set; }
        public string? dob { get; set; }
        public string? gender { get; set; }
        public string? relation { get; set; }
        public int status { get; set; }
        [AllowNull]
        public AppointmentModel? appointment { get; set; }
        public ICollection<ProblemSolutionModel> Solution { get; set; }

    }
}
