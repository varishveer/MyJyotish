using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class JyotishWalletHistoryViewmodel
    {
        public int Id { get; set; }
        [AllowNull]
        public string? PaymentId { get; set; }
        public int JId { get; set; }
        public long amount { get; set; }
        [AllowNull]

        public string? PaymentStatus { get; set; }
        [AllowNull]

        public string? date { get; set; }
        [AllowNull]

        public string? PaymentAction { get; set; }
        public int status { get; set; }
    }
}
