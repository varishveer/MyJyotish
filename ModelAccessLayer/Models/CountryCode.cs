﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class CountryCode
    {
        [Key]
        public int Id { get; set; }
        public int country { get; set; }
        public int countryCode { get; set; }

        public Country countryobj { get; set; }
        public ICollection<JyotishModel>? jyotish { get; set; } = new List<JyotishModel>();
        public ICollection<UserModel> User { get; set; } = new List<UserModel>();

    }
}
