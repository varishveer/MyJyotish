using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public  class AppointmentSlotViewModel
    {
        public int? Id { get; set; }
        public DateTime Date { get; set; }
        public string TimeDuration { get; set; }
        public TimeOnly TimeFrom { get; set; }
        public TimeOnly TimeTo { get; set; }
        public int JyotishId { get; set; }
    }
}
