using ModelAccessLayer.Models;
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
        public string UpdateProfile(JyotishUpdateViewModel model);
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

        public string AddJyotishVideo(JyotishVideosViewModel model);
        public string AddJyotishGallery(JyotishGalleryViewModel model);
        public List<JyotishGalleryModel> JyotishGallery(int Id);
        public List<JyotishVideosModel> JyotishVideos(int Id);
        public JyotishModel GetProfile(int Id);
        public List<SubscriptionGetViewModel> GetAllSubscription();
        public List<JyotishPaymentRecordModel> JyotishPaymentrecords(int Id);
        public JyotishPaymentRecordModel JyotishPaymentDetail(int Id);
        public string AddAppointmentSlot(AppointmentSlotViewModel model);
         public string UpdateAppointmentSlot(AppointmentSlotViewModel model);
         public string DeleteAppointmentSlot(int Id);
        public List<AppointmentSlotUserViewModel> GetAllAppointmentSlot(int id);
        public string AddProblemSolution(ProblemSolutionViewModel model);
        public ProblemSolutionJyotishGetAllViewModel GetProblemSolution(int appointmentId);
        public string UpdateProblemSolution(ProblemSolutionViewModel model);
        public string DeleteProblemSolution(int Id);
        public string AddUserAttachment(JyotishUserAttachmentViewModel model);
        public List<JyotishUserAttachmentJyotishViewModel> GetAllUserAttachments(int Id);
        public string UpdateUserAttachment(JyotishUserAttachmentJyotishUpdateViewModel model);
        public string DeleteUserAttachment(int id);
       
        public string AddWallet(JyotishWalletViewmodel pr);
        public long GetWallet(int JyotishId);
        
        public string AddWalletHistory(JyotishWalletHistoryViewmodel pr);
        public List<WalletHistoryModel> GetWalletHistory(int JyotishId);
    }
}
