using BusinessAccessLayer.Abstraction;
using BusinessAccessLayer.Abstraction;
using DataAccessLayer.DbServices;
using DataAccessLayer.Migrations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ModelAccessLayer.Models;
using ModelAccessLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserModel = ModelAccessLayer.Models.UserModel;
using UserWalletViewmodel = ModelAccessLayer.ViewModels.UserWalletViewmodel;

namespace BusinessAccessLayer.Implementation
{
    public class UserServices : IUserServices
    {
        private readonly ApplicationContext _context;
        public UserServices(ApplicationContext context)
        {
            _context = context;
        }





        public List<JyotishModel> GetAstroListCallChat(string ListName)
        {
            if (ListName == "Chat")
            {
                var record = _context.JyotishRecords.Where(x => x.Chat == true).ToList();
                return record;
            }
            else if (ListName == "Call")
            {
                var record = _context.JyotishRecords.Where(x => x.Call == true).ToList();
                return record;
            }
            else { return null; }
        }

        /*  public List<PoojaCategoryModel> GetAllPoojaCategory()
          {
              var record = _context.PoojaCategory.ToList();
              if (record == null)
              { return null; }
              else { return record; }
          }*/

        /* public List<PoojaRecordModel> GetPoojaList(int id)
         {
             var record = _context.PoojaRecord.Where(x => x.PoojaCategoryId == id).ToList();
             if (record == null)
             { return null; }
             else { return record; }
         }*/

        public PoojaRecordModel GetPoojaDetail(int PoojaId)
        {
            var record = _context.PoojaRecord.Where(x => x.Id == PoojaId).FirstOrDefault();
            if (record == null)
            { return null; }
            else { return record; }
        }


        public List<JyotishModel> TopAstrologer(string City)
        {

            var records = _context.JyotishRecords.Where(a => a.Role == "Jyotish").Where(x => x.City.Contains(City)).ToList();
            if (records.Count == 0)
            {
                records = _context.JyotishRecords.Where(x => x.Role == "Jyotish").Where(x => x.Country.Contains("India")).ToList();
            }
            return records;
        }
        public List<JyotishModel> AllAstrologer()
        {
            List<JyotishModel> record = _context.JyotishRecords.Where(x => x.Role == "Jyotish").ToList();
            return record;
        }

        public List<City> selecAllCity()
        {
            var record = _context.Cities.ToList();
            return record;
        }



        public JyotishProfileViewModel AstrologerProfile(int Id)
        {
            // Fetch the Jyotish record based on the provided Id and status
            var jyotishRecord = _context.JyotishRecords
                                         .Where(x => x.Role == "Jyotish" && x.Id == Id)
                                         .FirstOrDefault();

            // Return null if no record found
            if (jyotishRecord == null)
            {
                return null;
            }

            // Fetch videos and gallery related to the Jyotish record
            var videos = _context.JyotishVideos
                                 .Where(x => x.JyotishId == Id)
                                 .Select(video => new JyotishVideosUserViewModel
                                 {

                                     Id = video.Id,
                                     VideoUrl = video.VideoUrl,
                                     VideoTitle = video.VideoTitle,
                                     ImageUrl = video.ImageUrl,
                                     SerialNo = video.SerialNo

                                 }).OrderBy(order => order.SerialNo)
                                 .ToArray();

            var gallery = _context.JyotishGallery
                                  .Where(x => x.JyotishId == Id)
                                  .Select(image => new JyotishGalleryUserViewModel
                                  {
                                      Id = image.Id,
                                      ImageUrl = image.ImageUrl,
                                      ImageTitle = image.ImageTitle,
                                      SerialNo = image.SerialNo
                                  }).OrderBy(order => order.SerialNo)
                                  .ToArray();
            var achievementsArray = jyotishRecord.AwordsAndAchievement?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                  .Select(a => a.Trim())
                                                  .ToArray();

            var specializationArray = jyotishRecord.Specialization?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                 .Select(a => a.Trim())
                                                 .ToArray();
            var profileViewModel = new JyotishProfileViewModel
            {
                Id = jyotishRecord.Id,
                Email = jyotishRecord.Email,
                Mobile = jyotishRecord.Mobile,
                Role = jyotishRecord.Role,
                Name = jyotishRecord.Name,
                Gender = jyotishRecord.Gender,
                Language = jyotishRecord.Language,
                Expertise = jyotishRecord.Expertise,
                Country = jyotishRecord.Country,
                State = jyotishRecord.State,
                City = jyotishRecord.City,

                DateOfBirth = jyotishRecord.DateOfBirth,
                ProfileImageUrl = jyotishRecord.ProfileImageUrl,
                Status = jyotishRecord.ApprovedStatus,
                Otp = jyotishRecord.Otp,
                Experience = jyotishRecord.Experience,

                Call = jyotishRecord.Call,
                CallCharges = jyotishRecord.CallCharges,
                Chat = jyotishRecord.Chat,
                ChatCharges = jyotishRecord.ChatCharges,
                AppointmentCharges = jyotishRecord.AppointmentCharges,
                Address = jyotishRecord.Address,
                TimeTo = jyotishRecord.TimeTo,
                TimeFrom = jyotishRecord.TimeFrom,
                About = jyotishRecord.About,
                AwordsAndAchievement = achievementsArray,
                Specialization = specializationArray,
                Videos = videos,
                Gallery = gallery
            };

            return profileViewModel;
        }

