using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class PurchaseAdvertisement
    {
        public int Id { get; set; }
        public string AdvertisementArea { get; set; }
        public int adId { get; set; }
        public int jyotishId { get; set; }
        public string AreaId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string BannerUrl { get; set; }
        public bool activeStatus { get; set; }
        public bool status { get; set; }
        public AdvertisementPackage advertisement { get; set; }
        public JyotishModel jyotish { get; set; }
    }
}
