using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class InterviewFeedbackModel
    {
        public int Id { get; set; }
        public int InterviewId { get; set; }
        public int JyotishId { get; set; }
        public string? Message { get; set; }
        public bool ApprovedStatus { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; }

        public SlotModel Slot { get; set; }
        public JyotishModel Jyotish { get; set; }
    }
}
