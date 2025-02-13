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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UserModel = ModelAccessLayer.Models.UserModel;
using UserWalletViewmodel = ModelAccessLayer.ViewModels.UserWalletViewmodel;

namespace BusinessAccessLayer.Implementation
{
    public class UserServices : IUserServices
    {
        private readonly ApplicationContext _context;
        private readonly IAdminServices _admin;
        public UserServices(ApplicationContext context, IAdminServices admin)
        {
            _context = context;
            _admin = admin;
        }

        public dynamic GetAstroListCallChat(string ListName)
        {
            if (ListName == "Chat")
            {
                var record = _context.JyotishRecords.Where(x => x.Chat == true && x.Status && !x.NewStatus).Include(e => e.JyotishRating).Select(e => new {
                    activeStatus=e.ActiveStatus,
                    chat=e.Chat,
                    chatCharges=e.ChatCharges,
                    city=e.City,
                    experience=e.Experience,
                    expertise=e.Expertise,
                    gender=e.Gender,
                    id=e.Id,
                    name=e.Name,
                    profileImageUrl=e.ProfileImageUrl,
                    specialization=e.Specialization,
                    serviceStatus=e.ServiceStatus,
                    stars= e.JyotishRating!=null && e.JyotishRating.Any()?e.JyotishRating.Average(e=>e.Stars):0,
                    totalOrder= e.UserServiceRecord.Any()?e.UserServiceRecord.Where(e=>e.Action==1).Sum(e=>e.Count):0,
                    language=e.Language
                }).ToList().OrderByDescending(e => e.stars).ToList();
                return record;
            }
            else if (ListName == "Call")
            {
                var record = _context.JyotishRecords.Where(x => x.Call == true && x.Status && !x.NewStatus).Include(e => e.JyotishRating).Select(e => new {
                    activeStatus = e.ActiveStatus,
                    call = e.Call,
                    callCharges = e.CallCharges,
                    city = e.City,
                    experience = e.Experience,
                    expertise = e.Expertise,
                    gender = e.Gender,
                    id = e.Id,
                    name = e.Name,
                    profileImageUrl = e.ProfileImageUrl,
                    specialization = e.Specialization,
                    serviceStatus = e.ServiceStatus,
                    stars = e.JyotishRating != null && e.JyotishRating.Any() ? e.JyotishRating.Average(e => e.Stars) : 0,
                    totalOrder = e.UserServiceRecord.Any() ? e.UserServiceRecord.Where(e => e.Action == 2).Sum(e => e.Count) : 0,
                    language = e.Language
                }).ToList().OrderByDescending(e => e.stars).ToList();
                return record;
            }
            else { return null; }
        }

        public PoojaRecordModel GetPoojaDetail(int PoojaId)
        {
            var record = _context.PoojaRecord.Where(x => x.Id == PoojaId).FirstOrDefault();
            if (record == null)
            { return null; }
            else { return record; }
        }

