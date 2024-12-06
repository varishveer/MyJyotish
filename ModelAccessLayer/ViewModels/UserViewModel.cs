using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class UserViewModel
    {
        [Required]
        public string? Email { get; set; }
        [AllowNull]
        public string? Mobile { get; set; }
        [AllowNull]
        public string? Name { get; set; }
        [AllowNull]
        public string? Gender { get; set; }
        [AllowNull]
        public string? DoB { get; set; }
        [AllowNull]
        public string? PlaceOfBirth { get; set; }
        [AllowNull]
        public TimeOnly? TimeOfBirth { get; set; }
        [AllowNull]
        public int? Otp { get; set; }

        [AllowNull]
        public string? Password { get; set; }

        [AllowNull]
        public string? Status { get; set; }

        [AllowNull]
        public int? Id { get; set; } 
      
    
    }
}
