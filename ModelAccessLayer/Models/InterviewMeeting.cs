using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public  class InterviewMeeting
    {
        public int Id { get; set; }

        public string Link { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public int EmployeeId { get; set; }
        public int JyotishId { get; set; }
        public bool ApproveStatus { get; set; }
        public bool Status { get; set; }
        public JyotishModel Jyotish { get; set; }
        public Employees Employee { get; set; }


    }
}
