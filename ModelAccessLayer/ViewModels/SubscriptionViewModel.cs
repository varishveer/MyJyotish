﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public  class SubscriptionViewModel
    {

            public int? SubscriptionId { get; set; }
            public string Name { get; set; }
            public double OldPrice { get; set; }
            public double NewPrice { get; set; }
            public double Discount { get; set; }
            public double Gst { get; set; }
            public double DiscountAmount { get; set; }
            public string PlanType { get; set; }
            public string description { get; set; }
    }
}
