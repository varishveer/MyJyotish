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
        public List<IdImageViewModel> SliderImageList(string keyword);
        public string BookAppointment(AppointmentViewModel model);
        public List<JyotishModel> SpecializationFilter(string Keyword);
        public UserModel GetUserProfile(int Id);
        public string UpdateProfile(UserUpdateViewModel model, string path);
        public AppointmentModel GetAppointmentDetails(int Id);
        public List<AppointmentDetailViewModel> getAllAppointment(int Id);
        public List<UserPaymentRecordModel> UserPaymentrecords(int Id);
        public UserPaymentRecordModel UserPaymentDetail(int Id);
        public List<AppointmentSlotUserViewModel> GetAllAppointmentSlot(int id);
        public List<ProblemSolutionGetAllViewModel> GetAllProblemSolution(int Id);
        public ProblemSolutionGetAllViewModel GetProblemSolution(int Id);
        public string AddUserWallets(UserWalletViewmodel uw);
        public string PurchaseWithUserWallets(UserWalletViewmodel uw);

        public long GetWallet(int JyotishId);
        public List<City> selecAllCity();

        public string AddWalletHistory(WalletHistoryViewmodel pr);
        public dynamic GetWalletHistory(int UserId);
        public LayoutDataViewModel LayoutData(int Id);

    }
}
