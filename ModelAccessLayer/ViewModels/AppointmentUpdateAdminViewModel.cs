using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class AppointmentUpdateAdminViewModel
    {

        public int Id { get; set; }

        public DateTime Date { get; set; }


        public int JyotishId { get; set; }
        public int UserId { get; set; }
        public int SlotId { get; set; }
        public string Problem { get; set; }
        [AllowNull]

        public string? Status { get; set; }
        public int? Amount { get; set; }
    }
}
