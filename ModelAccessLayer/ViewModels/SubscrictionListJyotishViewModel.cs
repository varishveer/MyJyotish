using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class SubscrictionListJyotishViewModel
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
        public bool? Status { get; set; }

        public FeatureList[] Features { get; set; }
    }

    public class FeatureList
    {
        public int FeatureId { get; set; }
        public string Name { get; set; }
        public int ServiceCount { get; set; }
        public bool Status { get; set; }
    }

}
