using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class TeamMemberModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        public string ProfilePictureUrl {  get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public int JyotishId {  get; set; }
        [Required]
        public string Role { get; set; }

    }
}
