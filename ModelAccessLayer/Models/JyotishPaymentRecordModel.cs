using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class JyotishPaymentRecordModel
    {
        public int Id { get; set; }
        public int JyotishId { get; set; }
        public string JyotishName { get; set; }
        public string Amount { get; set; }

        public DateTime DateTime { get; set; }
        public string Status { get; set; }
        public string? OrderId { get; set; }
        [AllowNull]
        public string? PaymentId { get; set; }
        [AllowNull]
        public string? Signature { get; set; }
    }
}
