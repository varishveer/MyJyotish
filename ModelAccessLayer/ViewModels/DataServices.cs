using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class packages
    {
        public int Id { get; set; }
        public int SubscriptionId { get; set; }
        public int JyotishId { get; set; }
        [AllowNull]
        public string? PlanType { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool Status { get; set; }
    }
}
