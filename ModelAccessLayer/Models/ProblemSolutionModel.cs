using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public  class ProblemSolutionModel
    {
        [Key]
        public int Id { get; set; }
        public int AppointmentSolutionId { get; set; }
        public string Problem { get; set; }
        public string Solution { get; set; }
    
        public AppointmentSolutionModel AppointmentSolution { get; set; }
    }
}