        public dynamic TopAstrologer()
        {
            Random rmd = new Random();

            var records = _context.JyotishRecords
                .Where(j => j.Role == "Jyotish")
                .Include(x => x.JyotishRating) // Include related ratings
                .Select(x => new
                {
                    Id = x.Id,
                    profileImageUrl = x.ProfileImageUrl,
                    Name = x.Name,
                    City = x.City,
                    Expertise = x.Expertise,
                    Experience = x.Experience,
                    Language = x.Language,
                    CallPrice = x.CallCharges,
                    ChatPrice = x.ChatCharges,
                    stars = x.JyotishRating != null && x.JyotishRating.Any()
                            ? x.JyotishRating.Average(r => r.Stars)
                            : 0
                })
                .ToList() // Materialize the data into memory
                .OrderByDescending(e => e.stars) // Sort by stars in memory
                .Take(10) // Take top 10 records
                .OrderBy(e => rmd.Next()) // Randomize the order in memory
                .ToList();

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
                               .Select(x => new
                               {
                                   x.Id,
                                   x.Email,
                                   x.Mobile,
                                   x.AlternateMobile,
                                   x.Name,
                                   x.Gender,
                                   x.Language,
                                   x.Expertise,
                                   x.Country,
                                   x.State,
                                   x.City,
                                   x.DateOfBirth,
                                   x.ProfileImageUrl,
                                   x.Experience,
                                   x.Pooja,
                                   x.Call,
                                   x.CallCharges,
                                   x.Chat,
                                   x.ChatCharges,
                                   x.Appointment,
                                   x.AppointmentCharges,
                                   x.Address,
                                   x.TimeTo,
                                   x.TimeFrom,
                                   x.About,
                                   x.AwordsAndAchievement,
                                   x.Specialization,
                                   x.Date,
                                   x.countryCode,
                                   x.ActiveStatus,
                                   x.ServiceStatus
                               })
                               .FirstOrDefault();


            // Return null if no record found
            if (jyotishRecord == null)
            {
                return null;
            }


            var RatingList = _context.JyotishRating
      .Where(x => x.JyotishId == jyotishRecord.Id && x.Status)
      .Select(x => x.Stars);
            var Rating = RatingList.Any() ? RatingList.Average() : 0;
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

            var specializationArray = jyotishRecord.Specialization?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                 .Select(a => a.Trim())
                                                 .ToArray();
            var totalAmountForCurrentYearByMonth = _context.UserServiceRecord
                .Where(e => e.Action == 2 && e.JyotishId == Id).Sum(e => e.Count);

            var totalAmountForsYearByMonthForChat = _context.UserServiceRecord
    .Where(e => e.Action == 1 && e.JyotishId == Id).Sum(e => e.Count);
    
            var profileViewModel = new JyotishProfileViewModel
            {
                Id = jyotishRecord.Id,
                Email = jyotishRecord.Email,
                Mobile = jyotishRecord.Mobile,
                Name = jyotishRecord.Name,
                Gender = jyotishRecord.Gender,
                Language = jyotishRecord.Language,
                Expertise = jyotishRecord.Expertise,
                Country = jyotishRecord.Country,
                State = jyotishRecord.State,
                City = jyotishRecord.City,
                ActiveStatus=jyotishRecord.ActiveStatus,
                DateOfBirth = jyotishRecord.DateOfBirth,
                ProfileImageUrl = jyotishRecord.ProfileImageUrl,
                Experience = jyotishRecord.Experience,

                Call = jyotishRecord.Call,
                CallCharges = jyotishRecord.CallCharges,
                Chat = jyotishRecord.Chat,
                ChatCharges = jyotishRecord.ChatCharges,
                AppointmentCharges = jyotishRecord.AppointmentCharges,
                Appointment = jyotishRecord.Appointment,
                Address = jyotishRecord.Address,
                TimeTo = jyotishRecord.TimeTo,
                TimeFrom = jyotishRecord.TimeFrom,
                About = jyotishRecord.About,
                AwordsAndAchievement = jyotishRecord.AwordsAndAchievement,
                Specialization = specializationArray,
                Videos = videos,
                Gallery = gallery,
                Rating = Rating,
                TotalReview = RatingList.Count(),
                totalCall = totalAmountForCurrentYearByMonth,
                totalChat = totalAmountForsYearByMonthForChat
            };

            return profileViewModel;
        }

        public List<JyotishModelService> FilterAstrologer(FilterModel fm)
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
            var jyotish = _context.Database.SqlQuery<JyotishModelService>($"EXEC dbo.sp_filterJyotish @country={fm.country}, @city={fm.city}, @state={fm.state}, @expFrom={expFrom}, @expTo={expTo}, @rating={fm.rating}, @activity={fm.activity}, @gender={fm.gender ?? (object)DBNull.Value},@expertise={fm.expertise}")
          .ToList();

            return jyotish;
        }

