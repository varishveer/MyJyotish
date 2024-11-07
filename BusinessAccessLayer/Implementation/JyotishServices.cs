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
            existingRecord.ApprovedStatus = "Complete";



            if (model.ProfileImageUrl !=null)
            {

                var ProfileGuid = Guid.NewGuid().ToString();
                var SqlPath = "wwwroot/Images/Jyotish/" + ProfileGuid + model.ProfileImageUrl.FileName;
                var ProfilePath = Path.Combine(_uploadDirectory, SqlPath);
                using (var stream = new FileStream(ProfilePath, FileMode.Create))
                {
                    model.ProfileImageUrl.CopyTo(stream);
                }
                existingRecord.ProfileImageUrl = "Images/Jyotish/" + ProfileGuid + model.ProfileImageUrl.FileName;

            }
            else
            {
                existingRecord.ProfileImageUrl = existingRecord.ProfileImageUrl;
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

           
            DateTime today = DateTime.Now;
            DateTime yesterday = today.AddDays(-1); 
            
            DateTime yesterdayDateOnly = yesterday.Date;

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

      

            var Records = allRecords.Where(x => x.Date > yesterdayDateOnly).ToList();

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
            if (DateTime.Compare(model.Date,DateTime.Now)<0)
            { return "Invalid Date"; }

            var Jyotish = _context.JyotishRecords.Where(x => x.Id == model.JyotishId).FirstOrDefault();
            var slots = _context.AppointmentSlots.Where(x => x.Id == model.JyotishId).Select(e=>e.Date).ToList();

            if (Jyotish == null) { return "Invalid Jyotish"; }
            if (DateTime.Compare(model.Date, model.DateTo) <= 90)
            {
                for (DateTime date = model.Date; date <= model.DateTo; date = date.AddDays(1))
                {
                    if (Jyotish.TimeFrom != null && Jyotish.TimeTo != null && !slots.Contains(date))
                    {
                        for (TimeOnly time = (TimeOnly)Jyotish.TimeFrom; time <= (TimeOnly)Jyotish.TimeTo; time = time.AddMinutes(model.TimeDuration))
                        {
                            AppointmentSlotModel data = new AppointmentSlotModel();
                            data.Date = model.Date;
                            data.TimeFrom = time;
                            data.TimeTo = time.AddMinutes(model.TimeDuration);
                            data.JyotishId = model.JyotishId;
                            data.TimeDuration = model.TimeDuration;
                            data.Status = "Vacant";
                            if (model.saturday == 1)
                            {
                                if (date.DayOfWeek == DayOfWeek.Saturday)
                                {

                                    data.ActiveStatus = 0;
                                }
                                else

                                {
                                    data.ActiveStatus = 1;

                                }
                            }
                            else
                            {
                                data.ActiveStatus = 1;

                            }
                            if (model.sunday == 2)
                            {
                                if (date.DayOfWeek == DayOfWeek.Sunday)
                                {

                                    data.ActiveStatus = 0;

                                }
                                else
                                {
                                    data.ActiveStatus = 1;

                                }
                            }
                            else
                            {
                                data.ActiveStatus = 1;

                            }

                            if (model.skipDate != null && model.skipDate.ToString()!= "1001-01-01")
                            {
                                if (DateTime.Compare((DateTime)model.skipDate, date) == 0)
                                {
                                    data.ActiveStatus = 0;

                                }
                                else
                                {
                                    data.ActiveStatus = 1;

                                }
                            }

                            if (model.saturday == 0 && model.sunday == 0 & model.skipDate == null)
                            {

                                data.ActiveStatus = 1;
                            }
                            _context.AppointmentSlots.Add(data);

                        }
                    }
                }
            }
            if (_context.SaveChanges() > 0) { return "Successful"; }
            else { return "Data Not Saved"; }
        }

        public string RemoveSlotWithskipDates(AppointmentSlotViewModel model)
        {
            var Jyotish = _context.JyotishRecords.Where(x => x.Id == model.JyotishId).FirstOrDefault();
            var slotsId = _context.AppointmentSlots.Where(x => x.JyotishId == model.JyotishId && x.ActiveStatus==1).Select(e=>e.Id).ToList();

            if (Jyotish == null) { return "Invalid Jyotish"; }
            bool checkSkipDate = true;
            
                for(var i=0;i < slotsId.Count; i++)
                {
                    var slots = _context.AppointmentSlots.Where(x => x.Id == slotsId[i]).FirstOrDefault();
                    if (slots != null)
                    {
                    if (model.saturday == 1)
                    {
                        if (slots.Date.DayOfWeek == DayOfWeek.Saturday)
                        {
                            slots.ActiveStatus = 0;
                            _context.AppointmentSlots.Update(slots);
                        }
                    }
                    if (model.sunday == 2)
                    {
                        if (slots.Date.DayOfWeek == DayOfWeek.Sunday)
                        {
                            slots.ActiveStatus = 0;
                            _context.AppointmentSlots.Update(slots);
                        }
                    }
                    if (model.skipDate!=null && checkSkipDate)
                    {
                        if (DateTime.Compare((DateTime)model.skipDate, slots.Date)==0)
                        {
                            slots.ActiveStatus = 0;
                            _context.AppointmentSlots.Update(slots);
                            checkSkipDate = false;
                          
                        }
                    }
                }
            }

            if (_context.SaveChanges() > 0)
                {
                    return "Changes applied successfully";
                }
                else
                {
                    return "There is some problem while applied changes";

                }

            
        }
            //add wallet
            public string AddWallet(JyotishWalletViewmodel pr)
        {
            var Jyotish = _context.JyotishRecords.Where(x => x.Id == pr.jyotishId).FirstOrDefault();
            if (Jyotish == null) { return null; }
            var JyotishWallets = _context.JyotishWallets.Where(x => x.jyotishId == pr.jyotishId).FirstOrDefault();
            if (JyotishWallets == null)
            {
                jyotishWallet jw = new jyotishWallet
                {
                    jyotishId = pr.jyotishId,
                    WalletAmount = pr.WalletAmount,
                    status = 1
                };
                _context.JyotishWallets.Add(jw);
                if (_context.SaveChanges() > 0) { return "Successful"; }
                else { return "Data Not Saved"; }
            }
            else
            {
                JyotishWallets.WalletAmount += pr.WalletAmount;
                _context.JyotishWallets.Update(JyotishWallets);
                if (_context.SaveChanges() > 0) { return "Successful"; }
                else { return "Data Not Saved"; }
            }
        }
         public long GetWallet(int JyotishId)
        {
            var Jyotish = _context.JyotishWallets.Where(x => x.jyotishId == JyotishId).FirstOrDefault();
            if ( Jyotish == null) { return 0; }
            else
            {
                return Jyotish.WalletAmount;
            }
        }


        //add wallet History
        public string AddWalletHistory(WalletHistoryViewmodel pr)
        {
            var Jyotish = _context.JyotishRecords.Where(x => x.Id == pr.JId).FirstOrDefault();
            if (Jyotish == null) { return null; }
            Random rnd = new Random();
            var rndnum = rnd.Next(100, 999);
            var paymentId = Jyotish.Name + DateTime.Now.ToString("ddMMyyyy")+ pr.JId + rndnum;
            var paymentUnqId = _context.WalletHistroy.Where(x => x.PaymentId == paymentId).FirstOrDefault();
            if (paymentUnqId != null)
            {
                paymentId=paymentId + 1;
            }
            WalletHistoryModel jw = new WalletHistoryModel
            {
                JId = pr.JId,
                amount = pr.amount,
                PaymentStatus = pr.PaymentStatus,
                PaymentAction = pr.PaymentAction,
                date = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"),
                PaymentId = paymentId,
                status = 1,
                PaymentBy="jyotish",
                PaymentFor=pr.PaymentFor,
                UId=pr.UId!=0?pr.UId:null
            };
            _context.WalletHistroy.Add(jw);
            if (_context.SaveChanges() > 0) { return "Successful"; }
            else { return "Data Not Saved"; }

        }
        public dynamic GetWalletHistory(int JyotishId)
        {
            var Jyotish =(from wallet in _context.WalletHistroy join user in _context.Users on wallet.UId equals user.Id into userGroup
                          from user in userGroup.DefaultIfEmpty()
                          where wallet.JId == JyotishId
                          orderby wallet.Id descending
                          select new
                          {
                              UserName= wallet.UId != null ? user.Name : null,
                              paymentDate=wallet.date,
                              amount=wallet.amount,
                              paymentId=wallet.PaymentId,
                              paymentBy=wallet.PaymentBy,
                              paymentAction=wallet.PaymentAction,
                              profile= wallet.UId != null ? user.ProfilePictureUrl : null,
                              paymentFor=wallet.PaymentFor,
                              paymentStatus=wallet.PaymentStatus,
                              Id=wallet.Id
                          }
                          
                          );

            return Jyotish;
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
            var result = (from slot in _context.AppointmentSlots
                          where slot.JyotishId == id && slot.ActiveStatus == 1
                          group slot by slot.Date into g
                                                   
                          select new AppointmentSlotUserViewModel
                          {
                              Date = g.Key, // Date is the grouping key
                              SlotList = (from s in g
                                          select new AppointmentSlotDateUserViewModel
                                          {
                                              Id = s.Id,
                                              TimeDuration = s.TimeDuration,
                                              TimeFrom = s.TimeFrom,
                                              TimeTo = s.TimeTo,
                                              JyotishId = s.JyotishId,
                                              Status = s.Status
                                          }).ToList()
                                          
                          }                    
                          ).ToList();

            return result;
        } 
        public dynamic GetTodayAppointment(int jyotishId)
        {
            var result = (
                from appointmentRecords in _context.AppointmentRecords
                join user in _context.Users
                on appointmentRecords.UserId equals user.Id
                join slots in _context.AppointmentSlots on appointmentRecords.SlotId equals slots.Id
                where (appointmentRecords.JyotishId==jyotishId
                && DateTime.Compare(slots.Date.Date,DateTime.Now.Date)==0)
                select new
                {
                    appId = appointmentRecords.Id,
                    uId = appointmentRecords.UserId,
                    userName = user.Name,
                    userMobile = user.Mobile,
                    problem = appointmentRecords.Problem,
                    userEmail=user.Email,
                    userProfile=user.ProfilePictureUrl,
                    appoinDate=slots.Date,
                    appoinTimeFrom=slots.TimeFrom,
                    appoinTimeTo=slots.TimeTo

                }
                );

           
            return result;
        }

        public string AddProblemSolution(ProblemSolutionViewModel model)
        {
            if (model == null )
            {
                return "No data provided";
            }


            var appointment = _context.AppointmentRecords
                                         .FirstOrDefault(x => x.Id == model.AppointmentId);
            if (appointment == null)
            {
                return "Invalid Data: AppointmentId does not exist.";
            }
            if (appointment.Date > DateTime.Now)
            {
                return "Appointment is Upcoming";
            }
            if (model.Problem.Length > 10 || model.Solution.Length > 10) 
            {
                return "Problem or Solution Limit Exceed";
            }

            var isValid = _context.ProblemSolution.Where(x => x.AppointmentId == model.AppointmentId).FirstOrDefault();
            if(isValid != null)
            {
                return "Record Already Present.";
            }
            string problems = "";
            string Solutions = "";
            foreach (var it in model.Problem)
            {
                problems += it + "$%^";
            }

            foreach (var it in model.Solution)
            {
                Solutions += it + "$%^";
            }
            ProblemSolutionModel data = new ProblemSolutionModel
            {
                AppointmentId = appointment.Id,
                Problem = problems,
                Solution = Solutions
            };
            _context.ProblemSolution.Add(data);
            return _context.SaveChanges() > 0 ? "Successful" : "Internal Server Error";
        }


        public ProblemSolutionJyotishGetViewModel GetProblemSolution(int appointmentId)
        {
           

            var Appointment = _context.AppointmentRecords.Where(x => x.Id == appointmentId).FirstOrDefault();
            if (appointmentId == 0 || Appointment ==null)
            {
                return null;
            }

            var User = _context.Users.Where(x => x.Id == Appointment.UserId).FirstOrDefault(); 

           var record = _context.ProblemSolution.Where(x => x.AppointmentId == appointmentId).FirstOrDefault();
            if(record == null)
            {
                return null;
            }

            string problemString = record.Problem;
            string SolutionString = record.Solution;
          
            string[] problemArray = problemString.Split(new[] { "$%^" }, StringSplitOptions.RemoveEmptyEntries);
            string[] SolutionArray = SolutionString.Split(new[] { "$%^" }, StringSplitOptions.RemoveEmptyEntries);


            ProblemSolutionJyotishGetViewModel Result = new ProblemSolutionJyotishGetViewModel
            {

                Id = record.Id,
                AppointmentId = record.AppointmentId,
                UserId = User.Id,
                UserName = User.Name,
                
                Problem = problemArray,
                Solution = SolutionArray

            };
            return Result;
        }


        public List<ProblemSolutionJyotishGetAllViewModel> GetAllProblemSolution(int jyotishId)
        {
            var data = _context.ProblemSolution
                .Include(x => x.Appointment)
                    .ThenInclude(y => y.UserRecord)
                .Where(x => x.Appointment.JyotishId == jyotishId)
                .Select(x => new
                {
                    ProblemSolution = x,
                    AppointmentSlot = _context.AppointmentSlots.FirstOrDefault(s => s.Id == x.Appointment.SlotId)
                })
                .Select(x => new ProblemSolutionJyotishGetAllViewModel
                {
                    Id = x.ProblemSolution.Id,
                    UserName = x.ProblemSolution.Appointment.UserRecord.Name,
                    UserId = x.ProblemSolution.Appointment.UserRecord.Id,
                    Date = x.AppointmentSlot != null ? DateOnly.FromDateTime(x.AppointmentSlot.Date) : DateOnly.MinValue,
                    Time = x.AppointmentSlot != null ? x.AppointmentSlot.TimeFrom : TimeOnly.MinValue,
                    AppointmentId = x.ProblemSolution.AppointmentId
                })    
                .ToList();

            return data;
        }

        public ProblemSolutionJyotishDetailViewModel GetProblemSolutionDetail(int id)
        {
            var data = _context.ProblemSolution
                .Where(x => x.Id == id)
                .Include(x => x.Appointment)
                    .ThenInclude(y => y.UserRecord)
                .Select(x => new
                {
                    ProblemSolution = x,
                    AppointmentSlot = _context.AppointmentSlots.FirstOrDefault(s => s.Id == x.Appointment.SlotId)
                })
                .Select(x => new ProblemSolutionJyotishDetailViewModel
                {
                    Id = x.ProblemSolution.Id,
                    UserName = x.ProblemSolution.Appointment.UserRecord.Name,
                    UserId = x.ProblemSolution.Appointment.UserRecord.Id,
                    Date = x.AppointmentSlot != null ? DateOnly.FromDateTime(x.AppointmentSlot.Date) : DateOnly.MinValue,
                    Time = x.AppointmentSlot != null ? x.AppointmentSlot.TimeFrom: TimeOnly.MinValue,
                    AppointmentId = x.ProblemSolution.AppointmentId,
                    Problem = x.ProblemSolution.Problem != null ?
                        x.ProblemSolution.Problem.Split(new[] { "$%^" }, StringSplitOptions.RemoveEmptyEntries) : Array.Empty<string>(),
                    Solution = x.ProblemSolution.Solution != null ?
                        x.ProblemSolution.Solution.Split(new[] { "$%^" }, StringSplitOptions.RemoveEmptyEntries) : Array.Empty<string>()
                })
                .FirstOrDefault();

            return data;
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

            string problems = "";
            string Solutions = "";
            foreach (var it in model.Problem)
            {
                problems += it + "$%^";
            }

            foreach (var it in model.Solution)
            {
                Solutions += it + "$%^";
            }
            existingRecord.Problem = problems;
            existingRecord.Solution = Solutions;
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

        public string AddUserAttachment(JyotishUserAttachmentViewModel model)
        {
            try
            {
                // Validate model data
                if (model.JyotishId <= 0 || model.UserId <= 0 || model.ImageUrl == null || !model.ImageUrl.Any()|| model.ImageUrl.Count>5)
                {
                    return "Invalid data provided for the attachment.";
                }


                // Keep Titles synchronized with Image URLs
                for (int i = 0; i < model.ImageUrl.Count; i++)
                {
                    var attachmentRecord = new JyotishUserAttachmentModel
                    {
                        JyotishId = model.JyotishId,
                        UserId = model.UserId,
                        Image = model.ImageUrl[i],
                        Title = i < model.Title.Count ? model.Title[i] : null // Use Title if available
                    };

                    _context.JyotishUserAttachmentRecord.Add(attachmentRecord);
                }

                return _context.SaveChanges() > 0 ? "Successful" : "Failed to save attachments.";
            }
            catch (Exception)
            {
                return "Internal Server Error";
            }
        }

        public List<JyotishUserAttachmentJyotishViewModel> GetAllUserAttachments(int Id)
        {
            if (Id <= 0)
            {
                return new List<JyotishUserAttachmentJyotishViewModel>();
            }

            var record = _context.JyotishUserAttachmentRecord
                                 .Where(x => x.JyotishId == Id)
                                 .ToList();

            var result = record.Select(x => new JyotishUserAttachmentJyotishViewModel
            {
                Id = x.Id,
                JyotishId = x.JyotishId,
                UserId = x.UserId,
                UserName = _context.Users.Where(y=>y.Id == x.UserId).Select(z=>z.Name).FirstOrDefault(),
                UserEmail = _context.Users.Where(y => y.Id == x.UserId).Select(z => z.Email).FirstOrDefault(),
                Title = x.Title,
                Image = x.Image
            }).ToList();

            return result;
        }



        public string UpdateUserAttachment(JyotishUserAttachmentJyotishUpdateViewModel model)
        {
            try
            {
                // Retrieve existing record from the database
                var existingRecord = _context.JyotishUserAttachmentRecord.FirstOrDefault(x => x.Id == model.Id);
                if (existingRecord == null)
                {
                    return "Record not found";
                }

                // Keep existing image path if no new file is uploaded
                string accessPath = existingRecord.Image;

                // Handle new image upload
                if (model.Image != null)
                {
                    // Ensure the uploads directory exists
                    string uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Jyotish/ProblemSolution");
                    if (!Directory.Exists(uploadsFolderPath))
                    {
                        Directory.CreateDirectory(uploadsFolderPath);
                    }

                    // Generate unique filename
                    string uniqueString = Guid.NewGuid().ToString("N").Substring(0, 10);
                    string filePath = Path.Combine(uploadsFolderPath, uniqueString + Path.GetFileName(model.Image.FileName));
                    accessPath = Path.Combine("/Images/Jyotish/ProblemSolution", uniqueString + Path.GetFileName(model.Image.FileName));

                    // Save the new image
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        model.Image.CopyTo(stream);
                    }
                }

                // Update the record with new data
                existingRecord.Title = model.Title;
                existingRecord.Image = accessPath;

                // Save changes to the database
                _context.SaveChanges();

                return "Successful"; // Return success message
            }
            catch (Exception)
            {
                return "Internal Server Error"; // Handle errors
            }
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
