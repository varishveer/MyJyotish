using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public  class PoojaRecordModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Category { get; set; }
       
        [Required]
        public int PoojaCategoryId { get; set; } 
        [Required]
        public DateTime DateTime{ get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Headline { get; set; }
        [Required]
        public string Description { get; set; }
        public string Benefits { get; set; }
        public string Procedure { get; set; }
        public string AboutGod { get; set; }
        public string? Reviews { get; set; }

        public PoojaCategoryModel PoojaCategoryModel { get; set; }
    }
}
