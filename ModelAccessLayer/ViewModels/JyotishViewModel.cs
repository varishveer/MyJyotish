using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class JyotishViewModel
    {
        
        public int Id {  get; set; } 
        [Required]
        public string Name { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Language { get; set; }
        [Required]
        public string Expertise { get; set; }
        [Required]
        public int Country { get; set; }
        [Required]
        public int State { get; set; }
        [Required]
        public int City { get; set; }

        [AllowNull]
        public string? ProfileImageUrl { get; set; }
        [AllowNull]
        public IFormFile? Image { get; set; }
        [AllowNull]
        public DateTime? DateOfBirth { get; set; }
        




    }
}
