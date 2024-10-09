using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public  class SubscriptionFeaturesModel
    {
        [Key]
        public int FeatureId { get; set; }
        public string Name { get; set; }
        public ICollection<ManageSubscriptionModel> ManageSubscription { get; set; } = new List<ManageSubscriptionModel>();
    }
}
 