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



        public dynamic GetAstroListCallChat(string ListName);
        /*       public List<PoojaCategoryModel> GetAllPoojaCategory();*/
        /*  public List<PoojaRecordModel> GetPoojaList(int id);*/
        public PoojaRecordModel GetPoojaDetail(int PoojaId);
        public dynamic TopAstrologer();
        public List<JyotishModel> AllAstrologer();
        public JyotishProfileViewModel AstrologerProfile(int Id);
        public List<JyotishModelService> FilterAstrologer(FilterModel fm);
        public List<IdImageViewModel> SliderImageList();
        public string BookAppointment(AppointmentViewModel model);
        public List<JyotishModel> SpecializationFilter(string Keyword);
        public UserProfileViewModal GetUserProfile(int Id);
        public bool UpdateProfile(UserUpdateViewModel model, string path);
        public AppointmentModel GetAppointmentDetails(int Id);
        public List<AppointmentDetailViewModel> UpcommingAppointment(int Id);
        public List<AppointmentDetailViewModel> AppointmentHistory(int Id);
        public List<UserPaymentRecordModel> UserPaymentrecords(int Id);
        public UserPaymentRecordModel UserPaymentDetail(int Id);
        public List<AppointmentSlotUserViewModel> GetAllAppointmentSlot(int id);
        public string AddUserWallets(UserWalletViewmodel uw);
        public string PurchaseWithUserWallets(UserWalletViewmodel uw);
        public List<JyotishModelService> SearchAstrologer(string? searchInp);
        public List<JyotishUserAttachmentJyotishViewModel> GetAllUserAttachments(int Id);
        public dynamic GetProblemSolutionDetail(int appointmentId);
        public bool changeUserServiceStatus(int userId,bool status);

        public ProblemSolutionJyotishGetViewModel GetProblemSolution(int appointmentId);
        public dynamic GetAttachmentByAppointment(int appointmentId, int memberId);
        public dynamic getJyotishServicesCharges(int jyotishid);
        public dynamic getJyotishCallServicesCharges(int jyotishid);
        public bool getUserserviceStatus(int userId);

        public Task<List<Advertisementservice>> AdvertisementBanner();

        public long GetWallet(int UserId);
        public List<City> selecAllCity();

        public string AddWalletHistory(WalletHistoryViewmodel pr);
        public dynamic GetWalletHistory(int UserId);
        public LayoutDataViewModel LayoutData(int Id);
        public bool ApplyChargesFromUserWalletForService(int userId, int amount, string message, int jyotishId);

        public string AddRating(JyotishRatingViewModel data);
        public List<JyotishRatingViewModel> JyotishRatingList(int Id);
        public string IsUserValidForRating(int UserId, int JyotishId);
        public bool AddUserServiceRecord(UserServiceRecordViewModel data);
        public UserServiceRecordViewModel GetUserDataForService(int Id);

        public bool AddKundaliMatchingRecord(List<KundaliMatchingViewModel> DataList);
        public List<KundaliMatchingViewModel> GetAllKundaliMatchingRecord(int Id);
        public List<KundaliMatchingViewModel> GetLatestKundaliRecord(int Id);
        public bool DeleteKundaliRecord( int Id);
        public dynamic getAppointmentCharges();
        public dynamic getAllPoojaList();
        public dynamic getPoojaDetailByPoojaId(int Id);
        public bool BookPooja(BookedPoojaViewModel model);
        public string GetTimezone(string country);
        public dynamic getJyotishRecordByPoojaType(int poojaTypeId);
        public dynamic getAllmembers(int userId);

        public dynamic getAllAppointmentBymemebersanduser(int memberId, int userId, int jyotishId);
        public dynamic getjyotishByuserAppointment(int userId,int memberId);

        public dynamic GettopTenWalletHistory(int UserId);
        public  Task<List<Advertisementservice>> GetTopOneAdvertisementBanner();

        public dynamic getUserServiceRevordForUser(int userId);

        public bool UpdateUserService(int userId, int action, double totalTime);
        public bool AddContactUs(ContactUsServices cs);
       

    }

}
