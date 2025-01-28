using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class PrivacyPolicy
    {
        public int Id { get; set; }
        public string? Address { get; set; }
        public long? Mobile { get; set; }
        public string? Email { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? Policy { get; set; }
        public string? AboutUs { get; set; }
        public bool Status { get; set; }
    }
}
