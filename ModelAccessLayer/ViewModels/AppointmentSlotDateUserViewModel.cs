using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class AppointmentSlotDateUserViewModel
    {
        public int Id { get; set; }
        public int TimeDuration { get; set; }
        public TimeOnly TimeFrom { get; set; }
        public TimeOnly TimeTo { get; set; }
        public int JyotishId { get; set; }
        public string Status { get; set; }
    }
}
