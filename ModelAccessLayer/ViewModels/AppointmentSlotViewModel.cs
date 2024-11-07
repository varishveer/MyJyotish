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
        public DateTime DateTo { get; set; }
        public int saturday { get; set; }
        public int sunday { get; set; }
        public DateTime? skipDate { get; set; }
        public int TimeDuration { get; set; }
        public TimeOnly TimeFrom { get; set; }
        public TimeOnly TimeTo { get; set; }
        public int JyotishId { get; set; }
    }
}
