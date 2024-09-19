using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class CallingModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        [Required]
        public  string Type { get; set; }
        
        public int TotalTime {  get; set; }
        
        public TimeOnly StartTime { get; set;  }
        
        public TimeOnly EndTime { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public int UserId { get; set; }
        public UserModel User { get; set; }
        [Required]
        public int JyotishId { get; set; }
        public JyotishModel Jyotish { get; set; }



    }
}
