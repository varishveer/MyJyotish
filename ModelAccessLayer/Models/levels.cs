using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class levels
    {
        public int id { get; set; }
        public string levelsName { get; set; }
        public bool status { get; set; }

        public ICollection<Employees> employees { get; set; } = new List<Employees>();
        public ICollection<levelsAccessValidation> LevelAccess { get; set; } = new List<levelsAccessValidation>();
        public ICollection<RedeemCodeDiscountValidation> redeemDiscount { get; set; } = new List<RedeemCodeDiscountValidation>();

    }
}
