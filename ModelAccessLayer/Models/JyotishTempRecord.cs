using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public  class JyotishTempRecord
    {
        [Key]
        public int Id { get; set; }

        public string? Mobile { get; set; }
        public string? AlternateMobile { get; set; }
        public string? Name { get; set; }
        public string? Gender { get; set; }
        public string? Language { get; set; }
        public string? Expertise { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? Password { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? ApprovedStatus { get; set; }
        public string? Status { get; set; }
        public string? NewStatus { get; set; }
        public int? Experience { get; set; }
        public string? Pooja { get; set; }
        public bool? Call { get; set; }
        public int? CallCharges { get; set; }
        public bool? Chat { get; set; }
        public int? ChatCharges { get; set; }
        public int? AppointmentCharges { get; set; }
        public string? Address { get; set; }
        public TimeOnly? TimeTo { get; set; }
        public TimeOnly? TimeFrom { get; set; }
        public string? About { get; set; }
        public string? AwordsAndAchievement { get; set; }
        public string? Specialization { get; set; }
    }
}
