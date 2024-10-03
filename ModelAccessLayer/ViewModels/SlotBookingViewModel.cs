using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class SlotBookingViewModel
    {
    
        [Required]
        public string Time { get; set; }
        [Required]
       
        public DateTime Date { get; set; }
        [Required]
        public int JyotishId { get; set; }
    }
}
