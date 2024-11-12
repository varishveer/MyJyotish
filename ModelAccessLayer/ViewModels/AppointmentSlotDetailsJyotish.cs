using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class AppointmentSlotDetailsJyotish
    {
       
        public DateOnly dateFrom { get; set; }
        public DateOnly dateTo { get; set; }
        public TimeOnly timeTo { get; set; }
        public TimeOnly timeFrom { get; set; }
        public int timeDuration { get; set; }
       

    }
}
