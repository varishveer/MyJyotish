using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class JyotishWalletHistoryModel
    {
        [Key]
        public int Id { get; set; }
        public string PaymentId { get; set; }
        public int JId { get; set; }
        public long amount { get; set; }
        public string PaymentStatus { get; set; }
        public string date { get; set; }
        public string PaymentAction { get; set; }
        public int status { get; set; }
        public JyotishModel jyotish { get; set; }

    }
}
