using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class SlotModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public TimeOnly Time { get; set; }
        public int TimeDuration { get; set; }
        [Required]
        public DateOnly Date { get; set; }
        public string Status { get; set; }
        public int ActiveStatus { get; set; }
        [JsonIgnore]
        public ICollection<SlotBookingModel> SlotBookingRecords { get; set; }
        public ICollection<InterviewFeedbackModel> Feedback { get; set; }
    }
}
