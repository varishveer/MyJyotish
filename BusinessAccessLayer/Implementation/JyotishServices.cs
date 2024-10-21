using BusinessAccessLayer.Abstraction;
using DataAccessLayer.DbServices;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
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

        public string UpdateProfile(JyotishUpdateViewModel model)
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



            var CountryName = _context.Countries.Where(x => x.Id == model.Country).FirstOrDefault();
            var StateName = _context.States.Where(x => x.Id == model.State).FirstOrDefault();
            var CityName = _context.Cities.Where(x => x.Id == model.City).FirstOrDefault();
            // Update other properties as needed
            existingRecord.Mobile = model.Mobile;
            existingRecord.Name = model.Name;
            existingRecord.Gender = model.Gender;
            existingRecord.Language = model.Language;
            existingRecord.Expertise = model.Expertise;
            existingRecord.Country = CountryName.Name;
            existingRecord.State = StateName.Name;
            existingRecord.City = CityName.Name;
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
            
            _context.JyotishRecords.Update(existingRecord);
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

          public List<AppointmentModel> GetAllAppointment(int Id) 
         {
             var Jyotish = _context.JyotishRecords.Where(x => x.Id == Id).FirstOrDefault();
            if (Jyotish == null) {return null; }
             var Records = _context.AppointmentRecords.Where(x=>x.JyotishId == Jyotish.Id).ToList();
             return Records;
         }
         public List<AppointmentModel> UpcomingAppointment(int Id)
         {
             var Jyotish = _context.JyotishRecords.Where(x => x.Id == Id).FirstOrDefault();
            if(Jyotish== null) { return null; }
             DateTime Today =DateTime.Now;
             var Records = _context.AppointmentRecords.Where(e => e.Date > Today).Where(x=> x.JyotishId == Jyotish.Id).ToList();
             return Records;
         }
        public string AddAppointment(AddAppointmentJyotishModel model) 
         {   
             var Jyotish = _context.JyotishRecords.Where(x => x.Id == model.JyotishId).FirstOrDefault();
             var User = _context.Users.FirstOrDefault(x => x.Email == model.Email || x.Mobile == model.Mobile);
            var appointmentRecord = _context.AppointmentRecords.Where(x => x.SlotId == model.SlotId).FirstOrDefault();

            var Slot = _context.AppointmentSlots.Where(x => x.Id == model.SlotId).FirstOrDefault();
             if (Jyotish == null || Slot == null || Slot.Status == "Booked" || appointmentRecord != null) { return "invalid Data"; }

             AppointmentModel appointment = new AppointmentModel();
             appointment.Name = model.Name;
             appointment.Mobile = model.Mobile;
             appointment.Date = model.Date;
            appointment.Time = model.Time;
             appointment.Email = model.Email;
             appointment.JyotishId = model.JyotishId;
            appointment.SlotId = model.SlotId;
            appointment.TimeDuration = model.TimeDuration;

             if(User == null)
             {
                 UserModel userModel = new UserModel();
                 userModel.Email = model.Email;
                 userModel.Mobile = model.Mobile;
                 userModel.Name = model.Name;
                 userModel.Password = (new Random().Next(10000000, 100000000)).ToString();
                _context.Users.Add(userModel);
                if (_context.SaveChanges() > 0)
                { appointment.UserId = userModel.Id; }
                  
                 AccountServices.SendEmail("Password" + userModel.Password, model.Email, "MyJyotishG Password");
             }
             else { appointment.UserId = User.Id; }  
             appointment.Problem = model.Problem;
             appointment.Amount = Jyotish.AppointmentCharges;
             appointment.Status = "Upcomming";
             _context.AppointmentRecords.Add(appointment);
            Slot.Status = "Booked";
            _context.AppointmentSlots.Update(Slot);
             if (_context.SaveChanges() > 0) 
            { 
                return "Successful"; }
             return "internal Server Error.";
         }

         public string UpdateAppointment(UpdateAppointmentJyotishViewModel model) 
         {
             var Jyotish = _context.JyotishRecords.Where(x => x.Id == model.JyotishId).FirstOrDefault();
             var User = _context.Users.Where(x => x.Email == model.Email).FirstOrDefault();
             var appointment = _context.AppointmentRecords.Where(x => x.Id == model.Id).FirstOrDefault();
            var slot = _context.AppointmentSlots.Where(x => x.Id == model.SlotId).FirstOrDefault();
             if (User == null || Jyotish == null || appointment == null || slot == null) { return "invalid Data"; }


             appointment.Name = User.Name;
             appointment.Mobile = User.Mobile;
            
             appointment.Email = User.Email;
             appointment.JyotishId = Jyotish.Id;
             appointment.UserId = User.Id;
             appointment.Problem = model.Problem;
             appointment.Amount = Jyotish.AppointmentCharges;
            if (appointment.SlotId == model.SlotId)
            {
                appointment.TimeDuration = slot.TimeDuration;
                appointment.Time = slot.TimeFrom;
                appointment.Date = slot.Date;
            }
            var oldSlot = _context.AppointmentSlots.Where(x => x.Id == appointment.SlotId).FirstOrDefault();
            oldSlot.Status = "Vacant";
            _context.AppointmentSlots.Update(oldSlot);

            appointment.SlotId = model.SlotId;
            appointment.Date = slot.Date;
            appointment.TimeDuration = slot.TimeDuration;
            appointment.Time = slot.TimeFrom;
            

            appointment.Status = "Upcomming";
             _context.AppointmentRecords.Update(appointment);

             if (_context.SaveChanges() > 0) { return "Successful"; }
             return "internal Server Error.";
         }

         public AppointmentModel GetAppointment(int Id)
         {
            var appointment = _context.AppointmentRecords.Where(x => x.Id == Id).FirstOrDefault();
             if (appointment == null) { return null; }
             else { return appointment; }
         }
        public List<TeamMemberModel> TeamMember(int Id)
        {
            var Jyotish = _context.JyotishRecords.Where(x => x.Id == Id).FirstOrDefault();
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

            var IsJyotishValid = _context.JyotishRecords.Where(x => x.Id == teamMember.Id).FirstOrDefault();
            if (IsJyotishValid == null) 
            { return "Jyotish Not found";}

            Random random = new Random();
            // Generate a random number between 1000000000 and 9999999999
            long randomNumber = (long)(random.NextDouble() * 9000000000) + 1000000000;
            var filePath = "/Images/TeamMember/" + randomNumber + teamMember.ProfilePicture.FileName;
            var realPath = "/wwwroot/Images/TeamMember/" + randomNumber + teamMember.ProfilePicture.FileName;

            var fullPath = path + realPath;
            UploadFile(teamMember.ProfilePicture, fullPath);
            string newPassword = ((random.NextDouble() * 90000000) + 10000000).ToString();

            TeamMemberModel model = new TeamMemberModel()
            {
                Name = teamMember.Name,
                Mobile = teamMember.Mobile,
                ProfilePictureUrl = filePath,
                Email = teamMember.Email,
                Role = "TeamMember",
                JyotishId = IsJyotishValid.Id,
                Password =newPassword
            };

            _context.TeamMemberRecords.Add(model);
             var result = _context.SaveChanges();
            if (result > 0)
            {
                AccountServices.SendEmail(newPassword, teamMember.Email, "Password");
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
                var SqlPath = "wwwroot/Images/Jyotish/" + ProfileGuid + model.ImageUrl.FileName;
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

        public JyotishModel GetProfile(int Id)
        {
            var record = _context.JyotishRecords.Where(x => x.Id == Id).FirstOrDefault();
            return record;
        }

        public List<SubscriptionGetViewModel> GetAllSubscription()
        {
            var records = _context.Subscriptions
                                  .Select(subscription => new SubscriptionGetViewModel
                                  {
                                      SubscriptionId = subscription.SubscriptionId,
                                      Name = subscription.Name,
                                      OldPrice = subscription.OldPrice,
                                      NewPrice = subscription.NewPrice,
                                      Discount = subscription.Discount,
                                      Gst = subscription.Gst,
                                      DiscountAmount = subscription.DiscountAmount,
                                      PlanType = subscription.PlanType,
                                      description = subscription.description,
                                      Status = subscription.Status,

                                      Features = _context.ManageSubscriptionModels
                                            .Where(ms => ms.SubscriptionId == subscription.SubscriptionId)
                                            .Select(ms => ms.FeatureId)
                                            .Join(_context.SubscriptionFeatures,
                                                  featureId => featureId,
                                                  feature => feature.FeatureId,
                                                  (featureId, feature) => feature.Name)
                                            .ToArray()
                                  })
                                  .ToList();

            return records;
        }

        public List<JyotishPaymentRecordModel> JyotishPaymentrecords(int Id)
        {
            var Jyotish = _context.JyotishRecords.Where(x => x.Id == Id).FirstOrDefault();
            if (Jyotish == null) { return null; }
            var record = _context.JyotishPaymentRecord.Where(x => x.JyotishId == Id).ToList();
            if (record.Count > 0) { return record; }
            else { return null; }
        }
        public JyotishPaymentRecordModel JyotishPaymentDetail(int Id)
        {
           
            var record = _context.JyotishPaymentRecord.Where(x => x.Id == Id).FirstOrDefault();
            if (record != null) { return record; }
            else { return null; }
        }

        public string AddAppointmentSlot(AppointmentSlotViewModel model)
        {
            if(model.Date < DateTime.Now)
            { return "Invalid Date"; }

            var Jyotish = _context.JyotishRecords.Where(x => x.Id == model.JyotishId).FirstOrDefault();
            if (Jyotish == null) { return "Invalid Jyotish"; }


          

            // Calculate total duration in minutes
            double totalDuration = (model.TimeTo.ToTimeSpan() - model.TimeFrom.ToTimeSpan()).TotalMinutes;

            // Calculate number of intervals
            int intervals = (int)(totalDuration / model.TimeDuration);

            // Calculate and store the time intervals
            List<(TimeOnly Start, TimeOnly End)> timeIntervals = new List<(TimeOnly, TimeOnly)>();
            for (int i = 0; i < intervals; i++)
            {
                TimeOnly intervalStart = model.TimeFrom.AddMinutes(i * model.TimeDuration);
                TimeOnly intervalEnd = intervalStart.AddMinutes(model.TimeDuration);
                timeIntervals.Add((intervalStart, intervalEnd));
                AppointmentSlotModel data = new AppointmentSlotModel();
                data.Date = model.Date;
                data.TimeFrom = intervalStart;
                data.TimeTo = intervalEnd;
                data.JyotishId = model.JyotishId;
                data.TimeDuration = model.TimeDuration;
                data.Status = "Vacant";
                _context.AppointmentSlots.Add(data);
            }





           /* AppointmentSlotModel data = new AppointmentSlotModel();
            data.Date = model.Date;
            data.TimeFrom = model.TimeFrom;
            data.TimeTo = model.TimeTo;
            data.JyotishId = model.JyotishId;
            data.TimeDuration = model.TimeDuration;
            data.Status = "Vacant";
            _context.AppointmentSlots.Add(data);*/
            if (_context.SaveChanges() > 0) { return "Successful"; }
            else { return "Data Not Saved"; }
        }

        public string UpdateAppointmentSlot(AppointmentSlotViewModel model)
        {
            if (model.Date < DateTime.Now)
            { return "Invalid Date"; }

            var Jyotish = _context.JyotishRecords.Where(x => x.Id == model.JyotishId).FirstOrDefault();
            if (Jyotish == null) { return "Invalid Jyotish"; }

            var slot = _context.AppointmentSlots.Where(x => x.Id == model.Id).Where(x=>x.JyotishId == model.JyotishId).FirstOrDefault();
            if (slot == null)
            {
                return "Invalid Data";
            }
            if(slot.Status == "Booked")
            { return "Slot Has Been Booked It Can't Be Updated."; }

            slot.TimeFrom = model.TimeFrom;
            slot.TimeTo = model.TimeTo;
           
            slot.TimeDuration = model.TimeDuration;
            slot.Status = "Vacant";
            _context.AppointmentSlots.Update(slot);
            if (_context.SaveChanges() > 0) { return "Successful"; }
            else { return "Data Not Saved"; }
        }


        public string DeleteAppointmentSlot(int Id )
        {
            var slot = _context.AppointmentSlots.Where(x => x.Id == Id).FirstOrDefault();
            if(slot == null) { return "Invalid Id"; }

            _context.AppointmentSlots.Remove(slot);
            if (_context.SaveChanges() > 0) { return "Successful"; }
            else { return "Internal Server Error."; }
        }

        public List<AppointmentSlotModel> GetAllAppointmentSlot(int Id)
        {
            var Jyotish = _context.JyotishRecords.Where(x => x.Id == Id).FirstOrDefault();
            if (Jyotish == null) { return null; }

            var slots = _context.AppointmentSlots.Where(x => x.JyotishId == Id).Where(y=>y.Date >= DateTime.Now).ToList();
            return slots.Count > 0 ? slots : null;
        }
    }
}
