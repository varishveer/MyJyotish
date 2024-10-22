using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public  class AppointmentSlotUserViewModel
    {
        
        public DateTime Date { get; set; }
        public List<AppointmentSlotDateUserViewModel> SlotList { get; set; }
    }
}
