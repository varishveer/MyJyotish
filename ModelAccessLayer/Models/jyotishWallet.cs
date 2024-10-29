using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class jyotishWallet
    {
        [Key]
        public int Id { get; set; }
        public int jyotishId { get; set; }
        public long WalletAmount { get; set; }
        public int status { get; set; }
        public JyotishModel jyotish { get; set; }
    }
}
