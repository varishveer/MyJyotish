using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class EmployeeInterviewAttachmentModel
    {

        public int Id { get; set; }
        public int EmployeeIFId { get; set; }
        public string? Image { get; set; }
        public string? Video { get; set; }
        public bool Status { get; set; }
        public EmployeeInterviewFeedbackModel EmployeeIF { get; set; }

    }
}
