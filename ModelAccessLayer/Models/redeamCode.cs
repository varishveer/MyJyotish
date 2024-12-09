using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class redeamCode
    {
        public int Id { get; set; }
        public int jyotishId { get; set; }
        public int PlanId { get; set; }
        public string ReadeamCode { get; set; }
        public float discount { get; set; }
        public float discountAmount { get; set; }
        public int? EId { get; set; }
        public bool appstatus { get; set; }

        public bool status { get; set; }
        public DateTime date { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public JyotishModel jyotish { get; set; }
        public Employees Employee { get; set; }

    }
}
