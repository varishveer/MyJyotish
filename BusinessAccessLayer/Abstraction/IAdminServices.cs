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
        public bool AddPoojaCategory(PoojaCategoryModel _pooja);
        public bool AddNewPoojaList(PoojaListViewModel model);
        public bool AddExpertise(ExpertiseModel _expertise);
        public List<ExpertiseModel> GetAllExpertise();
        public List<PoojaCategoryModel> GetAllPoojaCategory();
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
        public bool AddPoojaDetail(PoojaRecordViewModel model);

    }
}
