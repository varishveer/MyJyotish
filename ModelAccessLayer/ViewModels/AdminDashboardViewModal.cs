using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public  class AdminDashboardViewModal
    {
        public int TotalUsers { get; set; }
        public int TotalJyotish { get; set; }
        public int PendingJyotish { get; set; }
        public int RejectedJyotish { get; set; }

    }
}
