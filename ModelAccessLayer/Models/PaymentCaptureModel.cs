using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class PaymentCaptureModel
    {
        public string PaymentId { get; set; }
        public decimal Amount { get; set; }
        public string OrderId { get; set; }
        public string SignatureId { get; set; }
        public string? Message { get; set; }
        public string? Status { get; set; } // "success" or "failed"
     
    }
}
