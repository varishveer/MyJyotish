using ModelAccessLayer.Models;
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
        public List<JyotishModel> GetAllJyotish();
        public List<JyotishModel> GetAllPendingJyotish();
        public List<UserModel> GetAllUser();
        public List<TeamMemberModel> GetAllTeamMember();
        public List<AppointmentListAdminViewModel> GetAllAppointment();
        public bool ApproveJyotish(IdViewModel JyotishId);
        public bool RejectJyotish(JyotishRejectMailViewModel model);
        public bool RemoveJyotish(IdViewModel JyotishId);
    /*    public bool AddPoojaCategory(PoojaCategoryViewModel _pooja);*/
     /*   public bool AddNewPoojaList(PoojaListViewModel model);*/
        public bool AddExpertise(ExpertiseModel _expertise);
        public List<ExpertiseModel> GetAllExpertise();
      /*  public List<PoojaCategoryModel> GetAllPoojaCategory();*/
        public AdminModel Profile(string email);
  /*      public AdminDashboardViewModal Dashboard();*/
        public List<PoojaRecordModel> PoojaRecord();
        public List<ChattingModel> ChattingRecord();
        public List<CallingModel> CallingRecord();
        public AppointmentDetailViewModel AppointmentDetails(int id);
        public string UpdateAppointment(UpdateAppointmentJyotishViewModel model);
        public bool AddCountry(Country _country);
        public bool AddState(State _state);
        public bool AddCity(City _city);
        public bool AddSlider(SliderImagesViewModel model);
        public List<SliderImagesModel> SliderImageList();
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

        public string AddFeature(SubscriptionFeatureViewModel.Add model);
        public List<SubscriptionFeatureViewModel.GetAll> GetAllFeatures();
        public string DeleteFeature(int Id);
        public string UpdateFeature(SubscriptionFeatureViewModel.Update model);
        public string AddManageSubscriptionData(ManageSubscriptionViewModel model);
        public string UpdateManageSubscriptionData(ManageSubscriptionViewModel model);
        public List<ManageSubscriptionViewModel> GetAllManageSubscriptionData();
        public string DeleteManageSubscriptionData(int Id);

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
     
         public ProblemSolutionAdminViewModel GetProblemSolution(int Id);

        public string ReschduledInterview(ReschduledInterviewViewModel model);
        public List<InteviewListViewModel> InteviewList();

        public JyotishModel JyotishProfile(int Id);
        public List<AdminNoticationDataViewModel> NotificationData();
    }
}
