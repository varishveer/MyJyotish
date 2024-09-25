using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class SlotModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Time { get; set; }
        public string Date { get; set; }
    }
}
