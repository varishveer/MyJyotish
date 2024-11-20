using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        public int? JyotishId { get; set; }
        public int? UserId { get; set; }
        public string SignatureId { get; set; }
        [AllowNull]
        public string? Method { get; set; }
        public string? Message { get; set; }
        public string? Status { get; set; } // "success" or "failed"
     
    }
}
