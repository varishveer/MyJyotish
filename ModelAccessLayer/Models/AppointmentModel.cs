using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class AppointmentModel
    {
        [Key]
        public int Id { get; set; }
        [AllowNull]
        public string Name { get; set; }
        public string Mobile { get; set; }
        public DateTime DateTime { get; set; }
        [AllowNull]
        public string Email { get; set; }
        public int JyotishId { get; set; } 
        public int UserId { get; set; }
        public string Problem { get; set; }
        [AllowNull]
        public string? Solution { get; set; }
        public string Status { get; set; }
        public int? Amount { get; set; }
    }
}
