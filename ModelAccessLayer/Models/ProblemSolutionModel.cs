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
        public int UserId { get; set; }
        public int JyotishId { get; set; }
        public string? Problem1 { get; set; }
        public string? Solution1 { get; set; }
        public string? Problem2 { get; set; }
        public string? Solution2 { get; set; }
        public string? Problem3 { get; set; }
        public string? Solution3 { get; set; }
        public string? Image1 { get; set; } 
        public string? Image2 { get; set; } 
        public string? Image3 { get; set; } 
        public DateTime Date { get; set; }
        public TimeOnly Time { get; set; }
        public int AppointmentId { get; set; }
        public AppointmentModel Appointment { get; set; }
        public JyotishModel Jyotish { get; set; }
        public UserModel User { get; set; }
    }
}