        public List<JyotishModel> FilterAstrologer(FilterModel fm)
        {
            int expFrom = 0;
            int expTo = 0;

            if (!string.IsNullOrEmpty(fm.experience))
            {
                var experienceRange = fm.experience.Split(',');

                // Ensure the array has at least two elements to avoid IndexOutOfRangeException
                if (experienceRange.Length >= 2)
                {
                    expFrom = Convert.ToInt32(experienceRange[0]);
                    expTo = Convert.ToInt32(experienceRange[1]);
                }
            }

            // Call the stored procedure and return results
            var jyotish = _context.Set<JyotishModel>()
          .FromSqlInterpolated($"EXEC dbo.sp_filterJyotish @country={fm.country}, @city={fm.city}, @state={fm.state}, @expFrom={expFrom}, @expTo={expTo}, @rating={fm.rating}, @activity={fm.activity}, @gender={fm.gender ?? (object)DBNull.Value},@expertise={fm.expertise}")
          .ToList();

            return jyotish;
        }

        public List<JyotishModel> SearchAstrologer(string? searchInp)
        {


            // Call the stored procedure and return results
            var jyotish = _context.Set<JyotishModel>()
          .FromSqlInterpolated($"EXEC dbo.sp_searchJyotish @searchInp={searchInp}")
          .ToList();

            return jyotish;
        }

        public List<IdImageViewModel> SliderImageList()
        {
            var Records = _context.Sliders
                .Where(x => x.Status)
                .OrderBy(x => x.SerialNo)
                .Select(x => new IdImageViewModel
                {
                    Id = x.SerialNo,
                    ImageUrl = x.HomePage
                })
                .ToList();

            if (Records.Count == 0)
            { return null; }
            return Records;

        }

        public string BookAppointment(AppointmentViewModel model)
        {
            var Jyotish = _context.JyotishRecords.Where(x => x.Id == model.JyotishId).FirstOrDefault();
            var User = _context.Users.Where(x => x.Id == model.UserId).FirstOrDefault();
            var Slot = _context.AppointmentSlots.Where(x => x.Id == model.SlotId).FirstOrDefault();
            if (User == null || Jyotish == null || Slot == null || Slot.Status == "Booked") { return "invalid Data"; }


            AppointmentModel appointment = new AppointmentModel();
            appointment.Date = Slot.Date;
            appointment.SlotId = model.SlotId;
            appointment.JyotishId = Jyotish.Id;
            appointment.UserId = User.Id;
            appointment.Problem = model.Problem;
            appointment.Amount = Jyotish.AppointmentCharges;
            appointment.Status = "Upcomming";
            Slot.Status = "Booked";
            appointment.ArrivedStatus = 0;
            _context.AppointmentSlots.Update(Slot);
            _context.AppointmentRecords.Add(appointment);
            var result = _context.SaveChanges();
            if (result > 0)
            {

                return "Successful";
            }
            return "internal Server Error.";

        }

        public List<JyotishModel> SpecializationFilter(string Keyword)
        {
            Keyword = char.ToUpper(Keyword[0]) + Keyword.Substring(1).ToLower();
            var record = _context.JyotishRecords.Where(x => x.Specialization == Keyword).ToList();
            return record;
        }

