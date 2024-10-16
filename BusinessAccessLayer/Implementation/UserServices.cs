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
                                 .Select(video => new JyotishVideosViewModel
                                 {
                                     
                                     Id = video.Id,
                                     VideoUrl = video.VideoUrl ,
                                     VideoTitle = video.VideoTitle
                                 })
                                 .ToArray();

            var gallery = _context.JyotishGallery
                                  .Where(x => x.JyotishId == Id)
                                  .Select(image => new JyotishGalleryModel
                                  {
                                      Id = image.Id,
                                      ImageUrl = image.ImageUrl,
                                      ImageTitle = image.ImageTitle
                                  })
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
                Password = jyotishRecord.Password,
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

        /* public List<JyotishModel> SearchAstrologer(string keyword)
         {

             var ignoredWords = new HashSet<string> { "best", "top", "in", "the", "a" };


             var keywords = keyword.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                   .Where(k => !ignoredWords.Contains(k.ToLower()))
                                   .ToList();


             var query = _context.JyotishRecords.Where(x => x.Role == "Jyotish");

             foreach (var word in keywords)
             {
                 query = query.Where(record => record.Name.Contains(word) ||
                                               record.Expertise.Contains(word) ||
                                               record.Language.Contains(word) ||
                                               record.Specialization.Contains(word) ||
                                               record.Pooja.Contains(word) ||
                                               record.City.Contains(word) ||
                                               record.State.Contains(word) ||
                                               record.Country.Contains(word));
             }


             return query.ToList();
         }*/




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
            if (User == null || Jyotish == null) { return "invalid Data"; }
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

        public List<AppointmentModel> getAllAppointment(int Id)
        {
            var User = _context.Users.Where(x => x.Id == Id).FirstOrDefault();
            if(User == null) { return null; }
            var records = _context.AppointmentRecords.Where(x => x.UserId == Id).ToList();
             return records; 
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
            var record = _context.UserPaymentRecord.Where(x => x.UserId == Id).ToList();
            if (record.Count > 0) { return record; }
            else { return null; }
        }
        public UserPaymentRecordModel UserPaymentDetail(int Id)
        {
            var User = _context.Users.Where(x => x.Id == Id).FirstOrDefault();
            if (User == null) { return null; }
            var record = _context.UserPaymentRecord.Where(x => x.UserId == Id).FirstOrDefault();
            if (record==null) { return record; }
            else { return null; }
        }
    }
}
