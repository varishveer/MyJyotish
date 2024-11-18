using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public  class JyotishUserAttachmentModel
    {
        [Key]
        public int Id { get; set; }
        public int JyotishId { get; set; }
        public int UserId { get; set; }
        public int? MemberId { get; set; }
        public int? AppointmentId { get; set; }

        public string Title { get; set; }
        public string Image { get; set; }
        public bool Status { get; set; }
        [JsonIgnore]
        public JyotishModel Jyotish { get; set; }
        public ClientMembers Member { get; set; }
        [JsonIgnore]
        public UserModel User { get; set; }
        public AppointmentModel Appointment { get; set; }


    }
}
