using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class EmployeeInterviewFeedbackModel
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public int? EmployeeId { get; set; }
        public int SlotBookingId { get; set; }
        public int Grade { get; set; }
        public bool Status {get; set;}
        public SlotBookingModel SlotBooking { get; set; }
        public Employees Employee { get; set; }
        public ICollection<EmployeeInterviewAttachmentModel> EmployeeInterviewAttachment { get; set; } = new List<EmployeeInterviewAttachmentModel>();
    }
}
