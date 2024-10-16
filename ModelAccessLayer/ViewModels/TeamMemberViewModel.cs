using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class TeamMemberViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        public IFormFile ProfilePicture { get; set; }
        [Required]
        public string Email {  get; set; }
        [Required]
        public int Id { get; set; }

    }
}
