﻿using ModelAccessLayer.Models;
using ModelAccessLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccessLayer.Abstraction
{
    public interface IPendingJyotishServices
    {

        public  Task<string> UploadDocumentAsync(DocumentViewModel model);
        public DocumentModel Documents(int Id);
        public Task<dynamic> ProfileData(int Id);

        public Task<JyotishTempRecord> ProfileTempData(int Id);
        public string UploadProfileImage(UploadProfileImagePJViewModel model, string? path);
        public string UpdateProfile(JyotishTempViewModel model, string? path);
        public string Role(string Email);
        public string ProfileImage(string Email);
        public string AddSlotBooking(SlotBookingAddViewModel model);
        public List<SlotListViewModel> SlotList();
        public SlotBookingDetailViewModel JyotishSlotDetails(int id);

        public  Task<string> UploadRejectedDocumentAsync(DocumentViewModel model);
       
        public LayoutDataViewModel LayoutData(int Id);
        public string RejectedMessage(int Id);
        public dynamic InterviewMeetingListByJyotishId(int jytotishId);

        public bool AddConfirmation(int JyotishId);

    }
}
