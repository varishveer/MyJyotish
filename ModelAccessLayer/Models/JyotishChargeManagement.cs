using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class JyotishChargeManagement
    {
        public int Id { get; set; }
        public int JyotishId { get; set; }
        public int Charge { get; set; }
        public bool status { get; set; }
        public JyotishModel jyotish { get; set; }
    }
}
