using BusinessAccessLayer.Abstraction;
using DataAccessLayer.DbServices;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ModelAccessLayer.Models;
using ModelAccessLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccessLayer.Implementation
{
    public class JyotishServices:IJyotishServices
    {
        private readonly ApplicationContext _context;
        private readonly string _uploadDirectory;
        public JyotishServices(ApplicationContext context)
        {
            _context = context;
            _uploadDirectory = Directory.GetCurrentDirectory();

            // Ensure directory exists
            if (!Directory.Exists(_uploadDirectory))
            {
                Directory.CreateDirectory(_uploadDirectory);
            }
        }

        public string UpdateProfile(JyotishCompleteViewModel model)
        {
            // Fetch the existing record from the database
            var existingRecord = _context.JyotishRecords.FirstOrDefault(x => x.Id == model.Id);
            if (existingRecord == null)
            {
                return "Jyotish Not Found";
            }

            // Validate email
            if (existingRecord.Email != model.Email)
            {
                return "Invalid Email";
            }

            // Update properties from the model
            existingRecord.Role = "Jyotish";
            existingRecord.Status = "Complete";

            if (model.ProfileImageUrl != null)
            {

                var ProfileGuid = Guid.NewGuid().ToString();
                var SqlPath = "wwwroot/PendingJyotish/Document/" + ProfileGuid + model.ProfileImageUrl.FileName;
                var ProfilePath = Path.Combine(_uploadDirectory, SqlPath);
                using (var stream = new FileStream(ProfilePath, FileMode.Create))
                {
                    model.ProfileImageUrl.CopyTo(stream);
                }
                existingRecord.ProfileImageUrl = "/Images/Jyotish/" + ProfileGuid + model.ProfileImageUrl.FileName;

            }

            // Update other properties as needed
            existingRecord.Mobile = model.Mobile;
            existingRecord.Name = model.Name;
            existingRecord.Gender = model.Gender;
            existingRecord.Language = model.Language;
            existingRecord.Expertise = model.Expertise;
            existingRecord.Country = model.Country;
            existingRecord.State = model.State;
            existingRecord.City = model.City;
            existingRecord.Password = model.Password;
            existingRecord.DateOfBirth = model.DateOfBirth;

            existingRecord.Otp = model.Otp;
            existingRecord.Experience = model.Experience;
            existingRecord.Pooja = model.Pooja;
            existingRecord.Call = model.Call;
            existingRecord.CallCharges = model.CallCharges;
            existingRecord.Chat = model.Chat;
            existingRecord.ChatCharges = model.ChatCharges;
            existingRecord.Address = model.Address;
            existingRecord.TimeTo = model.TimeTo;
            existingRecord.TimeFrom = model.TimeFrom;
            existingRecord.AppointmentCharges = model.AppointmentCharges;
            existingRecord.About = model.About;
            existingRecord.Specialization = model.Specialization;
            existingRecord.AwordsAndAchievement = model.AwordsAndAchievement;
            existingRecord.Pooja = model.Pooja;

            // Save changes to the database
            if (_context.SaveChanges() > 0)
            {
                return "Successful";
            }
            else
            {
                return "Data Not Saved";
            }
        }

        /*  public string UpdateProfile(JyotishCompleteViewModel model)
          {
              // Fetch the existing record from the database
              var existingRecord = _context.JyotishRecords.FirstOrDefault(x => x.Id == model.Id);

              if (existingRecord == null)
              {
                  throw new Exception("Jyotish Not Found");
              }

              // Validate email
              if (existingRecord.Email != model.Email)
              {
                  throw new Exception("Invalid Email");
              }

              // Update common properties
              existingRecord.Role = model.Role;
              existingRecord.Status = "Complete";

              // Handle profile image upload
              if (model.ProfileImageUrl != null)
              {
                  existingRecord.ProfileImageUrl = SaveFile(model.ProfileImageUrl, "ProfileImages");
              }

              // Update other properties from the model
              existingRecord.Mobile = model.Mobile;
              existingRecord.Name = model.Name;
              existingRecord.Gender = model.Gender;
              existingRecord.Language = model.Language;
              existingRecord.Expertise = model.Expertise;
              existingRecord.Country = model.Country;
              existingRecord.State = model.State;
              existingRecord.City = model.City;
              existingRecord.Password = model.Password; // Consider hashing the password
              existingRecord.DateOfBirth = model.DateOfBirth;

              existingRecord.Otp = model.Otp;
              existingRecord.Experience = model.Experience;
              existingRecord.Pooja = model.Pooja;
              existingRecord.Call = model.Call;
              existingRecord.CallCharges = model.CallCharges;
              existingRecord.Chat = model.Chat;
              existingRecord.ChatCharges = model.ChatCharges;
              existingRecord.Address = model.Address;
              existingRecord.TimeTo = model.TimeTo;
              existingRecord.TimeFrom = model.TimeFrom;
              existingRecord.AppointmentCharges = model.AppointmentCharges;



              // Save changes to the database
              if (_context.SaveChanges() > 0)
              {
                  return "Successful";
              }
              else
              {
                  throw new Exception("Data Not Saved");
              }
          }

          private string SaveFile(IFormFile file, string folderName)
          {
              var fileGuid = Guid.NewGuid().ToString();
              var fileName = Path.GetFileName(file.FileName);
              var filePath = Path.Combine(_uploadDirectory,"wwwroot" ,folderName, fileGuid + fileName);

              using (var stream = new FileStream(filePath, FileMode.Create))
              {
                  file.CopyTo(stream);
              }

              return $"/{folderName}/{fileGuid}{fileName}"; // Return the relative path
          }
  */

        public List<AppointmentModel> Appointment(string JyotishEmail) 
        {
            var Jyotish = _context.JyotishRecords.Where(x => x.Email == JyotishEmail).FirstOrDefault();
            var Records = _context.AppointmentRecords.Where(x=>x.JyotishId == Jyotish.Id).ToList();
            return Records;
        }
        public List<AppointmentModel> UpcomingAppointment(string JyotishEmail)
        {
            var Jyotish = _context.JyotishRecords.Where(x => x.Email == JyotishEmail).FirstOrDefault();
            DateTime Today =DateTime.Now;
            var Records = _context.AppointmentRecords.Where(e => e.DateTime > Today).Where(x=> x.JyotishId == Jyotish.Id).ToList();
            return Records;
        }
        public string AddAppointment(AppointmentViewModel model) 
        {
            var Jyotish = _context.JyotishRecords.Where(x => x.Id == model.JyotishId).FirstOrDefault();
            var User = _context.Users.Where(x => x.Id == model.UserId).FirstOrDefault();
            if (User != null || Jyotish != null) { return "invalid Data"; }
            AppointmentModel appointment = new AppointmentModel();
            appointment.Name = User.Name;
            appointment.Mobile = User.Mobile;
            appointment.DateTime = model.DateTime;
            appointment.Email = User.Email;
            appointment.JyotishId = Jyotish.Id;
            appointment.UserId = User.Id;
            appointment.Problem = model.Problem;
            appointment.Amount = Jyotish.AppointmentCharges;
            appointment.Status = "Upcomming";
            _context.AppointmentRecords.Add(appointment);
            var result = _context.SaveChanges();
            if (_context.SaveChanges() > 0) { return "Successful"; }
            return "internal Server Error.";

        }

        public List<TeamMemberModel> TeamMember(string JyotishEmail)
        {
            var Jyotish = _context.JyotishRecords.Where(x => x.Email == JyotishEmail).FirstOrDefault();
            if(Jyotish == null)
            { return null; }
            else
            {
                var records = _context.TeamMemberRecords.Where(x => x.JyotishId == Jyotish.Id).ToList();
                return records;
            }
            
        }
        public string AddTeamMember(TeamMemberViewModel teamMember,string path) 
        {
            var IsEmailValid = _context.TeamMemberRecords.Where(x => x.Email == teamMember.Email).FirstOrDefault();
            var IsMobileValid = _context.TeamMemberRecords.Where(x => x.Mobile == teamMember.Mobile).FirstOrDefault();
            if (IsEmailValid != null || IsMobileValid != null)
            { return "Email or Mobile no. Already Exist"; }

            var IsJyotishValid = _context.JyotishRecords.Where(x => x.Email == teamMember.JyotishEmail).FirstOrDefault();
            if (IsJyotishValid == null) 
            { return "Jyotish Not found";}

            Random random = new Random();
            // Generate a random number between 1000000000 and 9999999999
            long randomNumber = (long)(random.NextDouble() * 9000000000) + 1000000000;
            var filePath = "/Assets/Images/TeamMember/" + randomNumber + teamMember.ProfilePicture.FileName;

            var fullPath = path + filePath;
            UploadFile(teamMember.ProfilePicture, fullPath);

            TeamMemberModel model = new TeamMemberModel() 
            {
                Name= teamMember.Name,
                Mobile = teamMember.Mobile,
                ProfilePictureUrl = filePath,
                Email = teamMember.Email,
                Role= "TeamMember",
                JyotishId = IsJyotishValid.Id,
            };
            _context.TeamMemberRecords.Add(model);
             var result = _context.SaveChanges();
            if (result > 0)
            {
                return "Record Saved Successfully";
            }
            else 
            {
                return "Records Not Saved";
            }
        }
        public void UploadFile(IFormFile file, string fullPath)
        {
            FileStream stream = new FileStream(fullPath, FileMode.Create);
            file.CopyTo(stream);
        }
       /* public bool CreateAPooja(PoojaRecordModel model)
        {
            var isPoojaValid = _context.PoojaCategory.Where(x => x.Name == model.Name).FirstOrDefault();
            if (isPoojaValid != null)
            { return false; }
            _context.PoojaRecord.Add(model);
            int result = _context.SaveChanges();
            if (result > 0)
            { return true; }
            else { return false; }
        }*/
        public List<Country> CountryList() 
        {
            var Record = _context.Countries.ToList();
            return Record;
        }
        public List<State> StateList(int Id) 
        { 
            var Record = _context.States.Where(x => x.CountryId == Id).ToList();
            return Record;
        }
        public List<City> CityList(int Id) 
        {
            var Record = _context.Cities.Where(x=>x.StateId == Id).ToList();
            return Record;
        }

        public List<ExpertiseModel> ExpertiseList()
        {
            var Records = _context.ExpertiseRecords.ToList();
            return Records; 
        }
        public List<PoojaListModel> GetPoojaList()
        {
            var Records = _context.PoojaList.ToList();
            return Records;
        }
        public List<SpecializationListModel> GetSpecializationList()
        {
            var Records = _context.SpecializationList.ToList();
            return Records;
        }
        public JyotishDocumentViewModel DashBoard(string email)
        {
            if (email == null)
            { return null; }
            var IsEmailValid = _context.JyotishRecords.Where(x => x.Email == email).FirstOrDefault();
            if (IsEmailValid == null) { return null; }

            var calls = _context.CallingRecords.Where(x => x.JyotishId == IsEmailValid.Id).ToList().Count();
            var chats = _context.ChatingRecords.Where(x => x.JyotishId == IsEmailValid.Id).ToList().Count();
            var Appointments = _context.AppointmentRecords.Where(x => x.JyotishId == IsEmailValid.Id).ToList().Count();
            JyotishDocumentViewModel model = new JyotishDocumentViewModel()
            {
                Calls = calls,
                Chats = chats,
                Appointments = Appointments
            };

            return model;
        }
        public string AddJyotishVideo(JyotishVideosViewModel model)
        {
            if (model == null) { return "invalid data"; }
            JyotishVideosModel data = new JyotishVideosModel();
            data.VideoTitle = model.VideoTitle;
            data.VideoUrl = model.VideoUrl;
            data.JyotishId = model.JyotishId;
            _context.JyotishVideos.Add(data);
            if (_context.SaveChanges() > 0) { return "Successful"; }
            else { return "internal server Error"; }
        }

        public string AddJyotishGallery(JyotishGalleryViewModel model)
        {
            if (model == null) { return "invalid data"; }
            JyotishGalleryModel data = new JyotishGalleryModel();

            if (model.ImageUrl != null)
            {
                var ProfileGuid = Guid.NewGuid().ToString();
                var SqlPath = "wwwroot/PendingJyotish/Document/" + ProfileGuid + model.ImageUrl.FileName;
                var ProfilePath = Path.Combine(_uploadDirectory, SqlPath);
                using (var stream = new FileStream(ProfilePath, FileMode.Create))
                {
                    model.ImageUrl.CopyTo(stream);
                }
                data.ImageUrl = "/Images/Jyotish/" + ProfileGuid + model.ImageUrl.FileName;
            }


            data.ImageTitle = model.ImageTitle;
            data.JyotishId = model.JyotishId;
            _context.JyotishGallery.Add(data);
            if (_context.SaveChanges() > 0) { return "Successful"; }
            else { return "internal server Error"; }
        }
       

        public List<JyotishVideosModel> JyotishVideos(int Id)
        {
            var records = _context.JyotishVideos.Where(x => x.JyotishId == Id).ToList();
            return records;
        }
        public List<JyotishGalleryModel> JyotishGallery(int Id)
        {
            var records = _context.JyotishGallery.Where(x => x.JyotishId == Id).ToList();
            return records;
        }
    }
}
