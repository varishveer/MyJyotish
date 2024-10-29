using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class UserWallet
    {
        [Key]
        public int Id { get; set; }
        public int userId { get; set; }
        public long WalletAmount { get; set; }
        public int status { get; set; }
        public UserModel User { get; set; }
    }
}
