using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class SlotBookingDetailViewModel
    {
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public int TimeDuration { get; set; }

    }
}
