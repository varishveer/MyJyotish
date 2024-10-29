using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class AppointmentSolutionModel
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int JyotishId { get; set; }
        public int AppointmentId { get; set; }
        
        public JyotishModel Jyotish { get; set; }
        
        public UserModel User { get; set; }
        public AppointmentModel Appointment { get; set; }

        public ICollection<ProblemSolutionModel> Solution { get; set; }
    }
}
