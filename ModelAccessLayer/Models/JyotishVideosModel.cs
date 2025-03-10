﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class JyotishVideosModel
    {
        [Key]
        public int Id { get; set; }
        public string VideoTitle { get; set; }
        public string VideoUrl { get; set; }
        public string? ImageUrl { get; set; }
        public string SerialNo { get; set; }
        public int JyotishId { get; set; }
        public bool Status { get; set; }
        public JyotishModel Jyotish { get; set; }
    }
}
