﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class AddAppointmentJyotishModel
    {
        public string Email { get; set; }
        public int JyotishId { get; set; }

        [Required]
        public DateTime DateTime { get; set; }
        [Required]
        public string Problem { get; set; }
    
    }
}