        public UserProfileViewModal GetUserProfile(int Id)
        {
            var user = _context.Users.Where(x => x.Id == Id).FirstOrDefault();
            if (user == null)
            { return null; }

            var CountryName = _context.Countries.Where(x => x.Id == user.Country).Select(x=>x.Name).FirstOrDefault();
            var StateName = _context.States.Where(x => x.Id == user.State).Select(x => x.Name).FirstOrDefault();
            var CityName = _context.Cities.Where(x => x.Id == user.City).Select(x => x.Name).FirstOrDefault();
            var CountryCode = _context.CountryCode.Where(x => x.Id == user.CountryCodeId).Select(x => x.countryCode).FirstOrDefault();
            UserProfileViewModal Data = new UserProfileViewModal();
            Data.Id = Id;
            Data.Name = user.Name;
            Data.Email = user.Email;
            Data.Mobile = user.Mobile;
            Data.Gender = user.Gender;
            Data.DoB = user.DoB;
            Data.PlaceOfBirth = user.PlaceOfBirth;
            Data.TimeOfBirth = user.TimeOfBirth;
            Data.CurrentAddress = user.CurrentAddress;
            Data.CountryId = user.Country;
            Data.Country = CountryName;
            Data.StateId = user.State;
            Data.State = StateName;
            Data.CityId = user.City;
            Data.City = CityName;
            Data.Pincode = user.Pincode;
            Data.ProfilePictureUrl = user.ProfilePictureUrl;
            Data.CountryCodeId = user.CountryCodeId;
            Data.CountryCode = CountryCode;
            return Data;
        }
        public bool UpdateProfile(UserUpdateViewModel model, string path)
        {
            if (model == null)
            { return false; }

            var userModel = _context.Users.Where(x => x.Id == model.Id).FirstOrDefault();


            if (!string.IsNullOrEmpty(model.Mobile))
            {
                userModel.Mobile = model.Mobile;
            }

            if (!string.IsNullOrEmpty(model.Name))
            {
                userModel.Name = model.Name;
            }

            if (!string.IsNullOrEmpty(model.Gender))
            {
                userModel.Gender = model.Gender;
            }

           
            if (model.DoB == null)
            {
                userModel.DoB = model.DoB;
            }

            if (!string.IsNullOrEmpty(model.PlaceOfBirth))
            {
                userModel.PlaceOfBirth = model.PlaceOfBirth;
            }

            if (model.TimeOfBirth.HasValue)
            {
                userModel.TimeOfBirth = model.TimeOfBirth;
            }

            if (!string.IsNullOrEmpty(model.CurrentAddress))
            {
                userModel.CurrentAddress = model.CurrentAddress;
            }

            if (model.Pincode.HasValue)
            {
                userModel.Pincode = model.Pincode;
            }
           
            var StateName = _context.States.Where(x => x.Id == model.State).FirstOrDefault();
            var CityName = _context.Cities.Where(x => x.Id == model.City).FirstOrDefault();

           

            if (StateName != null)
            {
                userModel.State = model.State;
            }

            if (model.City.HasValue)
            {
                userModel.City = model.City;
            }
            if (model.ProfilePictureUrl != null)
            {
                string uploadFolder = path + "/wwwroot/Images/User";
                string imageName = Guid.NewGuid().ToString("N").Substring(0, 8) + model.ProfilePictureUrl.FileName;
                userModel.ProfilePictureUrl = "/Images/User/" + imageName;

                var filePath = Path.Combine(uploadFolder, imageName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    model.ProfilePictureUrl.CopyTo(stream);
                }
            }
            _context.Users.Update(userModel);
            if (_context.SaveChanges() > 0) { return true; }
            else { return false; }

        }

        public List<AppointmentDetailViewModel> UpcommingAppointment(int Id)
        {
            var User = _context.Users.Where(x => x.Id == Id).FirstOrDefault();
            if (User == null) { return null; }

            var Records = _context.AppointmentRecords.Where(x => x.UserId == Id && x.Date >= DateTime.Now.Date)  // Filter by the userId
             .Join(_context.Users,
                   record => record.UserId,
                   user => user.Id,
                   (record, user) => new { record, user })  // First join to get the user's details
             .Join(_context.JyotishRecords,  // Join with JyotishRecords instead of Users
                   combined => combined.record.JyotishId,
                   jyotish => jyotish.Id,
                   (combined, jyotish) => new { combined.record, combined.user, jyotish }) // Get Jyotish's details from JyotishRecords
             .Join(_context.AppointmentSlots,
                   combined => combined.record.SlotId,
                   slot => slot.Id,
                   (combined, slot) => new AppointmentDetailViewModel
                   {
                       Id = combined.record.Id,
                       UserName = combined.jyotish.Name,           // Store Jyotish's Name
                       UserEmail = combined.jyotish.Email,         // Store Jyotish's Email
                       UserMobile = combined.jyotish.Mobile,       // Store Jyotish's Mobile
                       UserId = combined.jyotish.Id,            // Added JyotishId
                       Problem = combined.record.Problem,
                       Date = slot.Date,
                       Time = slot.TimeFrom,
                       Status = combined.record.Status,
                       Amount = combined.record.Amount,
                       currentDate = DateTime.Now.Date.ToString("dd-MM-yyyy")
                   }).OrderByDescending(e => e.Id)
             .ToList();
            return Records;
        }

