using ModelAccessLayer.Models;
using ModelAccessLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserWalletViewmodel = ModelAccessLayer.ViewModels.UserWalletViewmodel;

namespace BusinessAccessLayer.Abstraction
{
    public interface IUserServices
    {



        public List<JyotishModel> GetAstroListCallChat(string ListName);
        /*       public List<PoojaCategoryModel> GetAllPoojaCategory();*/
        /*  public List<PoojaRecordModel> GetPoojaList(int id);*/
        public PoojaRecordModel GetPoojaDetail(int PoojaId);
        public List<JyotishModel> TopAstrologer(string City);
        public List<JyotishModel> AllAstrologer();
        public JyotishProfileViewModel AstrologerProfile(int Id);
        public List<JyotishModel> FilterAstrologer(FilterModel fm);
        public List<IdImageViewModel> SliderImageList();
        public string BookAppointment(AppointmentViewModel model);
        public List<JyotishModel> SpecializationFilter(string Keyword);
        public UserModel GetUserProfile(int Id);
        public string UpdateProfile(UserUpdateViewModel model, string path);
        public AppointmentModel GetAppointmentDetails(int Id);
        public List<AppointmentDetailViewModel> UpcommingAppointment(int Id);
        public List<AppointmentDetailViewModel> AppointmentHistory(int Id);
        public List<UserPaymentRecordModel> UserPaymentrecords(int Id);
        public UserPaymentRecordModel UserPaymentDetail(int Id);
        public List<AppointmentSlotUserViewModel> GetAllAppointmentSlot(int id);
        public List<ProblemSolutionGetAllViewModel> GetAllProblemSolution(int Id);

        public string AddUserWallets(UserWalletViewmodel uw);
        public string PurchaseWithUserWallets(UserWalletViewmodel uw);
        public List<JyotishModel> SearchAstrologer(string? searchInp);
        public List<JyotishUserAttachmentJyotishViewModel> GetAllUserAttachments(int Id);
        public dynamic GetProblemSolutionDetail(int appointmentId);

        public ProblemSolutionJyotishGetViewModel GetProblemSolution(int appointmentId);
        public dynamic GetAttachmentByAppointment(int appointmentId, int memberId);

        public long GetWallet(int JyotishId);
        public List<City> selecAllCity();

        public string AddWalletHistory(WalletHistoryViewmodel pr);
        public dynamic GetWalletHistory(int UserId);
        public LayoutDataViewModel LayoutData(int Id);

        public string AddRating(JyotishRatingViewModel data);
        public List<JyotishRatingViewModel> JyotishRatingList(int Id);
        public string IsUserValidForRating(int UserId, int JyotishId);
        public bool AddUserServiceRecord(UserServiceRecordViewModel data);
        public UserServiceRecordViewModel GetUserDataForService(int Id);

        public bool AddKundaliMatchingRecord(List<KundaliMatchingViewModel> DataList);
        public List<KundaliMatchingViewModel> GetAllKundaliMatchingRecord(int Id);
        public List<KundaliMatchingViewModel> GetLatestKundaliRecord(int Id);
        public bool DeleteKundaliRecord(int Id);
        public bool DeleteKundaliRecord(int UserId, int Id);
        public dynamic getAllPoojaList();
        public dynamic getPoojaDetailByPoojaId(int Id);
    }

}
