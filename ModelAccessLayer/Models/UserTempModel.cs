using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class UserTempModel
    {
        [Key]
        public int Id { get; set; } 
        public string Email { get; set; }
        public string FirstName { get; set; }
        public int? Otp { get; set; }
        public DateTime? StoredTime { get; set; }
    }
}
