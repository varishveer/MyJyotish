using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class TimezoneModal
    {
        public int Id { get; set; } 
        public string Country { get; set; }
        public string Timezone { get; set; }
        public bool Status { get; set; }

    }
}
