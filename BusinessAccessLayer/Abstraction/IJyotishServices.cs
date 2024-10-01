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
        public string UpdateProfile(JyotishCompleteViewModel model);
        public List<AppointmentModel> Appointment(string JyotishEmail);
        public List<AppointmentModel> UpcomingAppointment(string JyotishEmail);
        public string AddAppointment(AppointmentViewModel appointment);
        public List<TeamMemberModel> TeamMember(string JyotishEmail);
        public string AddTeamMember(TeamMemberViewModel teamMember, string Path);
        public bool CreateAPooja(PoojaRecordModel model);
        public List<Country> CountryList();
        public List<State> StateList(int Id);
        public List<City> CityList(int Id);
        public List<ExpertiseModel> ExpertiseList();
        public JyotishDocumentViewModel DashBoard(string email);
      

    }
}
