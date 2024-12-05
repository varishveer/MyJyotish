using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class InteviewListViewModel
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Mobile { get; set; }

        public string? Email { get; set; }
        public string? Expertise { get; set; }
        public int? Experience { get; set; }
        public string? Language { get; set; }
        public DateOnly? Date { get; set; }
        public TimeOnly? Time { get; set; }
        public int? SlotId { get; set; }
        public int? SlotBookingId { get; set; }
    
    }
}
