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
        public int UserId { get; set; }
        public int JyotishId { get; set; }
        public int SlotId { get; set; }

        
        [Required]
        public DateTime Date { get; set; }
        public TimeOnly Time { get; set; }
        [Required]
        public string Problem { get; set; }
        
    }
}
