using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class DocUpdateViewModel
    {
        [Required]
        public string ImageStatus { get; set; }
        [Required]
        public int JyotishId { get; set; }
    }
}
