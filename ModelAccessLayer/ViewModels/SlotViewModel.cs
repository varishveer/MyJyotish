using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class SlotViewModel
    {
        [Required] 
        public string Time { get; set; }
        [Required]
        public DateOnly Date { get; set; }
    }
}
