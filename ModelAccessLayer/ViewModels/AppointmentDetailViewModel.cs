using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class AppointmentDetailViewModel
    {
        public int? Id { get; set; }
        public string UserName { get; set; }
        public string UserMobile { get; set; }
        public string UserEmail { get; set; }
        public int UserId { get; set; }
        public string Problem { get; set; }
        public DateTime Date { get; set; }
        public TimeOnly Time { get; set; }
        public string Status { get; set; }
        public int? Amount { get; set; }
        public int? ArriveStatus { get; set; }
    }
}
