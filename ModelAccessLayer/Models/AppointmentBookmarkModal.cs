using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class AppointmentBookmarkModal
    {
        public int Id { get; set; }
        [AllowNull]
        public DateOnly? EndDate { get; set; }
        public string Reason { get; set; }
        public int? AppointmentId { get; set; }
        public int? JyotishId { get; set; }

        public JyotishModel Jyotish { get; set; }
        public AppointmentModel Appointment { get; set; }
    }
}
