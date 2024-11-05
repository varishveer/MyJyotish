using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class SlotListViewModel
    {
        public DateOnly Date { get; set; }
        public List<TimeStatusViewModel> Times { get; set; }

       
    }
}
