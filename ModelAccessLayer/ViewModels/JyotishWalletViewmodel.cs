using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class JyotishWalletViewmodel
    {
        public int Id { get; set; }
        public int jyotishId { get; set; }
        public long WalletAmount { get; set; }
        [AllowNull]
        public string? paymentfor { get; set; }
        public int status { get; set; }
    }
}