        public List<AppointmentDetailViewModel> AppointmentHistory(int Id)
        {
            var User = _context.Users.Where(x => x.Id == Id).FirstOrDefault();
            if (User == null) { return null; }

            var Records = _context.AppointmentRecords.Where(x => x.UserId == Id && x.Date < DateTime.Now.Date)  // Filter by the userId
             .Join(_context.Users,
                   record => record.UserId,
                   user => user.Id,
                   (record, user) => new { record, user })  // First join to get the user's details
             .Join(_context.JyotishRecords,  // Join with JyotishRecords instead of Users
                   combined => combined.record.JyotishId,
                   jyotish => jyotish.Id,
                   (combined, jyotish) => new { combined.record, combined.user, jyotish }) // Get Jyotish's details from JyotishRecords
             .Join(_context.AppointmentSlots,
                   combined => combined.record.SlotId,
                   slot => slot.Id,
                   (combined, slot) => new AppointmentDetailViewModel
                   {
                       Id = combined.record.Id,
                       UserName = combined.jyotish.Name,           // Store Jyotish's Name
                       UserEmail = combined.jyotish.Email,         // Store Jyotish's Email
                       UserMobile = combined.jyotish.Mobile,       // Store Jyotish's Mobile
                       UserId = combined.jyotish.Id,            // Added JyotishId
                       Problem = combined.record.Problem,
                       Date = slot.Date,
                       Time = slot.TimeFrom,
                       Status = combined.record.ArrivedStatus == 1 ? "Completed" : "Not Arrived",
                       Amount = combined.record.Amount,
                       currentDate = DateTime.Now.Date.ToString("dd-MM-yyyy")
                   }).OrderByDescending(e => e.Id)
             .ToList();
            return Records;
        }

        public dynamic GetProblemSolutionDetail(int appointmentId)
        {


            var Appointment = _context.AppointmentRecords.Where(x => x.Id == appointmentId).FirstOrDefault();
            if (appointmentId == 0 || Appointment == null)
            {
                return null;
            }

            var User = _context.Users.Where(x => x.Id == Appointment.UserId).FirstOrDefault();

            var record = (
                 from problem in _context.ProblemSolution
                 join appointment in _context.AppointmentRecords on problem.AppointmentId equals appointment.Id
                 join slot in _context.AppointmentSlots on appointment.SlotId equals slot.Id
                 join user in _context.Users on appointment.UserId equals user.Id
                 // Perform a left join on ClientMembers
                 join member in _context.ClientMembers on problem.memberId equals member.Id into memberGroup
                 from member in memberGroup.DefaultIfEmpty() // This allows for the left join behavior
                 where problem.AppointmentId == appointmentId

                 select new
                 {
                     problemId = problem.Id,
                     problems = problem.Problem.Split(new[] { "$%^" }, StringSplitOptions.RemoveEmptyEntries),
                     solution = problem.Solution,
                     appointmentId = appointment.Id,
                     appointmentDate = slot.Date.ToString("dd-MM-yyyy"),
                     appointmentTime = slot.TimeFrom,
                     duration = slot.TimeDuration,
                     UId = user.Id,
                     userName = user.Name,
                     currentDate = DateTime.Now,
                     memberName = member != null ? member.Name : null,
                     memberReltion = member != null ? member.relation : null,
                     memberId = member != null ? member.Id : 0,
                 }
                 ).ToList();



            return record;
        }


        public List<JyotishUserAttachmentJyotishViewModel> GetAllUserAttachments(int Id)
        {
            if (Id <= 0)
            {
                return new List<JyotishUserAttachmentJyotishViewModel>();
            }

            var record = _context.JyotishUserAttachmentRecord
                                 .Where(x => x.UserId == Id && x.Status)
                                 .ToList();

            var result = record.Select(x => new JyotishUserAttachmentJyotishViewModel
            {
                Id = x.Id,
                JyotishId = x.JyotishId,
                UserId = x.UserId,
                UserName = _context.Users.Where(y => y.Id == x.UserId).Select(z => z.Name).FirstOrDefault(),
                UserEmail = _context.Users.Where(y => y.Id == x.UserId).Select(z => z.Email).FirstOrDefault(),
                Title = x.Title,
                Image = x.Image
            }).ToList();

            return result;
        }

        public dynamic GetAttachmentByAppointment(int appointmentId, int memberId)
        {
            if (appointmentId != 0)
            {
                string member = memberId != 0 ? memberId.ToString() : null;
                var res = _context.JyotishUserAttachmentRecord.Where(e => e.AppointmentId == appointmentId && e.MemberId.ToString() == member && e.Status).OrderByDescending(e => e.Id).ToList();
                return res;
            }
            else
            {
                return "invalid Request";
            }
        }

