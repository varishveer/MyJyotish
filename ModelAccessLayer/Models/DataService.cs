using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class DataService
    {
        public class Advertisementservice
        {
            public int AdId { get; set; }  // The ID for the advertisement
            public string AreaId { get; set; }  // Comma-separated list of area IDs (e.g., "2,3,4")
            public string AdvertisementArea { get; set; }  // The category or type of advertisement (e.g., "Country")
            public DateTime StartDate { get; set; }  // The start date of the advertisement
            public int Id { get; set; }  // Primary ID (could represent a unique identifier for the record)
            public DateTime CreatedDate { get; set; }  // The date when the advertisement was created
            public DateTime EndDate { get; set; }  // The date when the advertisement was created
            public string BannerUrl { get; set; }  // URL of the advertisement's banner image
            public string PlanType { get; set; }
            [AllowNull]
            public string? Name { get; set; }
            [AllowNull]
            public string? Email { get; set; }
            [AllowNull]
            public string? Mobile { get; set; } 
            public int Duration { get; set; } 
            public bool ActiveStatus { get; set; }  // A flag to indicate if the advertisement is active
            public bool appStatus { get; set; }  // A flag to indicate if the advertisement is active
            public bool Status { get; set; }  // Status code for the advertisement (e.g., 1 = active, 0 = inactive)
            public int JyotishId { get; set; }  // Foreign key or identifier for the Jyotish (user/owner)
            public string AreaName { get; set; }  // Name of the area associated with the advertisement (e.g., "Algeria, American Samoa")
        }
    }
}
