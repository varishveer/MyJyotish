using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public  class PaymentCreateOrderViewModel
    {
        [AllowNull]
        public int UserId { get; set; }
        [AllowNull]
        public int JyotishId { get; set; }
        
        public decimal Amount { get; set; }
        [AllowNull]
        public string? Signature { get; set; }
        [AllowNull]
        public string? PaymentId { get; set; }
        [AllowNull]
        public string? OrderId { get; set; }

    }
}