        public ProblemSolutionJyotishGetViewModel GetProblemSolution(int appointmentId)
        {


            var Appointment = _context.AppointmentRecords.Where(x => x.Id == appointmentId).FirstOrDefault();
            if (appointmentId == 0 || Appointment == null)
            {
                return null;
            }

            var User = _context.Users.Where(x => x.Id == Appointment.UserId).FirstOrDefault();

            var record = _context.ProblemSolution.Where(x => x.AppointmentId == appointmentId).FirstOrDefault();
            if (record == null)
            {
                return null;
            }

            string problemString = record.Problem;
            string SolutionString = record.Solution;

            string[] problemArray = problemString.Split(new[] { "$%^" }, StringSplitOptions.RemoveEmptyEntries);



            ProblemSolutionJyotishGetViewModel Result = new ProblemSolutionJyotishGetViewModel
            {

                Id = record.Id,
                AppointmentId = (int)record.AppointmentId,
                UserId = User.Id,
                UserName = User.Name,

                Problem = problemArray,
                Solution = SolutionString

            };
            return Result;
        }

        public AppointmentModel GetAppointmentDetails(int Id)
        {
            var records = _context.AppointmentRecords.Where(x => x.Id == Id).FirstOrDefault();
            return records;
        }

        public List<UserPaymentRecordModel> UserPaymentrecords(int Id)
        {
            var User = _context.Users.Where(x => x.Id == Id).FirstOrDefault();
            if (User == null) { return null; }
            var record = _context.UserPaymentRecord.Where(x => x.UserId == Id).OrderBy(e => e.Id).Reverse().ToList();
            if (record.Count > 0) { return record; }
            else { return null; }
        }
        public UserPaymentRecordModel UserPaymentDetail(int Id)
        {

            var record = _context.UserPaymentRecord.Where(x => x.Id == Id).FirstOrDefault();
            if (record != null) { return record; }
            else { return null; }
        }

        public string AddUserWallets(UserWalletViewmodel uw)
        {
            var users = _context.Users.Where(x => x.Id == uw.userId).FirstOrDefault();
            if (users == null) { return null; }
            var Userwallet = _context.UserWallets.Where(x => x.userId == uw.userId).FirstOrDefault();

            if (Userwallet == null)
            {
                UserWallet user = new UserWallet
                {
                    userId = uw.userId,
                    WalletAmount = uw.WalletAmount,
                    status = 1
                };
                _context.UserWallets.Add(user);
                if (_context.SaveChanges() > 0) { return "Successful"; }
                else { return "Data Not Saved"; }
            }
            else
            {
                Userwallet.WalletAmount += uw.WalletAmount;
                _context.UserWallets.Update(Userwallet);
                if (_context.SaveChanges() > 0) { return "Successful"; }
                else { return "Data Not Saved"; }
            }

        }
        public string PurchaseWithUserWallets(UserWalletViewmodel uw)
        {
            var users = _context.Users.Where(x => x.Id == uw.userId).FirstOrDefault();
            if (users == null) { return null; }
            var Userwallet = _context.UserWallets.Where(x => x.userId == uw.userId).FirstOrDefault();

            if (Userwallet.WalletAmount > uw.WalletAmount)
            {
                Userwallet.WalletAmount -= uw.WalletAmount;
                _context.UserWallets.Update(Userwallet);
                if (_context.SaveChanges() > 0) { return "Successful"; }
                else { return "Data Not Saved"; }
            }
            else
            {
                return "Lower Balance";

            }

        }
        public long GetWallet(int UserId)
        {
            var Jyotish = _context.UserWallets.Where(x => x.userId == UserId).FirstOrDefault();
            if (Jyotish == null) { return 0; }
            else
            {
                return Jyotish.WalletAmount;
            }
        }
        //wallet history
        public string AddWalletHistory(WalletHistoryViewmodel pr)
        {
            var Jyotish = _context.Users.Where(x => x.Id == pr.UId).FirstOrDefault();
            if (Jyotish == null) { return null; }
            string paymentId = pr.PaymentId;
            if (pr.PaymentId == null)
            {
                Random rnd = new Random();
                var rndnum = rnd.Next(100, 999);
                paymentId = Jyotish.Name.Split(' ')[0] + DateTime.Now.ToString("ddMMyyyyhhmmss") + pr.UId + rndnum;
                var paymentUnqId = _context.WalletHistroy.Where(x => x.PaymentId == paymentId).FirstOrDefault();
                if (paymentUnqId != null)
                {
                    paymentId = paymentId + rndnum;
                }
            }
            WalletHistoryModel jw = new WalletHistoryModel
            {
                JId = pr.JId != 0 ? pr.JId : null,
                amount = pr.amount,
                PaymentStatus = pr.PaymentStatus,
                PaymentAction = pr.PaymentAction,
                date = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"),
                PaymentId = paymentId,
                status = 1,
                PaymentBy = "user",
                PaymentFor = pr.PaymentFor,
                UId = pr.UId
            };
            _context.WalletHistroy.Add(jw);
            if (_context.SaveChanges() > 0) { return "Successful"; }
            else { return "Data Not Saved"; }

        }
        public dynamic GetWalletHistory(int UserId)
        {
            var Jyotish = (from wallet in _context.WalletHistroy
                           join jyotish in _context.JyotishRecords on wallet.JId equals jyotish.Id into jyotishGroup
                           from jyotish in jyotishGroup.DefaultIfEmpty()
                           where wallet.UId == UserId
                           orderby wallet.Id descending
                           select new
                           {
                               UserName = wallet.JId != null ? jyotish.Name : null,
                               paymentDate = wallet.date,
                               amount = wallet.amount,
                               paymentId = wallet.PaymentId,
                               paymentBy = wallet.PaymentBy,
                               paymentAction = wallet.PaymentAction,
                               profile = wallet.JId != null ? jyotish.ProfileImageUrl : null,
                               paymentFor = wallet.PaymentFor,
                               paymentStatus = wallet.PaymentStatus,
                               Id = wallet.Id
                           }

                          );

            return Jyotish;
        }

