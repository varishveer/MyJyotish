using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class JyotishNotificationDataViewModel
    {
        public string Name { get; set; }
        public string BookingDate { get; set; }
        public string AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public int TimeDuration { get;  set; }
    }
}
