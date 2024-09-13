using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class AppointmentViewModel
    {
        [Required]
        public string Mode { get; set; }
        [Required]
        public string Name { get; set;  }
        [Required]
        public string Mobile { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        [Required]
        public string Email {  get; set; }
        [Required]
        public string JyotishEmail { get; set; }
        [Required]
        public string Problem { get; set; }
        [Required]
        public int Amount { get; set; }
    }
}
