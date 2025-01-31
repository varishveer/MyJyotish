using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class AdvertisementPackage
    {
        public int Id { get; set; }
        public string Plantype { get; set; }
        public int Duration { get; set; }
        public int Price { get; set; }
        public float Discount { get; set; }
        public float GST { get; set; }
        public float DiscountAmount { get; set; }
        public float FinalPrice { get; set; }
        public int MaxCountry { get; set; }
        public int MaxState { get; set; }
        public int MaxCity { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Status { get; set; }
     
    }
}
