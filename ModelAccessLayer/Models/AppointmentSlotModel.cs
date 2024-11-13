using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public  class AppointmentSlotModel
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int TimeDuration { get; set; }
        public TimeOnly TimeFrom { get; set; }
        public TimeOnly TimeTo { get; set; }
        public int? JyotishId { get; set; }
        public string Status { get; set; }
        public int ActiveStatus { get; set; }

        public JyotishModel JyotishData { get; set; }
        public ICollection<AppointmentModel> AppointmentData { get; set; }
    }
}
