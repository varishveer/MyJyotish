﻿using Microsoft.AspNetCore.Http;
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
        [AllowNull]
        public int? EId { get; set; }
        public bool appstatus { get; set; }
        public bool status { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string email { get; set; }
    }

    public class JyotishRatingViewModel
    {
        public int? Id { get; set; }
        public string FeedbackMessage { get; set; }
        public int Stars { get; set; }
        public DateTime? DateTime { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public int JyotishId { get; set; }
        public string? JyotishName { get; set; }
        public bool? Status { get; set; }
    }

    public class JyotishDashboardDataViewModel
    {
        public int TotalClient { get; set; }
        public int TodayAppointment { get; set; }
        public int UpcommingAppointment { get; set; }
        public int TotalChatTime { get; set; }
        public int TotalCallTime { get; set; }
        public int TotalPooja { get; set; }
        public int TotalTeamMember { get; set; }
        public int TotalRating { get; set; }

    }

    public class DepartmentViewModel
    {
        public string DepartmentName { get; set; }
    }
    public class LevelsViewModel
    {
        public string LevelsName { get; set; }
        public string? Description { get; set; }
    }
    public class EmployeesViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long mobile { get; set; }
        public string gender { get; set; }
        public string Email { get; set; }

        public string DateOfBirth { get; set; }
        public int Department { get; set; }
        public int levels { get; set; }
        public DateTime AddingDate { get; set; }
        public bool status { get; set; }
    } 
    public class EmployeesDocsViewModel
    {
        public int Id { get; set; }
        public int employees { get; set; }
       public string name { get; set; }
        public IFormFile IdProof { get; set; }
        public IFormFile metrics { get; set; }
        public IFormFile postmetrics { get; set; }
        public IFormFile degrees { get; set; }
       public string url { get; set; }
        public string email { get; set; }

        public bool status { get; set; }
    }
    public class EmployeesAccessPagesViewModel
    {
        public int Id { get; set; }
        public string pagesName { get; set; }
        public string pageUrl { get; set; }
        public bool status { get; set; }
    }

    public class AccessPageValidation
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public int PageId { get; set; }
        public int levelId { get; set; }
        public bool status { get; set; }

       
    }
    public class RedeemDiscountValidationViewModel
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public int levelId { get; set; }
        public float discount { get; set; }
        public bool status { get; set; }


    }
    public class InterviewMeetingViewModel
    {
        public int? Id { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public int JyotishId { get; set; }
        public string? JyotishName { get; set; }
        public bool? ApproveStatus { get; set; }
        public bool? Status { get; set; }
    }

    public class EmployeeInterviewFeedbackViewModel
    {
        public int? Id { get; set; }
        public string Message { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public int? JyotishId { get; set;  }
        public string? JyotishName { get; set; }
        public int SlotBookingId { get; set; }
        public int Grade { get; set; }
        [AllowNull]
        public string? startDate { get; set; }
        [AllowNull]

        public string? endDate { get; set; }
        public string? VideoUrl { get; set; }
        public IFormFile? Video { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? Image { get; set; }
       
    }

    public class UserServiceRecordViewModel
    {
       
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public TimeOnly TimeOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public int UserId { get; set; }
        public int JyotishId { get; set; }
        public int Action { get; set; }
    }

    public class RedeemCodeRequestViewModel
    {
        public int Id { get; set; }
        public int jyotishId { get; set; }
        public int planId { get; set; }
        public bool status { get; set; }

    }

    public class SliderImagesViewModel
    {
    
        public IFormFile HomePage { get; set; }
        public int SerialNo { get; set; }


    }

    public class KundaliMatchingViewModel
    {

        public int? Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public TimeOnly TimeOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
       
        public string Gender { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public float Timezone { get; set; }
    }
    public class PoojaRecordViewModel
    {
       
        public int Id { get; set; }

        public int PoojaType { get; set; }
        public int jyotishId { get; set; }
        public string title { get; set; }
        public string Procedure { get; set; }
        public string Benefits { get; set; }
        public string AboutGod { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile Image { get; set; }
        public bool status { get; set; }

      

    }

    public class BookedPoojaViewModel
    {
        public int Id { get; set; }
        public int PoojaId { get; set; }
        public int userId { get; set; }
        public DateTime BookingDate { get; set; }
        public bool status { get; set; }
    }

    public class AppointmentBookmarkViewModal
    {
        public int? Id { get; set; }
        [AllowNull]
        public DateOnly? EndDate { get; set; }
        public string Reason { get; set; }
        public int? AppointmentId { get; set; }
        public int? JyotishId { get; set; }
    }
	public class JyotishPoojaViewModel
	{
		public int Id { get; set; }
		public int JyotishId { get; set; }
		public int poojaType { get; set; }
		public int amount { get; set; }
		public DateTime date { get; set; }
		public bool status { get; set; }
		
	}
}
