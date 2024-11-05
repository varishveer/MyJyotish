using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public  class TimeStatusViewModel
    {
        public int Id { get; set; }
        public TimeOnly Time { get; set; }    // Assuming Time is a string
        public string Status { get; set; }
    }
}
