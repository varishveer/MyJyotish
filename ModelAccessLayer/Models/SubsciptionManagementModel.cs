using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class SubsciptionManagementModel
    {
        public int Id { get; set; }
        public int SubscriptionId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime Name { get; set; }
        public bool Status { get; set; }

    }
}