        public List<AppointmentSlotUserViewModel> GetAllAppointmentSlot(int id)
        {
            DateTime today = DateTime.Now;
            DateTime yesterday = today.AddDays(-1);

            DateTime yesterdayDateOnly = yesterday.Date;

            var record = _context.AppointmentSlots.Where(x => x.JyotishId == id)
                    .GroupBy(x => x.Date)
                    .Select(g => new AppointmentSlotUserViewModel
                    {

                        Date = g.Key, // Date is the grouping key
                        SlotList = g.Select(x => new AppointmentSlotDateUserViewModel
                        {
                            Id = x.Id,
                            TimeDuration = x.TimeDuration,
                            TimeFrom = x.TimeFrom,
                            TimeTo = x.TimeTo,
                            JyotishId = (int)x.JyotishId,
                            Status = x.Status,
                            Amount = _context.JyotishRecords.Where(e => e.Id == id).Select(x => (int)x.AppointmentCharges).FirstOrDefault()
                        }).ToList()
                    })
                    .ToList();
            var result = record.Where(x => x.Date > yesterdayDateOnly).ToList();
            return result;
        }
        public List<ProblemSolutionGetAllViewModel> GetAllProblemSolution(int id)
        {
            if (id == 0)
            {
                return null;
            }

            var data = _context.ProblemSolution
                .Include(x => x.Appointment)
                    .ThenInclude(x => x.UserRecord)
                .Include(x => x.Appointment)
                    .ThenInclude(x => x.JyotishRecord)
                .Where(x => x.Appointment.UserId == id)
                .AsEnumerable() // Switch to client-side evaluation for in-memory processing
                .GroupBy(x => new { x.Appointment.UserId, x.Appointment.JyotishId, x.Appointment.Id })
                .Select(group =>
                {
                    var first = group.FirstOrDefault();
                    var slot = _context.AppointmentSlots.FirstOrDefault(s => s.Id == first.Appointment.SlotId);

                    return new ProblemSolutionGetAllViewModel
                    {
                        Id = first.Id,
                        JyotishId = group.Key.JyotishId,
                        JyotishName = first.Appointment.JyotishRecord.Name,
                        JyotishEmail = first.Appointment.JyotishRecord.Email,
                        UserId = group.Key.UserId,
                        AppointmentId = group.Key.Id,
                        Date = slot != null ? DateOnly.FromDateTime(slot.Date) : DateOnly.MinValue,
                        Time = slot != null ? slot.TimeFrom : TimeOnly.MinValue,

                    };
                })
                .ToList();

            return data;
        }


        public LayoutDataViewModel LayoutData(int Id)
        {
            var record = _context.Users.Where(x => x.Id == Id).FirstOrDefault();
            if (record == null)
            {
                return null;
            }
            else
            {
                LayoutDataViewModel layoutData = new LayoutDataViewModel
                {
                    Id = record.Id,
                    Name = record.Name,
                    Image = record.ProfilePictureUrl
                };
                return layoutData;
            }
        }

        public string AddRating(JyotishRatingViewModel data)
        {
            var User = _context.Users.Where(x => x.Id == data.UserId).FirstOrDefault();
            var Jyotish = _context.JyotishRecords.Where(x => x.Id == data.JyotishId && x.Status == true).FirstOrDefault();
            if (User == null || Jyotish == null)
            {
                return "Invalid Id";
            }
            var Appointment = _context.AppointmentRecords.Where(x => x.JyotishId == data.JyotishId && x.UserId == data.UserId).FirstOrDefault();
            var Chat = _context.ChatedUser.Where(x => x.UserId == data.UserId && x.JyotishId == data.JyotishId).FirstOrDefault();
            var Rating = _context.JyotishRating.Where(x => x.JyotishId == data.JyotishId && x.UserId == data.UserId).FirstOrDefault();
            if (Appointment == null && Chat == null)
            {
                return "No Data Found of Services";
            }


            if (Rating != null)
            {
                Rating.FeedbackMessage = data.FeedbackMessage;
                Rating.Stars = data.Stars;

                Rating.DateTime = DateTime.Now;
                Rating.Status = false;
                _context.JyotishRating.Update(Rating);
            }
            else
            {
                JyotishRatingModel NewRecord = new JyotishRatingModel();
                NewRecord.FeedbackMessage = data.FeedbackMessage;
                NewRecord.Stars = data.Stars;
                NewRecord.UserId = data.UserId;
                NewRecord.JyotishId = data.JyotishId;
                NewRecord.DateTime = DateTime.Now;
                NewRecord.Status = false;
                _context.JyotishRating.Add(NewRecord);
            }

            if (_context.SaveChanges() > 0)
            {
                return "Successful";
            }
            else { return "Internal Server Error"; }


        }