        public List<JyotishModelService> SearchAstrologer(string? searchInp)
        {


            // Call the stored procedure and return results
            var jyotish = _context.Database.SqlQuery<JyotishModelService>($"EXEC dbo.sp_searchJyotish @searchInp={searchInp}")
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
            appointment.BookMark = 0;
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

            var CountryName = _context.Countries.Where(x => x.Id == user.Country).Select(x => x.Name).FirstOrDefault();
            var StateName = _context.States.Where(x => x.Id == user.State).Select(x => x.Name).FirstOrDefault();
            var CityName = _context.Cities.Where(x => x.Id == user.City).Select(x => x.Name).FirstOrDefault();
            var CountryCode = _context.CountryCode.Where(x => x.Id == user.CountryCodeId).Select(x => x.countryCode).FirstOrDefault();
            UserProfileViewModal Data = new UserProfileViewModal();
            Data.Id = Id;
            Data.Name = user.Name;
            Data.Email = user.Email;
            Data.Mobile = user.Mobile;
            Data.Gender = user.Gender;
            Data.DateOfBirth = user.DoB != null ? Convert.ToDateTime(user.DoB).ToString("dd/MM/yyy") : null;
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


            if (model.DoB != null)
            {
                userModel.DoB = model.DoB;
            }

            if (!string.IsNullOrEmpty(model.PlaceOfBirth))
            {
                userModel.PlaceOfBirth = model.PlaceOfBirth;
            }

            if (!string.IsNullOrEmpty(model.TimeOfBirth))
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

        public bool ApplyChargesFromUserWalletForService(int userId, int amount, string message, int jyotishId)
        {
            var transaction = _context.Database.BeginTransaction();
            var res = _context.UserWallets.Where(e => e.userId == userId && e.status == 1).FirstOrDefault();
            if (res != null)
            {
                if (res.WalletAmount < amount)
                {
                    return false;
                }

                res.WalletAmount = res.WalletAmount - amount;
                _context.UserWallets.Update(res);
                if (_context.SaveChanges() > 0)
                {
                    var jWallet = _context.JyotishWallets.Where(e => e.jyotishId == jyotishId && e.status == 1).FirstOrDefault();
                    if (jWallet != null)
                    {
                        var jyotishCharges = _admin.getJyotishChargesById(jyotishId);
                        if (jyotishCharges == null)
                        {
                            jyotishCharges = _admin.getJyotishChargesById(0);
                        }
                        var amountAfterPercentage = amount * (jyotishCharges.Charge / 100);
                        jWallet.WalletAmount += amountAfterPercentage;
                        _context.JyotishWallets.Update(jWallet);

                    }
                    else
                    {
                        jyotishWallet jw = new jyotishWallet
                        {
                            jyotishId = jyotishId,
                            WalletAmount = amount,
                            status = 1
                        };
                        _context.JyotishWallets.Add(jw);
                    }
                    if (_context.SaveChanges() > 0)
                    {

                        WalletHistoryViewmodel wh = new WalletHistoryViewmodel
                        {
                            JId = jyotishId,
                            UId = userId,
                            amount = amount,
                            PaymentAction = "Credit",
                            PaymentStatus = "success",
                            PaymentFor = message,
                        };
                        AddWalletHistory(wh);
                    }
                    transaction.Commit();
                    return true;
                }
                return false;
            }
            else
            {
                transaction.Rollback();
                return false;
            }
        }

        public dynamic getJyotishServicesCharges(int jyotishid)
        {
            var res = _context.JyotishRecords
    .Where(e => e.Id == jyotishid && e.Status)
    .Select(e => e.ChatCharges)
    .FirstOrDefault();
            return res;

        }

        public dynamic getJyotishCallServicesCharges(int jyotishid)
        {
            var res = _context.JyotishRecords
    .Where(e => e.Id == jyotishid && e.Status)
    .Select(e => e.CallCharges)
    .FirstOrDefault();
            return res;

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


        public dynamic getAllmembers(int userId)
        {
            var res = (from user in _context.Users
                       where user.Id == userId
                       select new
                       {
                           userid = user.Id,
                           userMemberId = 0,
                           userName = user.Name,
                           userDob = user.DoB,
                           members = _context.ClientMembers
                                              .Where(e => e.UId == userId && e.status == 1)
                                              .GroupBy(e => e.Name).AsEnumerable()
                                              .Select(g => g.First())
                       }).ToList();
            return res;
        }


        public dynamic getAllAppointmentBymemebersanduser(int memberId, int userId, int jyotishId)
        {
            string memberFromatedId = memberId == 0 ? null : memberId.ToString();
            var res = (from problem in _context.ProblemSolution
                       join appointment in _context.AppointmentRecords on problem.AppointmentId equals appointment.Id
                       orderby appointment.Id descending
                       where (problem.memberId.ToString() == memberFromatedId && appointment.UserId == userId && appointment.JyotishId == jyotishId) && (problem.Member.status == 1 || problem.Member == null)
                       select new
                       {
                           appointmentId = appointment.Id,
                           appointmentDate = appointment.AppointmentSlotData.Date.ToString("dd-MM-yyyy"),
                           appointmentTimeFrom = appointment.AppointmentSlotData.TimeFrom.ToString("hh:mm"),
                           appointmentTimeTo = appointment.AppointmentSlotData.TimeTo.ToString("hh:mm"),
                           jyotishNama = appointment.AppointmentSlotData.JyotishData.Name
                       }).ToList();
            return res;
        }
        public dynamic getjyotishByuserAppointment(int userId, int memberId)
        {
            var res = (from appointment in _context.AppointmentRecords
                       join user in _context.Users on appointment.UserId equals user.Id
                       join jyotish in _context.JyotishRecords on appointment.JyotishId equals jyotish.Id
                       join client in _context.ClientMembers on jyotish.Id equals client.JId
                       join ps in _context.ProblemSolution on appointment.Id equals ps.AppointmentId into psJoin
                       from ps in psJoin.DefaultIfEmpty()
                       where appointment.UserId == userId && (client.Id == memberId || memberId == 0)
                       orderby ps.Id
                       select new
                       {
                           jyotishId = jyotish.Id,
                           jyotishName = jyotish.Name,
                           expertise = jyotish.Expertise,
                           experience = jyotish.Experience
                       }).ToList().Distinct();
            return res;
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
            var OldData = _context.UserServiceRecord.Where(x => x.UserId == data.UserId && x.JyotishId == data.JyotishId && x.Status && x.Name == data.Name && x.Gender == data.Gender && x.DateOfBirth == data.DateOfBirth && x.TimeOfBirth == data.TimeOfBirth && x.PlaceOfBirth == data.PlaceOfBirth && x.Action==data.Action && x.date.Year==DateTime.Now.Date.Year).FirstOrDefault();

            if (OldData != null)
            {
                OldData.Count += 1;
                OldData.date = DateTime.Now;
                _context.UserServiceRecord.Update(OldData);
                return _context.SaveChanges() > 0;
            }

            UserServiceRecordModel newData = new UserServiceRecordModel();
            newData.Name = data.Name;
            newData.Gender = data.Gender;
            newData.DateOfBirth = (DateTime)data.DateOfBirth;
            newData.TimeOfBirth = data.TimeOfBirth;
            newData.PlaceOfBirth = data.PlaceOfBirth;
            newData.UserId = data.UserId;
            newData.JyotishId = data.JyotishId;
            newData.Action = data.Action;
            newData.Status = true;
            newData.Count = 1;
            newData.date = DateTime.Now;
            newData.LastTalkTime = 0;
            newData.TotalTime = 0;
            _context.UserServiceRecord.Add(newData);
            if (_context.SaveChanges() > 0)
            {
                return true;
            }
            else { return false; }


        }

        public bool UpdateUserService(int userId,int action,double totalTime)
        {
           
            var oldData = _context.UserServiceRecord.Where(e => e.UserId == userId && e.Action == action).OrderByDescending(e=>e.date).FirstOrDefault();

            if (oldData == null)
            {
                return false;
               
            }
            oldData.LastTalkTime = (float)totalTime;
            oldData.TotalTime += (float)totalTime;
            _context.UserServiceRecord.Update(oldData);
            return _context.SaveChanges() > 0;
        }

        public UserServiceRecordViewModel GetUserDataForService(int Id)
        {
            var User = _context.Users.Where(x => x.Id == Id).FirstOrDefault();
            if (User == null) { return null; }
            UserServiceRecordViewModel UserRecord = new UserServiceRecordViewModel();

            UserRecord.Name = User.Name;
            UserRecord.Gender = User.Gender;
            UserRecord.DateOfBirth = (DateTime)User.DoB;
            if (User.TimeOfBirth != null)
            {
                UserRecord.TimeOfBirth = User.TimeOfBirth.ToString();
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
                else
                {
                    Step1 = 1;
                }


                if (Step1 > 0)
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
            var Record = _context.KundaliMatchingRecord.Where(x => x.UserId == Id && x.Status).Select(x => new
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
            Record.Reverse();
            return Record;
        }

        public List<KundaliMatchingViewModel> GetLatestKundaliRecord(int Id)
        {
            var User = _context.Users.Where(x => x.Id == Id).FirstOrDefault();
            if (User == null)
            { return null; }
            var Record = _context.KundaliMatchingRecord.Where(x => x.UserId == Id && x.Status && x.ActiveStatus).Select(x => new
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
            if (Record == null)
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
                       where pooja.status && list.Status
                       select new
                       {
                           Id = pooja.Id,
                           poojaName = list.Name,
                           title = pooja.title,
                           image = pooja.Image,


                       }).ToList();
            return res;
        }

        public dynamic getPoojaDetailByPoojaId(int Id)
        {
            var res = (from pooja in _context.PoojaRecord
                       join list in _context.PoojaList on pooja.PoojaType equals list.Id
                       where pooja.Id == Id && pooja.status && list.Status
                       select new
                       {
                           Id = pooja.Id,
                           poojaTypeId = pooja.PoojaType,
                           poojaName = list.Name,
                           title = pooja.title,
                           image = pooja.Image,
                           proccedure = pooja.Procedure,
                           aboutgod = pooja.AboutGod,
                           benefits = pooja.Benefits

                       }).FirstOrDefault();
            return res;
        }

        public bool BookPooja(BookedPoojaViewModel model)
        {
            var res = _context.BookedPoojaList.Where(e => e.status && e.PoojaId == model.PoojaId && model.userId == e.userId && !e.completeStatus).FirstOrDefault();
            if (res != null)
            {
                return false;
            }

            BookedPoojaList book = new BookedPoojaList
            {
                PoojaId = model.PoojaId,
                userId = model.userId,
                jyotishId = model.jyotishId,
                BookingDate = DateTime.Now,
                PoojaDate = model.PoojaDate,
                status = true,
                completeStatus=false

            };

            _context.BookedPoojaList.Add(book);
            return _context.SaveChanges() > 0;
        }



        public string GetTimezone(string country)
        {
            var CountryCode = _context.Countries.Where(x => x.Name == country).Select(x => x.CountryCode).FirstOrDefault();
            if (CountryCode == null) { return null; }
            var Timezone = _context.Timezone.Where(x => x.Country == CountryCode).Select(x => x.Timezone).FirstOrDefault();
            if (Timezone == null) { return null; }
            return Timezone;
        }

        public dynamic getJyotishRecordByPoojaType(int poojaTypeId)
        {
            var res = _context.JyotishPooja.Where(e => e.poojaType == poojaTypeId && e.status).Include(s => s.pooja).Include(f => f.jyotish).Where(e => e.status).Select(e => new
            {
                jyotishId = e.jyotish.Id,
                jyotishName = e.jyotish.Name,
                jyotishProfile = e.jyotish.ProfileImageUrl,
                expertise = e.jyotish.Expertise,
                experience = e.jyotish.Experience,
                country = e.jyotish.Country,
                poojaName = e.pooja.Name,
                state = e.jyotish.State,
                city = e.jyotish.City,
                amount = e.amount
            }).ToList();
            return res;
        }

        public bool changeUserServiceStatus(int userId, bool status)
        {
            var res = _context.Users.Where(e => e.Id == userId).FirstOrDefault();
            if (res != null)
            {
                res.ServiceStatus = status;
                _context.Users.Update(res);
                return _context.SaveChanges() > 0;
            }
            return false;
        }

        public bool getUserserviceStatus(int userId)
        {
            var res = _context.Users.Where(e => e.Id == userId).Select(e => e.ServiceStatus).FirstOrDefault();
            if (res != null)
            {

                return (bool)res;
            }
            return false;
        }
        public dynamic getAppointmentCharges()
        {
            var res = _context.AppointmentChargesManagement.Where(e => e.status).FirstOrDefault();

            return res;
        }

        public dynamic GettopTenWalletHistory(int UserId)
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

                          ).Take(10).ToList();

            return Jyotish;
        }


        public async Task<List<Advertisementservice>> AdvertisementBanner()
        {
            try
            {
                // Initialize HttpClient
                HttpClient client = new HttpClient();

                // Get the response from ipinfo.io API
                HttpResponseMessage response = await client.GetAsync("https://ipinfo.io/?token=9a8523cd141797");
                string jsonResponse = await response.Content.ReadAsStringAsync();

                // Parse the JSON response into JObject
                var apiData = Newtonsoft.Json.Linq.JObject.Parse(jsonResponse);

                // Initialize the result list
                List<Advertisementservice> res = new List<Advertisementservice>();

                if (apiData != null)
                {
                    // Execute the stored procedure with parameters
                    res = _context.Database.SqlQueryRaw<Advertisementservice>(
                        "EXEC dbo.sp_AdvertisementManager @country = {0}, @state = {1}, @city = {2}, @action = {3}",
                        apiData["country"]?.ToString() ?? (object)DBNull.Value,  // Safely extract country, or DBNull if missing
                        apiData["region"]?.ToString() ?? (object)DBNull.Value,  // Safely extract region (state), or DBNull if missing
                        apiData["city"]?.ToString() ?? (object)DBNull.Value,    // Safely extract city, or DBNull if missing
                        3  // Hardcoded action parameter (could be dynamic if needed)
                    ).ToList();
                }
                res = res.OrderBy(e => Guid.NewGuid()).ToList();
                return res;
            }
            catch (Exception ex)
            {
                // Log the exception and rethrow or return an empty list
                Console.WriteLine($"An error occurred: {ex.Message}");
                return new List<Advertisementservice>(); // Return an empty list in case of error
            }
        }

        public async Task<List<Advertisementservice>> GetTopOneAdvertisementBanner()
        {
            try
            {
                // Initialize HttpClient
                HttpClient client = new HttpClient();

                // Get the response from ipinfo.io API
                HttpResponseMessage response = await client.GetAsync("https://ipinfo.io/?token=9a8523cd141797");
                string jsonResponse = await response.Content.ReadAsStringAsync();

                // Parse the JSON response into JObject
                var apiData = Newtonsoft.Json.Linq.JObject.Parse(jsonResponse);

                // Initialize the result list
                List<Advertisementservice> res = new List<Advertisementservice>();

                if (apiData != null)
                {
                    // Execute the stored procedure with parameters
                    res = _context.Database.SqlQueryRaw<Advertisementservice>(
                        "EXEC dbo.sp_AdvertisementManager @country = {0}, @state = {1}, @city = {2}, @action = {3}",
                        apiData["country"]?.ToString() ?? (object)DBNull.Value,  // Safely extract country, or DBNull if missing
                        apiData["region"]?.ToString() ?? (object)DBNull.Value,  // Safely extract region (state), or DBNull if missing
                        apiData["city"]?.ToString() ?? (object)DBNull.Value,    // Safely extract city, or DBNull if missing
                        4  // Hardcoded action parameter (could be dynamic if needed)
                    ).ToList();
                }
                return res;
            }
            catch (Exception ex)
            {
                // Log the exception and rethrow or return an empty list
                Console.WriteLine($"An error occurred: {ex.Message}");
                return new List<Advertisementservice>(); // Return an empty list in case of error
            }
        }

        public dynamic getUserServiceRevordForUser(int userId)
        {
            var res = _context.UserServiceRecord.Where(e => e.UserId == userId && e.Status && e.Action!=0).OrderByDescending(e => e.date).ToList();
            var totalAmountForCurrentYearByMonth = _context.UserServiceRecord
                .Where(e => e.Action == 2 && e.UserId == userId && e.Status).Sum(e => e.Count);
            var totalAmountForsYearByMonthForChat = _context.UserServiceRecord
    .Where(e => e.Action == 1 && e.UserId == userId && e.Status).Sum(e => e.Count);
            return new
            {
                totalCall = totalAmountForCurrentYearByMonth,
                totalChat = totalAmountForsYearByMonthForChat,
                serviceRecord = res
            };
        }

    }
}
