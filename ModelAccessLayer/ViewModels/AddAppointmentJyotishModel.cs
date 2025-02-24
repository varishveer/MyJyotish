using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class AddAppointmentJyotishModel
    {
        public string Name { get; set; }

        public string Email { get; set; }
        public string Mobile { get; set; }
        public int JyotishId { get; set; }
        public int SlotId { get; set; }
        public int country { get; set; }
      
        [Required]
        public string Problem { get; set; }
    
    }
}
