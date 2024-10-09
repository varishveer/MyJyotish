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
        public List<AppointmentModel> GetAllAppointment();
        public bool ApproveJyotish(IdViewModel JyotishId);
        public bool RejectJyotish(IdViewModel JyotishId);
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
        public AppointmentModel AppointmentDetails(int id);
        public bool UpdateAppointment(AppointmentModel model);
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
        public string ApproveJyotishDocs(EmailDocumentViewModel model);
        public string RejectJyotishDocs(EmailDocumentViewModel model);
        public string AddSlot(SlotViewModel slot);
        public List<SlotModel> SlotList();

        public string AddFeature(SubscriptionFeaturesModel model);
        public List<SubscriptionFeaturesModel> GetAllFeatures();
        public string DeleteFeature(int Id);
        public string UpdateFeature(SubscriptionFeaturesModel model);
        public string AddSubscription(SubscriptionViewModel model);
        public string UpdateSubscription(SubscriptionViewModel model);
        public List<SubscriptionGetViewModel> GetAllSubscription();
        public string DeleteSubsciption(int Id);
      /*  public string ApproveDocument(DocUpdateViewModel model);
        public string RejectDocument(DocUpdateViewModel model);
      */
    }
}
