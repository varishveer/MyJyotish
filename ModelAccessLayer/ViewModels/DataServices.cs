using ModelAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class packages
    {
        public int Id { get; set; }
        public int SubscriptionId { get; set; }
        public int JyotishId { get; set; }
        [AllowNull]
        public string? PlanType { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool Status { get; set; }
    }
    
    public class CountryCodeViewModel
    {
        public int Id { get; set; }
        public int country { get; set; }
        public int countryCode { get; set; }
    }

    public class InterviewFeedbackViewModel
    {
        public int Id { get; set; }
        public int InterviewId { get; set; }
        public int JyotishId { get; set; }
        [AllowNull]
        public string? JyotishName { get; set; }
        
        public string Message { get; set; }

        public bool ApprovedStatus { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; }
    }

    public class redeamCodeViewModel
    {
        public int Id { get; set; }
        public int jyotishId { get; set; }
        public int PlanId { get; set; }
        public string ReadeamCode { get; set; }

        public float discount { get; set; }
        public float discountAmount { get; set; }
        public bool status { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string email { get; set; }
    }
}
