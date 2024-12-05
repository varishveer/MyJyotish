using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class SlotBookingModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int SlotId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public int JyotishId { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public bool status { get; set; }
        public  JyotishModel JyotishRecords { get; set; }
        public SlotModel SlotRecords { get; set; }
        public ICollection<EmployeeInterviewFeedbackModel> EmployeeInterviewFeedbacks { get; set; } = new List<EmployeeInterviewFeedbackModel>(); 

       
    }
}
