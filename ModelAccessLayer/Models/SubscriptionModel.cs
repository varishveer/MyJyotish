using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public  class SubscriptionModel
    {
        [Key]
        public int SubscriptionId { get; set; }
        public string Name { get; set; }
        public double OldPrice { get; set; }
        public double NewPrice { get; set; }
        public double Discount { get; set; }
        public double Gst { get; set; }
        public double DiscountAmount { get; set; }
        public string PlanType { get; set; }    
        public string description { get; set; }
        public bool Status { get; set; }

        public ICollection<ManageSubscriptionModel> ManageSubscription { get; set; } = new List<ManageSubscriptionModel>();
        public ICollection<SubsciptionManagementModel> subscriptionManage { get; set; } = new List<SubsciptionManagementModel>();
		public ICollection<RedeemCodeRequest> redeemRequest { get; set; } = new List<RedeemCodeRequest>();
        

	}
}
