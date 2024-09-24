using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class LanguageModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public String Name { get; set; }


    }
}
