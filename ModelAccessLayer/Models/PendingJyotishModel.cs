using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class PendingJyotishModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Mobile { get; set; }

        [AllowNull]
        public string? Name { get; set; }
        [AllowNull]
        public string? Gender { get; set; }
        [AllowNull]
        public string? Language { get; set; }
        [AllowNull]
        public string? Expertise { get; set; }
        [AllowNull]
        public string? Email { get; set; }
       
        [AllowNull]
        public string? Country { get; set; }
        [AllowNull]
        public string? State { get; set; }
        [AllowNull]
        public string? City { get; set; }
        [AllowNull]
        public string? Password { get; set; }
        [AllowNull]
        public DateTime? DateOfBirth { get; set; }
        [AllowNull]
        public string? ProfileImageUrl { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
       /* public DocumentModel DocumentModel { get; set; }*/
        [AllowNull]
        public int? Otp { get; set; }
    }
}
