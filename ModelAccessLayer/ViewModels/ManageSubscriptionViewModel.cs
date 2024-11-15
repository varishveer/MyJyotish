using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class ManageSubscriptionViewModel
    {
        public class Add
        {
            public int SubscriptionId { get; set; }
            public int FeatureId { get; set; }
            public int ServiceCount { get; set; }

        }
        public class Update
        {
            public int Id { get; set; }
            public int SubscriptionId { get; set; }
            public int FeatureId { get; set; }
            public int ServiceCount { get; set; }
        }

        public class GetAll
        {
            public int Id { get; set; }
            public string SubscriptionName { get; set; }
            public string FeatureName { get; set; }
            public int ServiceCount { get; set; }
        }
    }
}
