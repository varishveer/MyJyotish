﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public  class JyotishUserAttachmentModel
    {
        [Key]
        public int Id { get; set; }
        public int JyotishId { get; set; }
        public int UserId { get; set; }
        public string Image { get; set; }
        public JyotishModel Jyotish { get; set; }

        public UserModel User { get; set; }

    }
}
