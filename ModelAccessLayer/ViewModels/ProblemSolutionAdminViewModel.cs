using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class ProblemSolutionAdminViewModel
    {

        public int? Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string JyotishName { get; set; }
        public string JyotishEmail { get; set; }
        public int JyotishId { get; set; }
        public int UserId { get; set; }
        public int AppointmentId { get; set; }
        public DateTime Date { get; set; }
        public TimeOnly Time { get; set; }
        public string? Problem1 { get; set; }
        public string? Solution1 { get; set; }
        public string? Problem2 { get; set; }
        public string? Solution2 { get; set; }
        public string? Problem3 { get; set; }
        public string? Solution3 { get; set; }
        public string? Image1 { get; set; }
        public string? Image2 { get; set; }
        public string? Image3 { get; set; }
    }
}
