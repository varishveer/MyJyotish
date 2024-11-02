using BusinessAccessLayer.Abstraction;
using DataAccessLayer.DbServices;
using DataAccessLayer.Migrations;
using Microsoft.EntityFrameworkCore;
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
                                     VideoUrl = video.VideoUrl ,
                                     VideoTitle = video.VideoTitle,
                                     ImageUrl= video.ImageUrl,
                                     SerialNo = video.SerialNo
                                     
                                 }).OrderBy(order=>order.SerialNo)
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
                Status = jyotishRecord.Status,
                Otp = jyotishRecord.Otp,
                Experience = jyotishRecord.Experience,
                Pooja = jyotishRecord.Pooja,
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

        public List<JyotishModel> SearchAstrologer(string keyword)
        {


            var ignoredWords = new HashSet<string> { "best", "top", "in", "the", "a" };


            var keywords = keyword.Split(new[] {" " }, StringSplitOptions.RemoveEmptyEntries)
                                  .Where(k => !ignoredWords.Contains(k.ToLower()))
                                  .ToList();

            List<JyotishModel> RecordList = new List<JyotishModel>();

            foreach (var it in keywords)
            {
                var word = char.ToUpper(it[0]) + it.Substring(1).ToLower();

                var result = _context.JyotishRecords.Where(x => x.Role == "Jyotish")
              .Where(record => record.Name.Contains(word) ||
                               record.Expertise.Contains(word) ||
                               record.Language.Contains(word) ||
                               record.Country.Contains(word) ||
                               record.State.Contains(word) ||
                               record.City.Contains(word) || record.Specialization.Contains(word) || record.Pooja.Contains(word))

              .ToList();
                RecordList.AddRange(result);
            }
           

            return RecordList;
        }

        



        public List<IdImageViewModel> SliderImageList(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return null;
            }

            IQueryable<IdImageViewModel> query = null;

            switch (keyword)
            {
                case "HomePage":
                    query = _context.Sliders.Select(s => new IdImageViewModel
                    {
                        Id = s.Id,
                        ImageUrl = s.HomePage 
                    });
                    break;
                case "BookPoojaCategory":
                    query = _context.Sliders.Select(s => new IdImageViewModel
                    {
                        Id = s.Id,
                        ImageUrl = s.BookPoojaCategory 
                    });
                    break;
                case "PoojaList":
                    query = _context.Sliders.Select(s => new IdImageViewModel
                    {
                        Id = s.Id,
                        ImageUrl = s.PoojaList 
                    });
                    break;
                default:
                    return null; 
            }

            var records = query.ToList();
            return records.Count > 0 ? records : null;
        }

        public string BookAppointment(AppointmentViewModel model)
        {
            var Jyotish = _context.JyotishRecords.Where(x => x.Id == model.JyotishId).FirstOrDefault();
            var User = _context.Users.Where(x => x.Id == model.UserId).FirstOrDefault();
            var Slot = _context.AppointmentSlots.Where(x=>x.Id == model.SlotId).FirstOrDefault();
            if (User == null || Jyotish == null|| Slot==null ||Slot.Status == "Booked" ) { return "invalid Data"; }


            AppointmentModel appointment = new AppointmentModel();
            appointment.Date = Slot.Date;
            appointment.SlotId = model.SlotId;
            appointment.JyotishId = Jyotish.Id;
            appointment.UserId = User.Id;
            appointment.Problem = model.Problem;
            appointment.Amount = Jyotish.AppointmentCharges;
            appointment.Status = "Upcomming";
            Slot.Status = "Booked";
            _context.AppointmentSlots.Update(Slot);
            _context.AppointmentRecords.Add(appointment);
            var result = _context.SaveChanges();
            if (result > 0) { return "Successful"; }
            return "internal Server Error.";

        }
        
        public List<JyotishModel> SpecializationFilter(string Keyword)
        {
            Keyword = char.ToUpper(Keyword[0]) + Keyword.Substring(1).ToLower();
            var record = _context.JyotishRecords.Where(x => x.Specialization == Keyword).ToList();
            return record;
        }

        public UserModel GetUserProfile(int Id)
        {
            var user = _context.Users.Where(x => x.Id == Id).FirstOrDefault();
            if (user == null)
            { return null; }
            user.CallingModelRecord = null;
            user.ChattingModelRecord = null;
            return user;
        }
        public string UpdateProfile(UserUpdateViewModel model, string path)
        {
            if(model ==  null)
            { return "Invalid Data"; }

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

            if (!string.IsNullOrEmpty(model.DoB))
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
            var CountryName = _context.Countries.Where(x => x.Id == model.Country).FirstOrDefault();
            var StateName = _context.States.Where(x => x.Id == model.State).FirstOrDefault();
            var CityName = _context.Cities.Where(x => x.Id == model.City).FirstOrDefault();

            if (CountryName != null)
            {
               
                userModel.Country = CountryName.Name; 
            }

            if (StateName !=null)
            {
                userModel.State = StateName.Name; 
            }

            if (model.City.HasValue)
            {
                userModel.City = CityName.Name;
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
            if (_context.SaveChanges() > 0) { return "Successful"; }
            else { return "Internal Server Error"; }
  
        }

        public List<AppointmentDetailViewModel> getAllAppointment(int Id)
        {
            var User = _context.Users.Where(x => x.Id == Id).FirstOrDefault();
            if(User == null) { return null; }





            var Records = _context.AppointmentRecords.Where(x => x.UserId == Id)  // Filter by the userId
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
                       Amount = combined.record.Amount
                   })
             .ToList();
            return Records; 
        }
        public AppointmentModel GetAppointmentDetails(int Id)
        {
            var records = _context.AppointmentRecords.Where(x => x.Id == Id).FirstOrDefault();
            return records;
        }

        public List<UserPaymentRecordModel> UserPaymentrecords(int Id)
        {
            var User = _context.Users.Where(x => x.Id == Id).FirstOrDefault();
            if(User == null) { return null; }
            var record = _context.UserPaymentRecord.Where(x => x.UserId == Id).OrderBy(e=>e.Id).Reverse().ToList();
            if (record.Count > 0) { return record; }
            else { return null; }
        }
        public UserPaymentRecordModel UserPaymentDetail(int Id)
        {
          
            var record = _context.UserPaymentRecord.Where(x => x.Id == Id).FirstOrDefault();
            if (record!=null) { return record; }
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
            Random rnd = new Random();
            var rndnum = rnd.Next(100, 999);
            var paymentId = Jyotish.Name + DateTime.Now.ToString("ddMMyyyyhhmmss") + pr.UId + rndnum;
            var paymentUnqId = _context.WalletHistroy.Where(x => x.PaymentId == paymentId).FirstOrDefault();
            if (paymentUnqId != null)
            {
                paymentId = paymentId + rndnum;
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
                UId =pr.UId
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
                        {   Id =x.Id,
                            TimeDuration = x.TimeDuration,
                            TimeFrom = x.TimeFrom,
                            TimeTo = x.TimeTo,
                            JyotishId = x.JyotishId,
                            Status = x.Status
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
                        Problem = group.SelectMany(p => p.Problem?.Split(new[] { "$%^" }, StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>()).ToArray(),
                        Solution = group.SelectMany(p => p.Solution?.Split(new[] { "$%^" }, StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>()).ToArray()
                    };
                })
                .ToList();

            return data;
        }





        public ProblemSolutionGetAllViewModel GetProblemSolution(int id)
        {
            if (id == 0)
            {
                return null;
            }

            var result = _context.ProblemSolution
                .Where(x => x.Id == id)
                .Include(ps => ps.Appointment)
                    .ThenInclude(a => a.UserRecord)
                .Include(ps => ps.Appointment)
                    .ThenInclude(a => a.JyotishRecord)
                .AsEnumerable()
                .GroupBy(ps => new { ps.Appointment.UserId, ps.Appointment.JyotishId, ps.Appointment.Id })
                .Select(group =>
                {
                    var first = group.FirstOrDefault();
                    var slot = _context.AppointmentSlots.FirstOrDefault(s => s.Id == first.Appointment.SlotId);

                    return new ProblemSolutionGetAllViewModel
                    {
                        Id = first.Id,
                        UserId = group.Key.UserId,
                        JyotishId = group.Key.JyotishId,
                        JyotishName = first.Appointment.JyotishRecord.Name,
                        JyotishEmail = first.Appointment.JyotishRecord.Email,
                        AppointmentId = group.Key.Id,
                        Date = slot != null ? DateOnly.FromDateTime(slot.Date) : DateOnly.MinValue,
                        Time = slot != null ? slot.TimeFrom : TimeOnly.MinValue,
                        Problem = group.SelectMany(p => p.Problem?.Split(new[] { "$%^" }, StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>()).ToArray(),
                        Solution = group.SelectMany(p => p.Solution?.Split(new[] { "$%^" }, StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>()).ToArray()
                    };
                })
                .FirstOrDefault();

            return result;
        }





    }
}
