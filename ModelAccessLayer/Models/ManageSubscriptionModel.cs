using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public  class ManageSubscriptionModel
    {
        [Key]
        public int Id { get; set; }
        public int SubscriptionId { get; set; }
        public int FeatureId { get; set; }
        public SubscriptionModel Subscription { get; set; }
        public SubscriptionFeaturesModel Feature { get; set; }
    }
}
