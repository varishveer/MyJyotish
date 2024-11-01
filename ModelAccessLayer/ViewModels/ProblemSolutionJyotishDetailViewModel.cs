using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class ProblemSolutionJyotishDetailViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public int AppointmentId { get; set; }
        public string[] Problem { get; set; }
        public string[] Solution { get; set; }
    }
}
