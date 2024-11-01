using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class ProblemSolutionAdminViewModel
    {

        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public int UserId { get; set; }
        public string JyotishName { get; set; }
        public string JyotishEmail { get; set; }
        public int JyotishId { get; set; }
        public int AppointmentId { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public string[] Problem { get; set; }
        public string[] Solution { get; set; }

    }
}