        public List<JyotishRatingViewModel> JyotishRatingList(int Id)
        {
            var record = _context.JyotishRating.Where(x => x.JyotishId == Id && x.Status == true).Include(x => x.Jyotish).Include(x => x.User).Select(x => new JyotishRatingViewModel
            {
                Id = x.Id,
                FeedbackMessage = x.FeedbackMessage,
                Stars = x.Stars,
                DateTime = x.DateTime,
                UserName = x.User.Name,



            }).OrderByDescending(x => x.DateTime).ToList();
            return record;
        }

        public string IsUserValidForRating(int UserId, int JyotishId)
        {
            var user = _context.Users.Where(x => x.Id == UserId).FirstOrDefault();
            var Jyotish = _context.JyotishRecords.Where(x => x.Id == JyotishId).FirstOrDefault();
            if (user == null || Jyotish == null) { return "Invalid Id"; }
            var Appointment = _context.AppointmentRecords.Where(x => x.JyotishId == JyotishId && x.UserId == UserId).FirstOrDefault();
            var Chat = _context.ChatedUser.Where(x => x.JyotishId == JyotishId && x.UserId == UserId).FirstOrDefault();
            if (Appointment == null && Chat == null) { return "No"; }
            return "Yes";
        }


        public bool AddUserServiceRecord(UserServiceRecordViewModel data)
        {
            var Jyotish = _context.JyotishRecords.Where(x => x.Id == data.JyotishId).FirstOrDefault();
            var User = _context.Users.Where(x => x.Id == data.UserId).FirstOrDefault();
            if (Jyotish == null || User == null)
            {
                return false;
            }

            UserServiceRecordModel newData = new UserServiceRecordModel();
            newData.Name = data.Name;
            newData.Gender = data.Gender;
            newData.DateOfBirth = data.DateOfBirth;
            newData.TimeOfBirth = data.TimeOfBirth;
            newData.PlaceOfBirth = data.PlaceOfBirth;
            newData.UserId = data.UserId;
            newData.JyotishId = data.JyotishId;
            newData.Action = data.Action;
            newData.Status = true;
            _context.UserServiceRecord.Add(newData);
            if (_context.SaveChanges() > 0)
            {
                return true;
            }
            else { return false; }


        }

        public UserServiceRecordViewModel GetUserDataForService(int Id)
        {
            var User = _context.Users.Where(x => x.Id == Id).FirstOrDefault();
            if (User == null) { return null; }
            UserServiceRecordViewModel UserRecord = new UserServiceRecordViewModel();

            UserRecord.Name = User.Name;
            UserRecord.Gender = User.Gender;
            UserRecord.DateOfBirth = (DateOnly)User.DoB;
            if (User.TimeOfBirth != null)
            {
                UserRecord.TimeOfBirth = (TimeOnly)User.TimeOfBirth;
            }

            UserRecord.PlaceOfBirth = User.PlaceOfBirth;
            UserRecord.UserId = User.Id;

            return UserRecord;

        }

        public bool AddKundaliMatchingRecord(List<KundaliMatchingViewModel> DataList)
        {
            var Transaction = _context.Database.BeginTransaction();
            try
            {
                if (DataList.Count == 0)
                {
                    return false;
                }

                int Step1 = 0;
                var KundaliRecords = _context.KundaliMatchingRecord.Where(x => x.UserId == DataList[0].UserId && x.ActiveStatus).ToList();
                if (KundaliRecords.Count > 0)
                {
                    KundaliRecords.ForEach(record => record.ActiveStatus = false);
                    _context.KundaliMatchingRecord.UpdateRange(KundaliRecords);
                    Step1 = _context.SaveChanges();
                }
                else {
                    Step1 = 1;
                }


                if (Step1>0)
                {
                    List<KundaliMatchingModel> KundaliList = new List<KundaliMatchingModel>();
                    foreach (var data in DataList)
                    {
                        KundaliList.Add(new KundaliMatchingModel
                        {
                            Name = data.Name,
                            UserId = data.UserId,
                            DateOfBirth = data.DateOfBirth,
                            TimeOfBirth = data.TimeOfBirth,
                            PlaceOfBirth = data.PlaceOfBirth,
                            DateTime = DateTime.Now,
                            Gender = data.Gender,
                            Status = true,
                            Latitude = data.Latitude,
                            Longitude = data.Longitude,
                            Timezone = data.Timezone,
                            ActiveStatus = true,
                        });
                    }
                    _context.AddRange(KundaliList);
                    if (_context.SaveChanges() > 0)
                    {
                        Transaction.Commit();
                        return true;
                    }
                    else
                    {
                        Transaction.Rollback();
                        return false;
                    }

                }
                else
                {

                    return false;
                }
            }
            catch (Exception ex)
            {
                Transaction.Rollback();
                return false;
            }
        }

