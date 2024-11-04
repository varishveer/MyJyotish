using Microsoft.AspNetCore.Http;
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
        /*-----------Basic Information---------------------*/
        public bool BasicSection { get; set; }
        public int Id { get; set; }
        public string? Name { get; set; }
        public string Email { get; set; }
        public string? Mobile { get; set; }
        public string? AlternateMobile { get; set; }
        public string? Gender { get; set; }
        public string? Language { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Image { get; set; }

        /*-----------Address---------------------*/
        public bool AddressSection { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
        public int? pincode { get; set; }

        /*-----------Availbility---------------------*/
        public bool AvailbilitySection { get; set; }
        public bool? Pooja { get; set; }

        public bool? Call { get; set; }
        public int? CallCharges { get; set; }
        public bool? Chat { get; set; }
        public int? ChatCharges { get; set; }
        public TimeOnly? TimeTo { get; set; }
        public TimeOnly? TimeFrom { get; set; }
       

        /*----------------About Section----------------------*/
        public bool AboutSection { get; set; }
        public string? About { get; set; }
        public string? AwordsAndAchievement { get; set; }
        public string? Specialization { get; set; }
        public string? Expertise { get; set; }
        public int? Experience { get; set; }







   


    }
}
