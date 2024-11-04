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
        public  DocumentModel Documents(string email);
        public  Task<JyotishModel> Profile(string email);
        public string UpdateProfile(JyotishTempViewModel model, string? path);
        public string Role(string Email);
        public string ProfileImage(string Email);
        public string AddSlotBooking(SlotBookingViewModel model);
        public List<SlotListViewModel> SlotList();
        public SlotBookingModel JyotishSlotDetails(int id);

    }
}
