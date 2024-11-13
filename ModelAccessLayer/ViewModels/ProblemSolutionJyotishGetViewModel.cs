using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class ProblemSolutionJyotishGetViewModel
    {

        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        
        public string[] Problem { get; set; }
        public string Solution { get; set; }
    }
}
