using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class RedeemCodeDiscountValidation
    {
        public int Id { get; set; }
        public int departmentId { get; set; }
        public int levelsId { get; set; }
        public float Discount { get; set; }
        public bool status { get; set; }

        public Department department { get; set; }
        public levels levels { get; set; }
    }
}
