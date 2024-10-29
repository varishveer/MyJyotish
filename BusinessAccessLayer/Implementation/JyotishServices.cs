﻿using BusinessAccessLayer.Abstraction;
using DataAccessLayer.DbServices;
using DataAccessLayer.Migrations;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

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




        public List<AppointmentDetailViewModel> UpcomingAppointment(int Id)
        {
            // Fetch the Jyotish details
            var Jyotish = _context.JyotishRecords.Where(x => x.Id == Id).FirstOrDefault();
            if (Jyotish == null) { return null; }

            DateTime Today = DateTime.Now;

            // Fetch appointment records for the given Jyotish and filter by future dates
            var allRecords = _context.AppointmentRecords
     .Where(record => record.JyotishId == Id)
     .Join(_context.Users,
           record => record.UserId,
           user => user.Id,
           (record, user) => new { record, user })
     .Join(_context.AppointmentSlots,
           combined => combined.record.SlotId,
           slot => slot.Id,
           (combined, slot) => new AppointmentDetailViewModel
           {
               Id = combined.record.Id,
               UserName = combined.user.Name,
               UserMobile = combined.user.Mobile,
               UserEmail = combined.user.Email,
               UserId = combined.record.UserId,
               Problem = combined.record.Problem,
               Date = slot.Date,
               Time = slot.TimeFrom,
               Status = combined.record.Status,
               Amount = combined.record.Amount
           })
     .ToList();  // Retrieve all records for the Jyotish

            // Log the count of all records before filtering
            Console.WriteLine($"Total records for JyotishId {Id}: {allRecords.Count}");

            var Records = allRecords.Where(x => x.Date >= Today).ToList();

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
          
         
             appointment.Date = Slot.Date;

         
             appointment.JyotishId = model.JyotishId;
            appointment.SlotId = model.SlotId;
         

             if(User == null)
             {
                 ModelAccessLayer.Models.UserModel userModel = new ModelAccessLayer.Models.UserModel();
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


            
            
            
             
             appointment.JyotishId = Jyotish.Id;
             appointment.UserId = User.Id;
             appointment.Problem = model.Problem;
             appointment.Amount = Jyotish.AppointmentCharges;
            if (appointment.SlotId == model.SlotId)
            {
              
                appointment.Date = slot.Date;
            }
            

            appointment.SlotId = model.SlotId;
            appointment.Date = slot.Date;
         

            appointment.Status = "Upcomming";
             _context.AppointmentRecords.Update(appointment);

             if (_context.SaveChanges() > 0) { return "Successful"; }
             return "internal Server Error.";
         }

         public AppointmentDetailViewModel GetAppointment(int Id)
         {
            var Record = _context.AppointmentRecords
      .Where(x => x.Id == Id) // Filter by appointmentId
      .Join(_context.Users,
            record => record.UserId,
            user => user.Id,
            (record, user) => new { record, user })
      .Join(_context.AppointmentSlots,
            combined => combined.record.SlotId,
            slot => slot.Id,
            (combined, slot) => new AppointmentDetailViewModel
            {
                Id = combined.record.Id,
                UserName = combined.user.Name,
                UserMobile = combined.user.Mobile,
                UserEmail = combined.user.Email,
                UserId = combined.record.UserId,
                Problem = combined.record.Problem,
                Date = slot.Date,
                Time = slot.TimeFrom,
                Status = combined.record.Status,
                Amount = combined.record.Amount
            })
      .FirstOrDefault();
            if (Record == null) { return null; }
             else { return Record; }
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
            if(model.Image != null)
            {
                const long MaxFileSize = 5 * 1024 * 1024; // 5 MB
                var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
                if (ValidateFile(model.Image, MaxFileSize, allowedExtensions) == false)
                { return "Invalid File."; }
                var ProfileGuid = Guid.NewGuid().ToString();
                var SqlPath = "wwwroot/Images/Jyotish/Video" + ProfileGuid + model.Image.FileName;
                var ProfilePath = Path.Combine(_uploadDirectory, SqlPath);
                using (var stream = new FileStream(ProfilePath, FileMode.Create))
                {
                    model.Image.CopyTo(stream);
                }
                data.ImageUrl = "/Images/Jyotish/Video" + ProfileGuid + model.Image.FileName;
            }
            data.VideoTitle = model.VideoTitle;
            data.VideoUrl = model.VideoUrl;
            data.JyotishId = model.JyotishId;
            data.SerialNo = model.SerialNo;
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
                const long MaxFileSize = 5 * 1024 * 1024; // 5 MB
                var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
                if (ValidateFile(model.ImageUrl, MaxFileSize, allowedExtensions) == false)
                { return "Invalid File."; }
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
            data.SerialNo = model.SerialNo;
            _context.JyotishGallery.Add(data);
            if (_context.SaveChanges() > 0) { return "Successful"; }
            else { return "internal server Error"; }
        }
       

        public List<JyotishVideosModel> JyotishVideos(int Id)
        {
            var records = _context.JyotishVideos.Where(x => x.JyotishId == Id).OrderBy(x => x.SerialNo).ToList();
            return records;
        }
        public List<JyotishGalleryModel> JyotishGallery(int Id)
        {
            var records = _context.JyotishGallery.Where(x => x.JyotishId == Id).OrderBy(x => x.SerialNo).ToList();
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
            var record = _context.JyotishPaymentRecord.Where(x => x.JyotishId == Id).OrderBy(x=>x.Id).Reverse().ToList();
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
            slot.Date = model.Date;
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

        public List<AppointmentSlotUserViewModel> GetAllAppointmentSlot(int id)
        {
            var result = _context.AppointmentSlots.Where(x=>x.JyotishId == id)
                    .GroupBy(x => x.Date)
                    .Select(g => new AppointmentSlotUserViewModel
                    {
                        
                        Date = g.Key, // Date is the grouping key
                        SlotList = g.Select(x => new AppointmentSlotDateUserViewModel
                        {   Id = x.Id,
                            TimeDuration = x.TimeDuration,
                            TimeFrom = x.TimeFrom,
                            TimeTo = x.TimeTo,
                            JyotishId = x.JyotishId,
                            Status = x.Status
                        }).ToList()
                    })
                    .ToList();
            return result;
        }

        public string AddProblemSolution(ProblemSolutionViewModel[] models)
        {
        
            if (models == null || models.Length == 0)
            {
                return "No data provided";
            }

          

            foreach (var model in models)
            {
              
                var appointment = _context.AppointmentRecords
                                         .FirstOrDefault(x => x.Id == model.AppointmentId);
                if (appointment == null)
                {
                    return "Invalid Data: AppointmentId does not exist.";
                }

             
                ProblemSolutionModel data = new ProblemSolutionModel
                {
                    AppointmentId = appointment.Id,
                    Problem = model.Problem,
                    Solution = model.Solution
                };

              
                _context.ProblemSolution.Add(data);
            }

            return _context.SaveChanges() > 0 ? "Successful" : "Internal Server Error";
        }


        public List<ProblemSolutionModel> GetAllProblemSolution(int appointmentId)
        {
            if (appointmentId == 0)
            {
                return new List<ProblemSolutionModel>();
            }

           
            return _context.ProblemSolution.Where(x => x.AppointmentId == appointmentId).ToList();
        }


        public string UpdateProblemSolution(ProblemSolutionViewModel model)
        {
            if (model == null || model.Id == 0)
            {
                return "Invalid data provided";
            }

            var existingRecord = _context.ProblemSolution.FirstOrDefault(x => x.Id == model.Id);
            if (existingRecord == null)
            {
                return "Record not found";
            }
            
            existingRecord.Problem = model.Problem;
            existingRecord.Solution = model.Solution;
            existingRecord.AppointmentId = model.AppointmentId;

            return _context.SaveChanges() > 0 ? "successful" : "Update failed";
        }


        public string DeleteProblemSolution(int id)
        {
            if (id <= 0) // Check for invalid ID (should be greater than 0)
            {
                return "Invalid Id";
            }

            var data = _context.ProblemSolution.FirstOrDefault(x => x.Id == id);
            if (data == null)
            {
                return "Invalid Id"; // Return this message if no record found
            }

            _context.ProblemSolution.Remove(data);
            return _context.SaveChanges() > 0 ? "Successful" : "Internal Server Error.";
        }

        private bool ValidateFile(IFormFile file, long maxSize, string[] allowedExtensions)
        {
            return file.Length <= maxSize && allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower());
        }

        public List<AppointmentDetailViewModel> GetAllAppointment(int Id)
        {
            var Jyotish = _context.JyotishRecords.Where(x => x.Id == Id).FirstOrDefault();
            if (Jyotish == null) { return null; }



            var allRecords = _context.AppointmentRecords
                 .Where(record => record.JyotishId == Id)
                 .Join(_context.Users,
                       record => record.UserId,
                       user => user.Id,
                       (record, user) => new { record, user })
                 .Join(_context.AppointmentSlots,
                       combined => combined.record.SlotId,
                       slot => slot.Id,
                       (combined, slot) => new AppointmentDetailViewModel
                       {
                           Id = combined.record.Id,
                           UserName = combined.user.Name,
                           UserMobile = combined.user.Mobile,
                           UserEmail = combined.user.Email,
                           UserId = combined.record.UserId,
                           Problem = combined.record.Problem,
                           Date = slot.Date,
                           Time = slot.TimeFrom,
                           Status = combined.record.Status,
                           Amount = combined.record.Amount
                       })
                 .ToList();

            /*var Records = _context.AppointmentRecords
            .Join(_context.Users,
          record => record.UserId,
          user => user.Id,
          (record, user) => new { record, user })
            .Join(_context.AppointmentSlots,
          combined => combined.record.SlotId,
          slot => slot.Id,
          (combined, slot) => new AppointmentDetailViewModel
          {
              Id = combined.record.Id,
              UserName = combined.user.Name,
              UserMobile = combined.user.Mobile,
              UserEmail = combined.user.Email,
              UserId = combined.record.UserId,
              Problem = combined.record.Problem,
              Date = slot.Date,
              Time = slot.TimeFrom,
              Status = combined.record.Status,
              Amount = combined.record.Amount
          })
    .ToList();*/





            return allRecords;
        }

        public string AddUserAttachment(JyotishUserAttachmentViewModel[] models)
        {
            if (models == null || models.Length == 0)
            {
                return "No data provided.";
            }

            foreach (var model in models)
            {
                // Validate the model properties
                if (model.JyotishId <= 0 || model.UserId <= 0 || model.Image == null)
                {
                    return "Invalid data provided for one or more attachments.";
                }

                // Save the image file to the desired location
                string filePath = Path.Combine("/wwwroot/Images/Jyotish/ProblemSolution/", model.Image.FileName);
                string AccessPath = Path.Combine("/Images/Jyotish/ProblemSolution/", model.Image.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    model.Image.CopyTo(stream);
                }

                var attachmentRecord = new JyotishUserAttachmentModel
                {
                    JyotishId = model.JyotishId,
                    UserId = model.UserId,
                    Image = AccessPath 
                };

               
                _context.JyotishUserAttachmentRecord.Add(attachmentRecord);
            }

            return _context.SaveChanges() > 0 ? "Successful" : "Internal Server Error.";
        }

        public List<JyotishUserAttachmentModel> GetAllUserAttachments(int jyotishId)
        {
            if (jyotishId <= 0)
            {
                return new List<JyotishUserAttachmentModel>(); 
            }

            return _context.JyotishUserAttachmentRecord.Where(x => x.JyotishId == jyotishId).ToList();
        }



        public string UpdateUserAttachment(JyotishUserAttachmentViewModel model)
        {
            if (model == null || model.Id <= 0)
            {
                return "Invalid data provided.";
            }

            // Find the existing record
            var existingRecord = _context.JyotishUserAttachmentRecord.FirstOrDefault(x => x.Id == model.Id);
            if (existingRecord == null)
            {
                return "Record not found.";
            }

            // Update the properties
            existingRecord.JyotishId = model.JyotishId;
            existingRecord.UserId = model.UserId;

            // Handle file update if a new file is provided
            if (model.Image != null)
            {
                // Save the new image file
                string filePath = Path.Combine("/wwwroot/Images/Jyotish/ProblemSolution/", model.Image.FileName);
                string accessPath = Path.Combine("/Images/Jyotish/ProblemSolution/", model.Image.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    model.Image.CopyTo(stream);
                }

                existingRecord.Image = accessPath; // Update image path
            }

            return _context.SaveChanges() > 0 ? "Successful" : "Update failed.";
        }

        public string DeleteUserAttachment(int id)
        {
            if (id <= 0)
            {
                return "Invalid Id.";
            }

            // Find the existing record
            var attachment = _context.JyotishUserAttachmentRecord.FirstOrDefault(x => x.Id == id);
            if (attachment == null)
            {
                return "Attachment not found.";
            }

            // Optionally, delete the physical file if needed
            string filePath = Path.Combine("/wwwroot", attachment.Image.TrimStart('/'));
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            // Remove the record from the database
            _context.JyotishUserAttachmentRecord.Remove(attachment);

            return _context.SaveChanges() > 0 ? "Successful" : "Deletion failed.";
        }

    }
}
