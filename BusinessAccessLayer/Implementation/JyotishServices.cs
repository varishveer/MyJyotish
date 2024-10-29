using BusinessAccessLayer.Abstraction;
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

        //add wallet
        public string AddWallet(JyotishWalletViewmodel pr)
        {
            var Jyotish = _context.JyotishRecords.Where(x => x.Id == pr.jyotishId).FirstOrDefault();
            if (Jyotish == null) { return null; }
            jyotishWallet jw=new jyotishWallet
            {
                jyotishId = pr.jyotishId,
                WalletAmount = pr.WalletAmount,
                status = 1
            };
            _context.JyotishWallets.Add(jw);
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

        public string AddProblemSolution(ProblemSolutionViewModel model)
        {
            var User = _context.Users.Where(x => x.Email == model.Email).FirstOrDefault();
            var Jyotish = _context.JyotishRecords.Where(x => x.Id == model.JyotishId).FirstOrDefault();
            if (User == null || Jyotish ==null) { return "Invalid Data"; }

            var Appointment = _context.AppointmentRecords.Where(x => x.Id == model.AppointmentId).Where(y => y.UserId == User.Id).Where(y => y.JyotishId == Jyotish.Id).FirstOrDefault();
            if(Appointment == null) { return "Invalid Data"; }
            DateTime currentDateTime = DateTime.Now;
            ProblemSolutionModel data = new ProblemSolutionModel();
            data.UserId = User.Id;
            data.JyotishId = Jyotish.Id;
            data.AppointmentId = Appointment.Id;
            data.Date = DateTime.Now;
            data.Time = TimeOnly.FromDateTime(currentDateTime);


            if (model.Image1 != null)
            {
                const long MaxFileSize = 5 * 1024 * 1024; // 5 MB
                var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
                if(ValidateFile(model.Image1, MaxFileSize, allowedExtensions) ==  false)
                { return "Invalid File."; }
                var ProfileGuid = Guid.NewGuid().ToString();
                var SqlPath = "wwwroot/Images/Jyotish/ProblemSolution/" + ProfileGuid + model.Image1.FileName;
                var ProfilePath = Path.Combine(_uploadDirectory, SqlPath);
                using (var stream = new FileStream(ProfilePath, FileMode.Create))
                {
                    model.Image1.CopyTo(stream);
                }
                data.Image1 = "/Images/Jyotish/ProblemSolution/" + ProfileGuid + model.Image1.FileName;
            }
            if (model.Image2 != null)
            {
                const long MaxFileSize = 5 * 1024 * 1024; // 5 MB
                var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
                if (ValidateFile(model.Image2, MaxFileSize, allowedExtensions) == false)
                { return "Invalid File."; }
                var ProfileGuid = Guid.NewGuid().ToString();
                var SqlPath = "wwwroot/Images/Jyotish/ProblemSolution/" + ProfileGuid + model.Image2.FileName;
                var ProfilePath = Path.Combine(_uploadDirectory, SqlPath);
                using (var stream = new FileStream(ProfilePath, FileMode.Create))
                {
                    model.Image2.CopyTo(stream);
                }
                data.Image2 = "/Images/Jyotish/ProblemSolution/" + ProfileGuid + model.Image2.FileName;
            }
            if (model.Image3 != null)
            {
                const long MaxFileSize = 5 * 1024 * 1024; // 5 MB
                var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
                if (ValidateFile(model.Image3, MaxFileSize, allowedExtensions) == false)
                { return "Invalid File."; }
                var ProfileGuid = Guid.NewGuid().ToString();
                var SqlPath = "wwwroot/Images/Jyotish/ProblemSolution/" + ProfileGuid + model.Image3.FileName;
                var ProfilePath = Path.Combine(_uploadDirectory, SqlPath);
                using (var stream = new FileStream(ProfilePath, FileMode.Create))
                {
                    model.Image3.CopyTo(stream);
                }
               data.Image3 = "/Images/Jyotish/ProblemSolution/" + ProfileGuid + model.Image3.FileName;
            }

            if (model.Problem1 != null)
            {
                data.Problem1 = model.Problem1;
            }
            if (model.Solution1 != null)
            {
                data.Solution1 = model.Solution1;
            }

            if (model.Problem2 != null)
            {
                data.Problem2 = model.Problem2;
            }
            if (model.Solution2 != null)
            {
                data.Solution2 = model.Solution2;
            }

            if (model.Problem3 != null)
            {
                data.Problem3 = model.Problem3;
            }
            if (model.Solution3 != null)
            {
                data.Solution3 = model.Solution3;
            }

            _context.ProblemSolution.Add(data);
            if(_context.SaveChanges()>0)
            { return "Successful"; }
            else { return "Internal Server Error."; }
         
        }

        public List<ProblemSolutionGetAllViewModel> GetAllProblemSolution(int Id)
        {
            if (Id == 0)
            {
                return null;
            }

         
            var Data = _context.ProblemSolution
                                .Where(x => x.JyotishId == Id)
                                .Select(x => new ProblemSolutionGetAllViewModel
                                {
                                    Id = x.Id,
                                    JyotishId = x.JyotishId,
                                    AppointmentId = x.AppointmentId,
                                    Date = x.Date,
                                    Time = x.Time,
                                    Problem1 = x.Problem1,
                                    Solution1 = x.Solution1,
                                    Problem2 = x.Problem2,
                                    Solution2 = x.Solution2,
                                    Problem3 = x.Problem3,
                                    Solution3 = x.Solution3,
                                    Image1 = x.Image1,
                                    Image2 = x.Image2,
                                    Image3 = x.Image3,
                                  
                                    Name = _context.Users.Where(u => u.Id == x.UserId).Select(u => u.Name).FirstOrDefault(),
                                    Email = _context.Users.Where(u => u.Id == x.UserId).Select(u => u.Email).FirstOrDefault()
                                })
                                .ToList();

            return Data;
        }


        public string UpdateProblemSolution(ProblemSolutionViewModel model)
        {
            var User = _context.Users.Where(x => x.Email == model.Email).FirstOrDefault();
            var Jyotish = _context.JyotishRecords.Where(x => x.Id == model.JyotishId).FirstOrDefault();
            if (User == null || Jyotish == null) { return "Invalid Data"; }

            var Appointment = _context.AppointmentRecords.Where(x => x.Id == model.AppointmentId).Where(y => y.UserId == User.Id).Where(y => y.JyotishId == Jyotish.Id).FirstOrDefault();
            if (Appointment == null) { return "Invalid Data"; }

            var data = _context.ProblemSolution.Where(x => x.Id == model.Id).FirstOrDefault();
            if(data == null) { return "Invalid Data"; }
            data.UserId = User.Id;
            data.JyotishId = Jyotish.Id;
            data.AppointmentId = Appointment.Id;

         


            if (model.Image1 != null)
            {
                const long MaxFileSize = 5 * 1024 * 1024; // 5 MB
                var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
                if (ValidateFile(model.Image1, MaxFileSize, allowedExtensions) == false)
                { return "Invalid File."; }
                var ProfileGuid = Guid.NewGuid().ToString();
                var SqlPath = "wwwroot/Images/Jyotish/ProblemSolution/" + ProfileGuid + model.Image1.FileName;
                var ProfilePath = Path.Combine(_uploadDirectory, SqlPath);
                using (var stream = new FileStream(ProfilePath, FileMode.Create))
                {
                    model.Image1.CopyTo(stream);
                }
                data.Image1 = "Images/Jyotish/ProblemSolution/" + ProfileGuid + model.Image1.FileName;
            }
            if (model.Image2 != null)
            {
                const long MaxFileSize = 5 * 1024 * 1024; // 5 MB
                var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
                if (ValidateFile(model.Image2, MaxFileSize, allowedExtensions) == false)
                { return "Invalid File."; }
                var ProfileGuid = Guid.NewGuid().ToString();
                var SqlPath = "wwwroot/Images/Jyotish/ProblemSolution/" + ProfileGuid + model.Image2.FileName;
                var ProfilePath = Path.Combine(_uploadDirectory, SqlPath);
                using (var stream = new FileStream(ProfilePath, FileMode.Create))
                {
                    model.Image2.CopyTo(stream);
                }
                data.Image2 = "Images/Jyotish/ProblemSolution/" + ProfileGuid + model.Image2.FileName;
            }
            if (model.Image3 != null)
            {
                const long MaxFileSize = 5 * 1024 * 1024; // 5 MB
                var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
                if (ValidateFile(model.Image3, MaxFileSize, allowedExtensions) == false)
                { return "Invalid File."; }
                var ProfileGuid = Guid.NewGuid().ToString();
                var SqlPath = "wwwroot/Images/Jyotish/ProblemSolution/" + ProfileGuid + model.Image3.FileName;
                var ProfilePath = Path.Combine(_uploadDirectory, SqlPath);
                using (var stream = new FileStream(ProfilePath, FileMode.Create))
                {
                    model.Image3.CopyTo(stream);
                }
                data.Image3 = "Images/Jyotish/ProblemSolution/" + ProfileGuid + model.Image3.FileName;
            }

            if (model.Problem1 != null)
            {
                data.Problem1 = model.Problem1;
            }
            if (model.Solution1 != null)
            {
                data.Solution1 = model.Solution1;
            }

            if (model.Problem2 != null)
            {
                data.Problem2 = model.Problem2;
            }
            if (model.Solution2 != null)
            {
                data.Solution2 = model.Solution2;
            }

            if (model.Problem3 != null)
            {
                data.Problem3 = model.Problem3;
            }
            if (model.Solution3 != null)
            {
                data.Solution3 = model.Solution3;
            }

            _context.ProblemSolution.Update(data);
            if (_context.SaveChanges() > 0)
            { return "Successful"; }
            else { return "Internal Server Error."; }

        }

        public string DeleteProblemSolution(int Id)
        {
            if(Id == null) { return "Invalid Id"; }
            var data = _context.ProblemSolution.Where(x => x.Id == Id).FirstOrDefault();
            if(data == null) { return "Invalid Id"; }

            _context.ProblemSolution.Remove(data);
            if (_context.SaveChanges() > 0) { return "Successful"; }
            else { return "Internal Server Error."; }
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
    }
}
