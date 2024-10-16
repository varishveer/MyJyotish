using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class UpdateAppointmentJyotishViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public int JyotishId { get; set; }

        [Required]
        public DateTime DateTime { get; set; }
        [Required]
        public string Problem { get; set; }
        [AllowNull]
        public string Solution { get; set; }
    }
}
