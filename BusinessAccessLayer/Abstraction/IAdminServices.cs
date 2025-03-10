﻿using ModelAccessLayer.Models;
using ModelAccessLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccessLayer.Abstraction
{
    public interface IAdminServices
    {
        public dynamic GetAllJyotish();
        public List<JyotishModel> GetAllPendingJyotish();
        public dynamic GetAllUser();
        public List<TeamMemberModel> GetAllTeamMember();
        public string AddAppointmentSlot(AppointmentSlotViewModel model);
        public List<AppointmentListAdminViewModel> GetAllAppointment();
        public bool ApproveJyotish(IdViewModel JyotishId);
        public bool RejectJyotish(JyotishRejectMailViewModel model);
        public bool RemoveJyotish(IdViewModel JyotishId);
        /*    public bool AddPoojaCategory(PoojaCategoryViewModel _pooja);*/
        /*   public bool AddNewPoojaList(PoojaListViewModel model);*/
        public bool AddPoojaList(PoojaCategoryViewModel _pooja);

        public List<SpecializationListModel> GetSpecializationList();
        public dynamic getPlan(int Id);
        public bool countryCode(CountryCodeViewModel model);
        public dynamic getCountryCode();

        public bool AddExpertise(ExpertiseModel _expertise);
        public List<ExpertiseModel> GetAllExpertise();
      /*  public List<PoojaCategoryModel> GetAllPoojaCategory();*/
        public AdminModel Profile(string email);
  /*      public AdminDashboardViewModal Dashboard();*/
        public List<PoojaRecordModel> PoojaRecord();
        public bool AddSpecialization(string specName);
        public string RemoveSlotWithskipDates(AppointmentSlotViewModel model);

        public List<ChattingModel> ChattingRecord();
        public List<CallingModel> CallingRecord();
        public AppointmentDetailViewModel AppointmentDetails(int id);
        public string UpdateAppointment(UpdateAppointmentJyotishViewModel model);
        public bool AddCountry(Country _country);
        public bool AddState(State _state);
        public bool AddCity(City _city);
        public bool AddSlider(SliderImagesViewModel model);
        public List<SliderImagesModel> SliderImageList();
        public bool DeleteHomePageBanner(int Id);
       /* public bool AddPoojaDetail(PoojaRecordViewModel model);*/
        public List<DocumentModel> GetAllJyotishDocument();
        public List<CallingModel> GetJyotishCalls(int id);
        public List<ChattingModel> GetJyotishChats(int id);
        public DocumentModel GetJyotishDocs(int id);
        public JyotishModel GetJyotishDetails(int id);
        public string UpdateJyotishDetails(JyotishDetailsViewModel model);
        public string ApproveJyotishDocs(JyotishDocApproveViewModel model);
        public string RejectJyotishDocs(EmailDocumentViewModel model);
        public string AddSlot(SlotViewModel slot);
        public List<SlotModel> SlotList();
        public List<Department> DepartmentList();
        public List<levels> LevelsList();
        public bool AddEmployees(EmployeesViewModel model);

        public string AddFeature(SubscriptionFeatureViewModel model);
        public List<SubscriptionFeatureViewModel> GetAllFeatures();
        public string DeleteFeature(int Id);
        public string UpdateFeature(SubscriptionFeatureViewModel model);
        public string AddManageSubscriptionData(ManageSubscriptionViewModel model);
        public string UpdateManageSubscriptionData(ManageSubscriptionViewModel model);
        public List<ManageSubscriptionViewModel> GetAllManageSubscriptionData();
        public string DeleteManageSubscriptionData(int Id);
        public dynamic getJyotishByEmail(string email);
        public bool AddRedeamCode(redeamCodeViewModel model);
        public bool AddEmployeesDocs(EmployeesDocsViewModel model);

        public string AddSubscription(SubscriptionViewModel model);
        public string UpdateSubscription(SubscriptionViewModel model);
        public List<SubscriptionViewModel> GetAllSubscription();
        public string DeleteSubsciption(int Id);
        public SubscriptionGetViewModel GetSubscription(int Id);
        public List<JyotishPaymentRecordModel> JyotishPaymentrecords();
        public JyotishPaymentRecordModel JyotishPaymentDetail(int Id);
        public List<UserPaymentRecordModel> UserPaymentrecords();
        public UserPaymentRecordModel UserPaymentDetail(int Id);
        public List<ProblemSolutionAdminViewModel> GetAllProblemSolution();
        public bool AddDepartments(DepartmentViewModel model);

        public bool AddLevels(LevelsViewModel model);
        public dynamic getEmployeesList();
        public dynamic getEmployeesDocsList(int employeeId);
        public bool deleteAdvertisementPAckage(int id);

         public ProblemSolutionAdminViewModel GetProblemSolution(int Id);

        public string ReschduledInterview(ReschduledInterviewViewModel model);
        public List<InteviewListViewModel> InteviewList();

        public JyotishModel JyotishProfile(int Id);
        public List<AdminNoticationDataViewModel> NotificationData();
        public string AddInterviewFeedback(InterviewFeedbackViewModel feedback);
        public List<InterviewFeedbackViewModel> GetAllInterviewFeedback();
        public List<JyotishRatingViewModel> PendingRatingList();
        public List<JyotishRatingViewModel> ApprovedRatingList();
        public string ApproveRating(int Id);
        public string DeleteRating(int Id);
        public bool AddAccessPages(EmployeesAccessPagesViewModel model);
        public List<EmployeesAccessPages> getAccessPages();
        public bool AddInterviewMeeting(InterviewMeetingViewModel data);
        public List<InterviewMeetingViewModel> InterviewMeetingList();
        public List<InterviewMeetingViewModel> InterviewMeetingListByJyotishId(int jytotishId);
        public Dictionary<string, bool> startAndEndDateofMeating(int slotBookingId);

        public bool UpdatePoojaList(PoojaCategoryViewModel _pooja);

        public bool RemovePoojaList(int Id);

		public bool StartAndEndInterviewTime(int SlotBookingId, bool Time);
        public bool AddEmployeeInterviewFeedback(EmployeeInterviewFeedbackViewModel data);
        public List<EmployeeInterviewFeedbackViewModel> EmployeeInterviewFeedbackList();
        public bool AddPageAccessValidation(AccessPageValidation model);

        public bool AddDiscount(RedeemDiscountValidationViewModel model);
        public dynamic getEmployeePages(int employeeId);

        public dynamic GetRedeemRequest();
        public dynamic getRedeemCodeForApprove();

        public bool ApproveRedeem(int RedeemId);
        public bool RejectRedeem(int RedeemId);

        public List<AppointmentSlotDetailsJyotish> SlotDetails();
        public List<SkipdateJyotishViewModel> SlotSkipList();

		public bool CreateAPooja(PoojaRecordModel model);
        public List<SubscrictionListJyotishViewModel> GetAllSubscriptionForAdmin();

        public dynamic getAdvertisementPackage();

        public dynamic poojaByPoojaId(int id);
		public bool UpdatePooja(PoojaRecordModel model);
		public bool removePooja(int Id);
		public dynamic getAllPooja();
        public bool AddJyotishCharges(int charges, int jyotishId);
        public bool appointmentManagement(int charges);
        public dynamic getAllJyotishCharges();
        public dynamic getAppointmentCharges();
        public dynamic getJyotishChargesById(int jyotishId);

        public dynamic getAllParticularJyotishCharges();
        public dynamic getAdminDashboard();
        public bool AddPrivacyPolicy(PrivacyPolicyService pp);
        public dynamic getPrivacyPolicy();
        public bool createAdvertisementPackage(AdvertisementPackageService aps);
        public dynamic getPurchasedAdvertisement();
        public bool changeApproveStatusOfAdvertisement(int id, bool appstatus);

        public bool changeActiveStatusOfAdvertisement(int id);
        public bool AddActiveMail(string email, string password);
        public bool ChangeActiveMail(int id);
        public dynamic GetActivemail();
        public dynamic GetAllActivemail();
        public string AddAdminVideo(AdminVideoService model);
        public bool UpdateVideo(AdminVideoService model);
        public bool RemoveVideo(int id);
        public dynamic GetAdminVideoList();

        public bool DeleteActiveMail(int id);
        public bool MakeContactComplete(int id);
        public dynamic GetEnquiryList();
        public dynamic GetEnquiryHistoryList();
        public dynamic GetTop3AdminVideoList();

    }
}
