﻿using ModelAccessLayer.Models;
using ModelAccessLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccessLayer.Abstraction
{
    public interface IJyotishServices
    {
        public string UpdateProfile(JyotishProfileUpdateViewModal model);
        public List<AppointmentDetailViewModel> GetAllAppointment(int Id);
        public List<AppointmentDetailViewModel> UpcomingAppointment(int Id);
           public string AddAppointment(AddAppointmentJyotishModel model);
        public AppointmentDetailViewModel GetAppointment(int Id);
           public string UpdateAppointment(UpdateAppointmentJyotishViewModel model); 
        public List<TeamMemberModel> TeamMember(int Id);
        public string AddTeamMember(TeamMemberViewModel teamMember, string Path);
   /*     public bool CreateAPooja(PoojaRecordModel model);*/
        public List<Country> CountryList();
        public List<State> StateList(int Id);
        public List<City> CityList(int Id);
        public List<ExpertiseModel> ExpertiseList();
        public List<PoojaListModel> GetPoojaList();
        public List<SpecializationListModel> GetSpecializationList();
        public JyotishDocumentViewModel DashBoard(string email);
        public int countryCode(int country);
        public float purchaseWithReadmCode(string redeamCode, int JyotishId,int planId);
        public dynamic getBookedPoojaList(int jyotishId);

        public string AddJyotishVideo(JyotishVideosViewModel model);
        public string AddJyotishGallery(JyotishGalleryViewModel model);
        public List<JyotishGalleryModel> JyotishGallery(int Id);
        public List<JyotishVideosModel> JyotishVideos(int Id);
        public JyotishProfileUpdateViewModal GetProfile(int Id);
        public List<SubscrictionListJyotishViewModel> GetAllSubscription(int jyotishId);
        public List<JyotishPaymentRecordModel> JyotishPaymentrecords(int Id);
        public JyotishPaymentRecordModel JyotishPaymentDetail(int Id);
        public string AddAppointmentSlot(AppointmentSlotViewModel model);
         public string UpdateAppointmentSlot(AppointmentSlotViewModel model);
         public string DeleteAppointmentSlot(int Id);
        public List<AppointmentSlotUserViewModel> GetAllAppointmentSlot(int id);
        public List<AppointmentSlotUserViewModel> GetAllbookedAppointment(int id);
        public bool updateArrivedClient(int appointmentId,int jyotishId);
        public dynamic GetAllAppointmentHistory(int jyotishId);
        public dynamic GetAttachmentByAppointment(int appointmentId, int memberId);
        public List<SubscriptionFeatureViewModel> GetAllFeatures();

        public string AddProblemSolution(ProblemSolutionViewModel model);
        public ProblemSolutionJyotishGetViewModel GetProblemSolution(int appointmentId);
        public List<ProblemSolutionJyotishGetAllViewModel> GetAllProblemSolution(int JyotishId);
        public dynamic GetProblemSolutionDetail(int appointmentId);
        public List<AppointmentDetailViewModel> GetAllProblemSolutionByUser(int jyotishId, int UId);

        public string UpdateProblemSolution(ProblemSolutionViewModel model);
        public string DeleteProblemSolution(int Id);
        public string AddUserAttachment(JyotishUserAttachmentViewModel model);
        public List<JyotishUserAttachmentJyotishViewModel> GetAllUserAttachments(int Id);
        public string UpdateUserAttachment(JyotishUserAttachmentJyotishUpdateViewModel model);
        public string DeleteUserAttachment(int id);
       public bool AddClientMembers(ClientMembersViewModel model);
        public string PurchaseWithJyotishWallets(JyotishWalletViewmodel uw);

        public dynamic getClientMembers(int Id,int jyotishId);
        public dynamic GetAllUpcommingAppointment(int jyotishId);

        public string AddWallet(JyotishWalletViewmodel pr);
        public long GetWallet(int JyotishId);

        public string AddWalletHistory(WalletHistoryViewmodel pr);
        public dynamic GetWalletHistory(int JyotishId);
        public string RemoveSlotWithskipDates(AppointmentSlotViewModel model);
        public dynamic GetTodayAppointment(int JyotishId);
        public List<AppointmentSlotDetailsJyotish> AppointmentSlotDetails(int Id);
        public dynamic SkipDateList(int Id);
        public List<JyotishNotificationDataViewModel> NotificationData(int Id);
        public LayoutDataViewModel LayoutData(int Id);
        public dynamic getPlan(int Id);
        public bool upgradePackages(packages packages);
        public bool BookMark(int appointmentId);
        public JyotishDashboardDataViewModel JyotishDashboardData(int Id);
        public bool SendRequest(RedeemCodeRequestViewModel model);

        public dynamic getRedeemCode(int jyotishId);

        public bool AddAppointmentBookmark(AppointmentBookmarkViewModal modal);
        public bool DeleteAppointmentBookmark(int Id);
        public AppointmentBookmarkViewModal GetAppointmentBookmark(int Id);

        public dynamic GetUpcommingAppointmentById(int appointmentId);

        public bool purchaseAdvertisement(PurchaseAdvertisementService ps);

        public bool AddJyotishPooja(JyotishPoojaViewModel model);
        public dynamic getJyotishPoojaList(int Id);

        public bool UpdateJyotishPooja(JyotishPoojaViewModel model);
        public bool removeJyotishPooja(int PoojaId);
        public string changeJyotishActiveStatus(int jyotishId, bool status);

        public bool changeJyotishServiceStatus(int jyotishId, bool status);

        public bool getJyotishserviceStatus(int jyotishId);

        public dynamic getJyotishDashboardRecord(int jyotishId);

        public dynamic GetTopTenWalletHistory(int JyotishId);

        public dynamic getUserServiceRevordForJyotish(int jyotishId);
        public dynamic getPurchasedAdvertisement(int jyotishId);
        public bool AddPoojaBookMark(PoojaBookMarkService pms);
        public bool CompletePoojaContact(int id);
        public dynamic getBookedPoojaListWhichIsCompleted(int jyotishId);
        public dynamic getPoojaBookmark(int id);
        public bool removePoojaBookmark(int id);
        public bool RemoveVideo(int id);
        public bool RemoveGallery(int id);
        public bool UpdateGallery(JyotishGalleryViewModel model);
        public bool UpdateVideo(JyotishVideosViewModel model);
      

    }
}
