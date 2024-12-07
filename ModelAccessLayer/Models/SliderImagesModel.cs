﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class SliderImagesModel
    {
        [Key]
        public int Id { get; set; }
       
        public string HomePage { get; set; }
        public int SerialNo { get; set; }
        public bool Status { get;set; }

    }
}
