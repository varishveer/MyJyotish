﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public  class JyotishRatingModel
    {
        [Key]
        public int Id { get; set; }
        public string FeedbackMessage { get; set; }
        public int Stars { get; set; }
        public DateTime DateTime { get; set; }
        public int UserId { get; set; }
        public int JyotishId { get; set; }
        public bool Status { get; set; }
        public ICollection<UserModel> User { get; set; } = new List<UserModel>();
        public ICollection<JyotishModel> Jyotish { get; set; } = new List<JyotishModel>();

    }
}
