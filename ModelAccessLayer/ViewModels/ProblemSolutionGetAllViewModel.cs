using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class ProblemSolutionGetAllViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int JyotishId { get; set; }
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
