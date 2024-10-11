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
    public class UserUpdateViewModel
    {
        [Required]
        public int Id { get; set; }
   
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
        public string? CurrentAddress { get; set; }
        [AllowNull]
        public int? Country { get; set; }
        [AllowNull]
        public int? State { get; set; }
        [AllowNull]
        public int? City { get; set; }
        [AllowNull]
        public int? Pincode { get; set; }
        [AllowNull]
        public IFormFile? ProfilePictureUrl { get; set; }
     
    }
}