        public List<KundaliMatchingViewModel> GetAllKundaliMatchingRecord(int Id)
        {
            var User = _context.Users.Where(x => x.Id == Id).FirstOrDefault();
            if (User == null)
            { return null; }
            var Record = _context.KundaliMatchingRecord.Where(x => x.UserId == Id && x.Status).Select(x=> new
            KundaliMatchingViewModel
            {
                Id=x.Id,
                Name = x.Name,
                UserId = x.UserId,
                DateOfBirth= x.DateOfBirth,
                TimeOfBirth = x.TimeOfBirth,
                PlaceOfBirth = x.PlaceOfBirth,
                Gender = x.Gender,
                Latitude= x.Latitude,
                Longitude = x.Longitude,
                Timezone = x.Timezone,

            }).ToList();
            Record.Reverse();
            return Record;
        }

        public List<KundaliMatchingViewModel> GetLatestKundaliRecord(int Id)
        {
            var User = _context.Users.Where(x => x.Id == Id).FirstOrDefault();
            if (User == null)
            { return null; }
            var Record = _context.KundaliMatchingRecord.Where(x => x.UserId == Id && x.Status &&x.ActiveStatus).Select(x => new
            KundaliMatchingViewModel
            {
                Id = x.Id,
                Name = x.Name,
                UserId = x.UserId,
                DateOfBirth = x.DateOfBirth,
                TimeOfBirth = x.TimeOfBirth,
                PlaceOfBirth = x.PlaceOfBirth,
                Gender = x.Gender,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                Timezone = x.Timezone,

            }).ToList();
            return Record;
        }
        public bool DeleteKundaliRecord(int Id)
        {
            var Record = _context.KundaliMatchingRecord.Where(x => x.Id == Id && x.Status).FirstOrDefault();
            if(Record == null)
            {
                return false;
            }

            Record.Status = false;
            Record.ActiveStatus = false;
            _context.KundaliMatchingRecord.Update(Record);
            if (_context.SaveChanges() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public dynamic getAllPoojaList()
        {
            var res = (from pooja in _context.PoojaRecord
                     
                       join list in _context.PoojaList on pooja.PoojaType equals list.Id
                       where pooja.status &&  list.Status
                       select new
                       {
                          Id=pooja.Id,
                          poojaName=list.Name,
                          title=pooja.title,
                          image=pooja.Image,
                          

                       }).ToList();
            return res;
        }

		public dynamic getPoojaDetailByPoojaId(int Id)
		{
			var res = (from pooja in _context.PoojaRecord
					   join list in _context.PoojaList on pooja.PoojaType equals list.Id
					   where pooja.Id==Id&& pooja.status &&  list.Status
					   select new
					   {
						   Id = pooja.Id,
						   poojaName = list.Name,
						   title = pooja.title,
						   image = pooja.Image,						   
                           proccedure=pooja.Procedure,
                           aboutgod=pooja.AboutGod,
                           benefits=pooja.Benefits

					   }).FirstOrDefault();
			return res;
		}

        public bool BookPooja(BookedPoojaViewModel model)
        {
            var res = _context.BookedPoojaList.Where(e => e.status && e.PoojaId == model.PoojaId && model.userId == e.userId).FirstOrDefault();
            if (res != null)
            {
                return false;
            }

            BookedPoojaList book = new BookedPoojaList
            {
                PoojaId = model.PoojaId,
                userId = model.userId,
                BookingDate = DateTime.Now,
                status=true

            };

            _context.BookedPoojaList.Add(book);
            return _context.SaveChanges() > 0;
        }


        public string GetTimezone(string country)
        {
            var CountryCode = _context.Countries.Where(x=>x.Name == country).Select(x=>x.CountryCode).FirstOrDefault(); 
            if(CountryCode == null) { return null; }
            var Timezone = _context.Timezone.Where(x => x.Country == CountryCode).Select(x=>x.Timezone).FirstOrDefault();
            if (Timezone == null) { return null; }
            return Timezone;
        }

	}
}
