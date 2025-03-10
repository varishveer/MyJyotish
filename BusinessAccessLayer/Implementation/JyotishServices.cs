﻿using Azure;
using BusinessAccessLayer.Abstraction;
using DataAccessLayer.DbServices;
using DataAccessLayer.Migrations;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging.Abstractions;
using ModelAccessLayer.Models;
using ModelAccessLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BusinessAccessLayer.Implementation
{
    public class JyotishServices : IJyotishServices
    {
        private readonly ApplicationContext _context;
        private readonly string _uploadDirectory;
        private readonly IAccountServices _account;
        public JyotishServices(ApplicationContext context, IAccountServices account)
        {
            _context = context;
            _uploadDirectory = Directory.GetCurrentDirectory();

            // Ensure directory exists
            if (!Directory.Exists(_uploadDirectory))
            {
                Directory.CreateDirectory(_uploadDirectory);
            }

            _account = account;
        }

        public string UpdateProfile(JyotishProfileUpdateViewModal model)
        {
            // Fetch the existing record from the database
            var existingRecord = _context.JyotishRecords.FirstOrDefault(x => x.Id == model.Id);
            if (existingRecord == null)
            {
                return "Jyotish Not Found";
            }
            _context.Entry(existingRecord).Reload();
            if (model.ProfileImageUrl != null)
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
            var StateName = _context.States.Where(x => x.Id == model.State).FirstOrDefault();
            var CityName = _context.Cities.Where(x => x.Id == model.City).FirstOrDefault();

            existingRecord.Mobile = model.Mobile;
            existingRecord.AlternateMobile = model.AlternateMobile;
            existingRecord.Name = model.Name;
            existingRecord.Gender = model.Gender;
            existingRecord.Language = model.Language;
            existingRecord.Expertise = model.Expertise;

            existingRecord.State = StateName.Name;
            existingRecord.City = CityName.Name;
            existingRecord.DateOfBirth = model.DateOfBirth;
            existingRecord.Experience = model.Experience;
            existingRecord.Call = model.Call;
            existingRecord.Appointment = model.Appointment;
            if (model.Call)
            {
                existingRecord.CallCharges = model.CallCharges;
            }
            else
            {
                existingRecord.CallCharges = 0;
            }
            existingRecord.Chat = model.Chat;
            if (model.Chat)
            {
                existingRecord.ChatCharges = model.ChatCharges;
            }
            else
            {
                existingRecord.ChatCharges = 0;
            }
            if (model.Appointment)
            {
                existingRecord.AppointmentCharges = model.AppointmentCharges;
            }
            else
            {
                existingRecord.AppointmentCharges = 0;
            }
            existingRecord.Pooja = model.Pooja;
            existingRecord.Address = model.Address;
            existingRecord.TimeTo = model.TimeTo;
            existingRecord.TimeFrom = model.TimeFrom;

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


        public List<SubscriptionFeatureViewModel> GetAllFeatures()
        {



            var record = _context.SubscriptionFeatures.Where(x => x.Status == true).OrderByDescending(e => e.FeatureId).Select(x => new SubscriptionFeatureViewModel
            {
                Id = x.FeatureId,
                Name = x.Name,
                ServiceUrl = x.ServiceUrl
            }).ToList();
            return record;
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
               Amount = combined.record.Amount,
               ArriveStatus = combined.record.ArrivedStatus
           })
     .ToList();  // Retrieve all records for the Jyotish



            var Records = allRecords.Where(x => x.Date > yesterdayDateOnly).ToList();

            return Records;
        }
        public string AddAppointment(AddAppointmentJyotishModel model)
        {

            var totalUsedServiceCount = _context.AppointmentRecords.Count(e => e.JyotishId == model.JyotishId && e.BookBy=="jyotish");
            var serviceCount = (from package in _context.PackageManager
                                join managepk in _context.ManageSubscriptionModels on package.SubscriptionId equals managepk.SubscriptionId
                                join feature in _context.SubscriptionFeatures on managepk.FeatureId equals feature.FeatureId
                                where package.JyotishId == model.JyotishId && package.Status && feature.ServiceUrl == "/Jyotish/AddAppointment"
                                select managepk.ServiceCount).FirstOrDefault();
            if (totalUsedServiceCount >= serviceCount)
            {
                return "Limit Exeeced";
            }

            var Jyotish = _context.JyotishRecords.Where(x => x.Id == model.JyotishId).FirstOrDefault();
            var User = _context.Users.FirstOrDefault(x => x.Email == model.Email || x.Mobile == model.Mobile);

            var appointmentRecord = _context.AppointmentRecords.Where(x => x.SlotId == model.SlotId).FirstOrDefault();

            var Slot = _context.AppointmentSlots.Where(x => x.Id == model.SlotId).FirstOrDefault();
            if (Jyotish == null || Slot == null || Slot.Status == "Booked" || appointmentRecord != null) { return "invalid Data"; }

            AppointmentModel appointment = new AppointmentModel();


            appointment.Date = DateTime.Now;


            appointment.JyotishId = model.JyotishId;
            appointment.SlotId = model.SlotId;

            if (User == null)
            {
                ModelAccessLayer.Models.UserModel userModel = new ModelAccessLayer.Models.UserModel();
                userModel.Email = model.Email;
                userModel.Mobile = model.Mobile;
                userModel.Name = model.Name;
                userModel.Country = model.country;
                userModel.CountryCodeId = _context.CountryCode.Where(e => e.country == model.country).Select(e=>e.Id).FirstOrDefault();
                userModel.Password = (new Random().Next(10000000, 100000000)).ToString();
                _context.Users.Add(userModel);
                if (_context.SaveChanges() > 0)
                { appointment.UserId = userModel.Id; }
                var message = $"Thanks for using Myjyotishg, <br> Here is you credential for login <br> Email : {model.Email} <br> Password : {userModel.Password}";
                _account.SendEmail(message, model.Email, "MyJyotishG login credential");
            }
            else { appointment.UserId = User.Id; }
            appointment.Problem = model.Problem;
            appointment.Amount = Jyotish.AppointmentCharges;
            appointment.Status = "Upcomming";
            appointment.ArrivedStatus = 0;
            appointment.BookMark = 0;
            appointment.BookBy = "jyotish";
            _context.AppointmentRecords.Add(appointment);
            Slot.Status = "Booked";
            _context.AppointmentSlots.Update(Slot);
            if (_context.SaveChanges() > 0)
            {
                return "Successful";
            }
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
            appointment.ArrivedStatus = 0;
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
            if (Jyotish == null)
            { return null; }
            else
            {
                var records = _context.TeamMemberRecords.Where(x => x.JyotishId == Jyotish.Id).ToList();
                return records;
            }

        }
        public string AddTeamMember(TeamMemberViewModel teamMember, string path)
        {
            var IsEmailValid = _context.TeamMemberRecords.Where(x => x.Email == teamMember.Email).FirstOrDefault();
            var IsMobileValid = _context.TeamMemberRecords.Where(x => x.Mobile == teamMember.Mobile).FirstOrDefault();
            if (IsEmailValid != null || IsMobileValid != null)
            { return "Email or Mobile no. Already Exist"; }

            var IsJyotishValid = _context.JyotishRecords.Where(x => x.Id == teamMember.Id).FirstOrDefault();
            if (IsJyotishValid == null)
            { return "Jyotish Not found"; }

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
                Password = newPassword
            };

            _context.TeamMemberRecords.Add(model);
            var result = _context.SaveChanges();
            if (result > 0)
            {
                _account.SendEmail(newPassword, teamMember.Email, "Password");
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

        public bool CreateAPooja(PoojaRecordModel model)
        {

           

            var isPoojaValid = _context.PoojaRecord.Where(x => x.PoojaType == model.PoojaType && x.status).FirstOrDefault();
            if (isPoojaValid != null)
            { return false; }
            _context.PoojaRecord.Add(model);
            int result = _context.SaveChanges();
            if (result > 0)
            { return true; }
            else { return false; }
        }

        public bool UpdatePooja(PoojaRecordModel model)
        {
            var isPoojaValid = _context.PoojaRecord.Where(x => x.Id == model.Id && x.status).FirstOrDefault();
            if (isPoojaValid == null)
            { return false; }
            isPoojaValid.PoojaType = model.PoojaType;
            isPoojaValid.title = model.title;
            isPoojaValid.Benefits = model.Benefits;
            isPoojaValid.Procedure = model.Procedure;
            isPoojaValid.AboutGod = model.AboutGod;
            if (model.Image != null)
            {
                isPoojaValid.Image = model.Image;
            }

            _context.PoojaRecord.Update(isPoojaValid);
            int result = _context.SaveChanges();
            if (result > 0)
            { return true; }
            else { return false; }
        }

        public bool removePooja(int Id)
        {
            var isPoojaValid = _context.PoojaRecord.Where(x => x.Id == Id && x.status).FirstOrDefault();
            if (isPoojaValid == null)
            { return false; }
            isPoojaValid.status = false;
            _context.PoojaRecord.Update(isPoojaValid);
            int result = _context.SaveChanges();
            if (result > 0)
            { return true; }
            else { return false; }
        }

        public dynamic getPoojaByJyotish(int Id)
        {
            var res = (from pooja in _context.PoojaRecord
                       join poojaType in _context.PoojaList on pooja.PoojaType equals poojaType.Id
                       where pooja.status && poojaType.Status
                       orderby pooja.Id descending
                       select new
                       {
                           id = pooja.Id,
                           poojaName = poojaType.Name,
                           poojaTitle = pooja.title,
                           benefits = pooja.Benefits,
                           procedure = pooja.Procedure,
                           aboutGod = pooja.AboutGod,
                           image = pooja.Image
                       }
                       ).ToList();

            return res;
        }

        public dynamic getBookedPoojaList(int jyotishId)
        {
            var res = (
                from bookpooja in _context.BookedPoojaList
                join user in _context.Users on bookpooja.userId equals user.Id
                join pooja in _context.PoojaRecord on bookpooja.PoojaId equals pooja.Id
                join poojaType in _context.PoojaList on pooja.PoojaType equals poojaType.Id
                join country in _context.Countries on user.Country equals country.Id
                join state in _context.States on user.State equals state.Id
                join city in _context.Cities on user.City equals city.Id
                where bookpooja.jyotishId == jyotishId && bookpooja.status  && !bookpooja.completeStatus
                orderby bookpooja.Id descending
                select new
                {
                    id=bookpooja.Id,
                    userId = user.Id,
                    poojaId = pooja.Id,
                    userName = user.Name,
                    mobile = user.Mobile,
                    poojaName = poojaType.Name,
                    bookingDate = bookpooja.BookingDate.ToString("dd-MM-yyyy hh:mm"),
                    poojaDate = bookpooja.PoojaDate.ToString("dd-MM-yyyy"),
                    address = city.Name + "," + state.Name + "," + country.Name
                }
                ).ToList();
            return res;
        } 
        public dynamic getBookedPoojaListWhichIsCompleted(int jyotishId)
        {
            var res = (
                from bookpooja in _context.BookedPoojaList
                join bookmark in _context.PoojaBookMark on bookpooja.Id equals bookmark.poojaId into leftbookmark from bookmark in leftbookmark.DefaultIfEmpty()
                join user in _context.Users on bookpooja.userId equals user.Id
                join pooja in _context.PoojaRecord on bookpooja.PoojaId equals pooja.Id
                join poojaType in _context.PoojaList on pooja.PoojaType equals poojaType.Id
                join country in _context.Countries on user.Country equals country.Id
                join state in _context.States on user.State equals state.Id
                join city in _context.Cities on user.City equals city.Id
                where bookpooja.jyotishId == jyotishId && bookpooja.status  && bookpooja.completeStatus 
                orderby bookpooja.Id descending
                select new
                {
                    id=bookpooja.Id,
                    userId = user.Id,
                    poojaId = pooja.Id,
                    userName = user.Name,
                    mobile = user.Mobile,
                    poojaName = poojaType.Name,
                    bookingDate = bookpooja.BookingDate.ToString("dd-MM-yyyy hh:mm"),
                    poojaDate = bookpooja.PoojaDate.ToString("dd-MM-yyyy"),
                    address = city.Name + "," + state.Name + "," + country.Name,
                    bookmarkId=bookmark!=null?bookmark.Id:0,
                    bookMarkStatus=bookmark!=null?bookmark.status:false
                }
                ).ToList();
            return res;
        }

        public dynamic getPoojaBookmark(int id)
        {
            var res = _context.PoojaBookMark.Where(e => e.Id == id && e.status).FirstOrDefault();
            return res;
        }

        public bool removePoojaBookmark(int id)
        {
            var res = _context.PoojaBookMark.Where(e => e.Id == id && e.status).FirstOrDefault();
            if (res == null)
            {
                return false;
            }
            res.status = false;
            _context.PoojaBookMark.Update(res);
            return _context.SaveChanges() > 0;
        }

        public bool AddPoojaBookMark(PoojaBookMarkService pms)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                PoojaBookMark pm = new PoojaBookMark
                {
                    poojaId = pms.poojaId,
                    BookMark = pms.BookMark,
                    EndDate = pms.EndDate,
                    status = true
                };
                _context.PoojaBookMark.Add(pm);
                if (_context.SaveChanges() > 0)
                {
                    var res = _context.BookedPoojaList.Where(e => e.Id == pms.poojaId && e.status).FirstOrDefault();
                    if (res == null)
                    {
                        transaction.Rollback();
                        return false;
                    }
                    res.completeStatus = true;
                    _context.BookedPoojaList.Update(res);
                    transaction.Commit();
                    return _context.SaveChanges() > 0;
                };
                return false;
            }catch(Exception ex)
            {
                transaction.Rollback();
                return false;
            }
        }

        public bool CompletePoojaContact(int id)
        {
            var res = _context.BookedPoojaList.Where(e => e.Id == id && e.status).FirstOrDefault();
            if (res == null)
            {
                return false;
            }
            res.completeStatus = true;
            _context.BookedPoojaList.Update(res);
            return _context.SaveChanges() > 0;
        }

        public dynamic poojaByPoojaId(int id)
        {
            var res = _context.PoojaRecord.Where(e => e.Id == id && e.status).FirstOrDefault();
            return res;
        }
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
            var Record = _context.Cities.Where(x => x.StateId == Id).ToList();
            return Record;
        }

        public List<ExpertiseModel> ExpertiseList()
        {
            var Records = _context.ExpertiseRecords.ToList();
            return Records;
        }
        public List<PoojaListModel> GetPoojaList()
        {
            var Records = _context.PoojaList.Where(e => e.Status).ToList();
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

            var totalUsedServiceCount = _context.JyotishVideos.Count(e => e.JyotishId == model.JyotishId && e.Status);
            var serviceCount = (from package in _context.PackageManager
                                join managepk in _context.ManageSubscriptionModels on package.SubscriptionId equals managepk.SubscriptionId
                                join feature in _context.SubscriptionFeatures on managepk.FeatureId equals feature.FeatureId
                                where package.JyotishId == model.JyotishId && package.Status && feature.ServiceUrl == "/Jyotish/Video"
                                select managepk.ServiceCount).FirstOrDefault();
            if (totalUsedServiceCount >= serviceCount)
            {
                return "Limit Exeeced";
            }


            var gallerySrCount = _context.JyotishVideos.Where(e => e.JyotishId == model.JyotishId && e.Status && Convert.ToInt32(e.SerialNo) >= Convert.ToInt32(model.SerialNo)).ToList();
            int srCount = 1;
            foreach (var item in gallerySrCount)
            {
                item.SerialNo = (Convert.ToInt32(model.SerialNo) + srCount).ToString();
                srCount++;
            }
            _context.JyotishVideos.UpdateRange(gallerySrCount);

            JyotishVideosModel data = new JyotishVideosModel();
            if (model.Image != null)
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
            data.Status = true;
            _context.JyotishVideos.Add(data);
            if (_context.SaveChanges() > 0) { return "Successful"; }
            else { return "internal server Error"; }
        }

        public bool UpdateVideo(JyotishVideosViewModel model)
        {
            var res = _context.JyotishVideos.Where(e => e.Id == model.Id && e.Status).FirstOrDefault();
            if (res == null)
            {
                return false;
            }
            var gallerySrCount = _context.JyotishVideos.Where(e => e.JyotishId == model.JyotishId && e.Status && Convert.ToInt32(e.SerialNo) >= Convert.ToInt32(model.SerialNo)).ToList();
            int srCount = 1;
            foreach (var item in gallerySrCount)
            {
                item.SerialNo = (Convert.ToInt32(model.SerialNo) + srCount).ToString();
                srCount++;
            }
            _context.JyotishVideos.UpdateRange(gallerySrCount);
            if (model.Image != null)
            {
                const long MaxFileSize = 5 * 1024 * 1024; // 5 MB
                var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
                if (ValidateFile(model.Image, MaxFileSize, allowedExtensions) == false)
                { return false; }
                var ProfileGuid = Guid.NewGuid().ToString();
                var SqlPath = "wwwroot/Images/Jyotish/Video" + ProfileGuid + model.Image.FileName;
                var ProfilePath = Path.Combine(_uploadDirectory, SqlPath);
                using (var stream = new FileStream(ProfilePath, FileMode.Create))
                {
                    model.Image.CopyTo(stream);
                }
                res.ImageUrl = "/Images/Jyotish/Video" + ProfileGuid + model.Image.FileName;
            }

            res.VideoTitle = model.VideoTitle;
            res.VideoUrl = model.VideoUrl;
            res.SerialNo = model.SerialNo;
            _context.JyotishVideos.Update(res);
            return _context.SaveChanges() > 0;
        }

        public string AddJyotishGallery(JyotishGalleryViewModel model)
        {
            if (model == null) { return "invalid data"; }
            var totalUsedServiceCount = _context.JyotishGallery.Count(e => e.JyotishId == model.JyotishId && e.Status);
            var serviceCount = (from package in _context.PackageManager
                                join managepk in _context.ManageSubscriptionModels on package.SubscriptionId equals managepk.SubscriptionId
                                join feature in _context.SubscriptionFeatures on managepk.FeatureId equals feature.FeatureId
                                where package.JyotishId == model.JyotishId && package.Status && feature.ServiceUrl == "/Jyotish/Gallery"
                                select managepk.ServiceCount).FirstOrDefault();
            if (totalUsedServiceCount >= serviceCount)
            {
                return "Limit Exeeced";
            }

            var gallerySrCount = _context.JyotishGallery.Where(e => e.JyotishId == model.JyotishId && e.Status && Convert.ToInt32(e.SerialNo) >= Convert.ToInt32(model.SerialNo)).ToList();
            int srCount = 1;
            foreach(var item in gallerySrCount)
            {
                item.SerialNo = (Convert.ToInt32(model.SerialNo) + srCount).ToString();
                srCount++;
            }
            _context.JyotishGallery.UpdateRange(gallerySrCount);
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

            data.Status = true;
            data.ImageTitle = model.ImageTitle;
            data.JyotishId = model.JyotishId;
            data.SerialNo = model.SerialNo;
            _context.JyotishGallery.Add(data);
            if (_context.SaveChanges() > 0) { return "Successful"; }
            else { return "internal server Error"; }
        }

        public bool UpdateGallery(JyotishGalleryViewModel model)
        {
            var res = _context.JyotishGallery.Where(e => e.Id == model.Id && e.Status).FirstOrDefault();
            if (res == null)
            {
                return false;
            }
            var gallerySrCount = _context.JyotishGallery.Where(e => e.JyotishId == model.JyotishId && e.Status && Convert.ToInt32(e.SerialNo) >= Convert.ToInt32(model.SerialNo)).ToList();
            int srCount = 1;
            foreach (var item in gallerySrCount)
            {
                item.SerialNo = (Convert.ToInt32(model.SerialNo) + srCount).ToString();
                srCount++;
            }
            _context.JyotishGallery.UpdateRange(gallerySrCount);
            if (model.ImageUrl != null)
            {
                const long MaxFileSize = 5 * 1024 * 1024; // 5 MB
                var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
                if (ValidateFile(model.ImageUrl, MaxFileSize, allowedExtensions) == false)
                { return false; }
                var ProfileGuid = Guid.NewGuid().ToString();
                var SqlPath = "wwwroot/Images/Jyotish/" + ProfileGuid + model.ImageUrl.FileName;
                var ProfilePath = Path.Combine(_uploadDirectory, SqlPath);
                using (var stream = new FileStream(ProfilePath, FileMode.Create))
                {
                    model.ImageUrl.CopyTo(stream);
                }
                res.ImageUrl = "/Images/Jyotish/" + ProfileGuid + model.ImageUrl.FileName;
            }

            res.ImageTitle = model.ImageTitle;
            res.SerialNo = model.SerialNo;
            _context.JyotishGallery.Update(res);
            return _context.SaveChanges() > 0;
        }

        public bool RemoveGallery(int id)
        {
            var res = _context.JyotishGallery.Where(e => e.Id == id).FirstOrDefault();
            if (res == null)
            {
                return false;
            
            }
            var ImagePath = Path.Combine(Directory.GetCurrentDirectory(), res.ImageUrl);
            if (Path.Exists(ImagePath))
            {
                System.IO.File.Delete(ImagePath);
            }

            res.Status = false;
            _context.JyotishGallery.Update(res);
            return _context.SaveChanges() > 0;
        }
         public bool RemoveVideo(int id)
        {
            var res = _context.JyotishVideos.Where(e => e.Id == id).FirstOrDefault();
            if (res == null)
            {
                return false;
            }
           
            res.Status = false;
            _context.JyotishVideos.Update(res);
            return _context.SaveChanges() > 0;
        }

       

        public List<JyotishVideosModel> JyotishVideos(int Id)
        {
            var records = _context.JyotishVideos.Where(x => x.JyotishId == Id && x.Status).OrderBy(x => x.SerialNo).ToList();
            return records;
        }
        public List<JyotishGalleryModel> JyotishGallery(int Id)
        {
            var records = _context.JyotishGallery.Where(x => x.JyotishId == Id && x.Status).OrderBy(x => x.SerialNo).ToList();
            return records;
        }

        public JyotishProfileUpdateViewModal GetProfile(int Id)
        {
            var record = _context.JyotishRecords.Where(x => x.Id == Id).FirstOrDefault();


            JyotishProfileUpdateViewModal Data = new JyotishProfileUpdateViewModal();
            Data.Id = record.Id;
            Data.Name = record.Name;
            Data.Email = record.Email;
            Data.Gender = record.Gender;
            Data.Language = record.Language;
            Data.Mobile = record.Mobile;
            Data.AlternateMobile = record.AlternateMobile;
            Data.Expertise = record.Expertise;
            Data.countryName = record.Country;
            Data.stateName = record.State;
            Data.cityName = record.City;
            Data.Country = _context.Countries.Where(e => e.Name == record.Country).Select(e => e.Id).FirstOrDefault();
            Data.State = _context.States.Where(e => e.Name == record.State).Select(e => e.Id).FirstOrDefault();
            Data.City = _context.Cities.Where(e => e.Name == record.City).Select(e => e.Id).FirstOrDefault();
            Data.DateOfBirth = (DateOnly)record.DateOfBirth;
            Data.ProfileImage = record.ProfileImageUrl;
            Data.Experience = record.Experience != null ? (int)record.Experience : 0;
            Data.Pooja = record.Pooja != null ? (bool)record.Pooja : false;
            Data.Call = record.Call != null ? (bool)record.Call : false;
            if (Data.Call)
            {
                Data.CallCharges = record.CallCharges;
            }

            Data.Chat = (bool)record.Chat;
            if (Data.Chat)
            {
                Data.ChatCharges = record.ChatCharges;
            }


            Data.Appointment = record.Appointment != null ? (bool)record.Appointment : false;
            if (record.Appointment != null)
            {
                Data.AppointmentCharges = record.AppointmentCharges;
            }

            Data.Address = record.Address;
            if (record.TimeFrom != null)
            {
                Data.TimeFrom = (TimeOnly)record.TimeFrom;

            }
            if (record.TimeTo != null)
            {
                Data.TimeTo = (TimeOnly)record.TimeTo;

            }
            Data.About = record.About;
            Data.AwordsAndAchievement = record.AwordsAndAchievement;
            Data.Specialization = record.Specialization;

            return Data;
        }

        public List<SubscrictionListJyotishViewModel> GetAllSubscription(int jyotishId)
        {
            var activeFeatures = _context.SubscriptionFeatures
                        .Where(x => x.Status == true)
                        .ToList();
            var pkManager = _context.PackageManager.Where(e => e.JyotishId == jyotishId && e.Status).Select(e => e.SubscriptionId).FirstOrDefault();
            var packageLimit= _context.Subscriptions.Where(e => e.SubscriptionId == pkManager).Select(e => e.Name).FirstOrDefault().ToLower() == "silver" ? 2 : _context.Subscriptions.Where(e => e.SubscriptionId == pkManager).Select(e => e.Name).FirstOrDefault().ToLower() == "gold"?1: _context.Subscriptions.Where(e => e.SubscriptionId == pkManager).Select(e => e.Name).FirstOrDefault().ToLower() == "platinum"?1:3;
            var records = _context.Subscriptions
                                  .Where(x => x.Status == true)
                                  .Select(subscription => new SubscrictionListJyotishViewModel
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
                                      Features = _context.SubscriptionFeatures
                                         .Where(x => x.Status == true).Select(x => new FeatureList
                                         {
                                             FeatureId = x.FeatureId,
                                             Name = x.Name,
                                             ServiceCount = 0,
                                             Status = false
                                         }).ToArray()
                                  }).OrderByDescending(e => e.SubscriptionId).Skip(3- packageLimit).ToList();

            var ManageSubscriptionData = _context.ManageSubscriptionModels.Where(x => x.Status == true).ToList();

            foreach (var data in records)
            {
                foreach (var feature in data.Features)
                {
                    // Get the matching record from ManageSubscriptionData
                    var NewData = ManageSubscriptionData
                                    .FirstOrDefault(x => x.SubscriptionId == data.SubscriptionId
                                                      && x.FeatureId == feature.FeatureId
                                                      && x.Status == true);
                    // Check if a matching record is found
                    if (NewData != null)
                    {
                        feature.Status = true;
                        feature.ServiceCount = NewData.ServiceCount;
                    }
                }
            }

            return records;
        }

        public bool upgradePackages(packages packages)
        {

            var plan = _context.Subscriptions.Where(e => e.SubscriptionId == packages.SubscriptionId).Select(e => e.PlanType).FirstOrDefault();
            var checkPrePlan = _context.PackageManager.Where(e => e.Status && e.JyotishId == packages.JyotishId).FirstOrDefault();
            var checkPrePlanforvalidation = _context.PackageManager.Where(e => e.Status && e.JyotishId == packages.JyotishId && e.SubscriptionId == packages.SubscriptionId).FirstOrDefault();

            if (plan == null || checkPrePlanforvalidation != null)
            {
                return false;
            }



            var purchaseDate = DateTime.Now;
            dynamic expiryDate = 0;
            if (plan.ToLower() == "yearly")
            {

                expiryDate = purchaseDate.AddYears(1);
            }
            else if (plan.ToLower() == "monthly")
            {
                expiryDate = purchaseDate.AddMonths(1);

            }
            else if (plan.ToLower() == "weekly")
            {
                expiryDate = purchaseDate.AddDays(7);

            }

            if (checkPrePlan != null)
            {
                checkPrePlan.SubscriptionId = packages.SubscriptionId;
                checkPrePlan.PurchaseDate = purchaseDate;
                checkPrePlan.ExpiryDate = expiryDate;
                checkPrePlan.JyotishId = checkPrePlan.JyotishId;
                checkPrePlan.Status = true;

                _context.PackageManager.Update(checkPrePlan);
            }
            else
            {
                SubsciptionManagementModel model = new SubsciptionManagementModel
                {
                    SubscriptionId = packages.SubscriptionId,
                    JyotishId = packages.JyotishId,
                    PurchaseDate = purchaseDate,
                    ExpiryDate = expiryDate,
                    Status = true,

                };

                _context.PackageManager.Add(model);
            }
            if (_context.SaveChanges() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<JyotishPaymentRecordModel> JyotishPaymentrecords(int Id)
        {
            var Jyotish = _context.JyotishRecords.Where(x => x.Id == Id).FirstOrDefault();
            if (Jyotish == null) { return null; }
            var record = _context.JyotishPaymentRecord.Where(x => x.JyotishId == Id).OrderBy(x => x.Id).Reverse().ToList();
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
            if (DateTime.Compare(model.Date, DateTime.Now) < 0)
            { return "Invalid Date"; }
            var Jyotish = _context.JyotishRecords.Where(x => x.Id == model.JyotishId).FirstOrDefault();
            var existingSlots = _context.AppointmentSlots
         .Where(x => x.JyotishId == model.JyotishId &&
                      x.Date >= model.Date)
         .Select(e => e.Date)
         .ToList();

            if (Jyotish == null) { return "Invalid Jyotish"; }
            if (DateTime.Compare(model.Date, model.DateTo) <= 90)
            {
                List<AppointmentSlotModel> slotsToAdd = new List<AppointmentSlotModel>();
                for (DateTime date = model.Date; date <= model.DateTo; date = date.AddDays(1))
                {
                    if (existingSlots.Contains(date.Date))
                    {
                        continue;
                    }


                    TimeOnly timeFrom = TimeOnly.FromDateTime(Convert.ToDateTime(model.TimeFrom));
                    TimeOnly timeTo = TimeOnly.FromDateTime(Convert.ToDateTime(model.TimeTo));

                    TimeOnly startTime = timeFrom;
                    TimeOnly endTime = timeTo;

                    bool isSaturdaySkipped = model.saturday == 1 && date.DayOfWeek == DayOfWeek.Saturday;
                    bool isSundaySkipped = model.sunday == 2 && date.DayOfWeek == DayOfWeek.Sunday;
                    bool isSkipDateMatched = model.skipDate != null && model.skipDate.ToString() != "1001-01-01" &&
                    DateTime.Compare((DateTime)model.skipDate, date) == 0;

                    for (TimeOnly time = startTime; time <= endTime; time = time.AddMinutes(model.TimeDuration))
                    {
                        var data = new AppointmentSlotModel
                        {
                            Date = date,
                            TimeFrom = time,
                            TimeTo = time.AddMinutes(model.TimeDuration),
                            JyotishId = model.JyotishId,
                            TimeDuration = model.TimeDuration,
                            Status = "Vacant",
                            ActiveStatus = (isSaturdaySkipped || isSundaySkipped || isSkipDateMatched) ? 0 : 1
                        };

                        slotsToAdd.Add(data);
                    }

                    // Batch insert
                }
                if (slotsToAdd.Count > 0)
                {
                    _context.AppointmentSlots.AddRange(slotsToAdd);
                }
            }
            if (_context.SaveChanges() > 0) { return "Successful"; }
            else { return "Data Not Saved"; }
        }

        public string RemoveSlotWithskipDates(AppointmentSlotViewModel model)
        {
            var Jyotish = _context.JyotishRecords.Find(model.JyotishId);
            if (Jyotish == null)
            {
                return "Invalid Jyotish";
            }

            var slots = _context.AppointmentSlots
                .Where(x => x.JyotishId == model.JyotishId && x.ActiveStatus == 1)
                .ToList();

            List<AppointmentSlotModel> slotsToUpdate = new List<AppointmentSlotModel>();

            DateTime? skipDate = model.skipDate;
            bool isSkipDateValid = skipDate.HasValue && skipDate.Value != new DateTime(1001, 1, 1);

            foreach (var slot in slots)
            {
                bool shouldDeactivate = false;

                if ((model.saturday == 1 && slot.Date.DayOfWeek == DayOfWeek.Saturday) ||
                    (model.sunday == 2 && slot.Date.DayOfWeek == DayOfWeek.Sunday) ||
                    (isSkipDateValid && slot.Date.Date == skipDate.Value.Date))
                {
                    shouldDeactivate = true;
                }

                if (shouldDeactivate)
                {
                    slot.ActiveStatus = 0;
                    slotsToUpdate.Add(slot);
                }
            }

            if (slotsToUpdate.Count > 0)
            {
                _context.AppointmentSlots.UpdateRange(slotsToUpdate);
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
            if (Jyotish == null) { return 0; }
            else
            {
                return Jyotish.WalletAmount;
            }
        }

        public string PurchaseWithJyotishWallets(JyotishWalletViewmodel uw)
        {
            var users = _context.JyotishRecords.Where(x => x.Id == uw.jyotishId).FirstOrDefault();
            if (users == null) { return null; }
            var Jyotishwallet = _context.JyotishWallets.Where(x => x.jyotishId == uw.jyotishId).FirstOrDefault();
            var res = _context.RedeamCode.Where(e => e.jyotishId == uw.jyotishId && e.status).ToList();
            if (Jyotishwallet.WalletAmount > uw.WalletAmount)
            {
                Jyotishwallet.WalletAmount -= uw.WalletAmount;
                _context.JyotishWallets.Update(Jyotishwallet);
                if (_context.SaveChanges() > 0)
                {

                    if (res != null)
                    {
                        foreach (var item in res)
                        {

                            item.status = false;
                        }
                        _context.RedeamCode.UpdateRange(res);
                    }

                    return "Successful";

                }
                else
                {
                    return "Data Not Saved";
                }
            }
            else
            {
                return "Lower Balance";

            }

        }

        //add wallet History
        public string AddWalletHistory(WalletHistoryViewmodel pr)
        {
            var Jyotish = _context.JyotishRecords.Where(x => x.Id == pr.JId).FirstOrDefault();
            if (Jyotish == null) { return null; }
            string paymentId = pr.PaymentId;
            if (pr.PaymentId == null)
            {
                Random rnd = new Random();
                var rndnum = rnd.Next(100, 999);
                paymentId = Jyotish.Name.Split(' ')[0] + DateTime.Now.ToString("ddMMyyyy") + pr.JId + rndnum;
                var paymentUnqId = _context.WalletHistroy.Where(x => x.PaymentId == paymentId).FirstOrDefault();
                if (paymentUnqId != null)
                {
                    paymentId = paymentId + 1;
                }
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
                PaymentBy = "jyotish",
                PaymentFor = pr.PaymentFor,
                UId = pr.UId != 0 ? pr.UId : null
            };
            _context.WalletHistroy.Add(jw);
            if (_context.SaveChanges() > 0) { return "Successful"; }
            else { return "Data Not Saved"; }

        }
        public dynamic GetWalletHistory(int JyotishId)
        {
            var Jyotish = (from wallet in _context.WalletHistroy
                           join user in _context.Users on wallet.UId equals user.Id into userGroup
                           from user in userGroup.DefaultIfEmpty()
                           where wallet.JId == JyotishId
                           orderby wallet.Id descending
                           select new
                           {
                               UserName = wallet.UId != null ? user.Name : null,
                               paymentDate = wallet.date,
                               amount = wallet.amount,
                               paymentId = wallet.PaymentId,
                               paymentBy = wallet.PaymentBy,
                               paymentAction = wallet.PaymentAction,
                               profile = wallet.UId != null ? user.ProfilePictureUrl : null,
                               paymentFor = wallet.PaymentFor,
                               paymentStatus = wallet.PaymentStatus,
                               Id = wallet.Id
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

            var slot = _context.AppointmentSlots.Where(x => x.Id == model.Id).Where(x => x.JyotishId == model.JyotishId).FirstOrDefault();
            if (slot == null)
            {
                return "Invalid Data";
            }
            if (slot.Status == "Booked")
            { return "Slot Has Been Booked It Can't Be Updated."; }
            TimeOnly timeFrom = TimeOnly.FromDateTime(Convert.ToDateTime(model.TimeFrom));
            TimeOnly timeTo = TimeOnly.FromDateTime(Convert.ToDateTime(model.TimeTo));
            slot.Date = model.Date;
            slot.TimeFrom = timeFrom;
            slot.TimeTo = timeTo;

            slot.TimeDuration = model.TimeDuration;
            slot.Status = "Vacant";
            _context.AppointmentSlots.Update(slot);
            if (_context.SaveChanges() > 0) { return "Successful"; }
            else { return "Data Not Saved"; }
        }


        public string DeleteAppointmentSlot(int Id)
        {
            var slot = _context.AppointmentSlots.Where(x => x.Id == Id).FirstOrDefault();
            if (slot == null) { return "Invalid Id"; }

            _context.AppointmentSlots.Remove(slot);
            if (_context.SaveChanges() > 0) { return "Successful"; }
            else { return "Internal Server Error."; }
        }

        public List<AppointmentSlotUserViewModel> GetAllAppointmentSlot(int id)
        {
            var result = (from slot in _context.AppointmentSlots
                          where slot.JyotishId == id && slot.ActiveStatus == 1 && slot.Status == "Vacant" && slot.Date >= DateTime.Now
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
                                              JyotishId = (int)s.JyotishId,
                                              Status = s.Status
                                          }).ToList()

                          }
                          ).ToList();

            return result;
        }

        public List<AppointmentSlotUserViewModel> GetAllbookedAppointment(int id)
        {
            var result = (from slot in _context.AppointmentSlots
                          where slot.JyotishId == id && slot.ActiveStatus == 1 && slot.Status == "Booked"
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
                                              JyotishId = (int)s.JyotishId,
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
                where (appointmentRecords.JyotishId == jyotishId
                && DateTime.Compare(slots.Date.Date, DateTime.Now.Date) == 0)
                select new
                {
                    appId = appointmentRecords.Id,
                    uId = appointmentRecords.UserId,
                    userName = user.Name,
                    userMobile = user.Mobile,
                    problem = appointmentRecords.Problem,
                    userEmail = user.Email,
                    userProfile = user.ProfilePictureUrl,
                    appoinDate = slots.Date,
                    appoinTimeFrom = slots.TimeFrom,
                    appoinTimeTo = slots.TimeTo

                }
                );


            return result;
        }


        public dynamic GetAllUpcommingAppointment(int jyotishId)
        {

            var checkBookmarkValidation = _context.AppointmentBookmark.Where(e => e.JyotishId == jyotishId && e.EndDate <= DateOnly.FromDateTime(DateTime.Now) && e.EndDate != null && e.Status).ToList();
            if (checkBookmarkValidation.Any())
            {
                List<AppointmentModel> appointment = new List<AppointmentModel>();
                foreach (var item in checkBookmarkValidation)
                {
                    item.Status = false;
                    var appointmentRecords = _context.AppointmentRecords.Where(e => e.Id == item.AppointmentId).FirstOrDefault();
                    appointmentRecords.BookMark = 0;
                    appointment.Add(appointmentRecords);

                }

                _context.AppointmentBookmark.UpdateRange(checkBookmarkValidation);
                if (_context.SaveChanges() > 0)
                {
                    _context.AppointmentRecords.UpdateRange(appointment);
                    _context.SaveChanges();
                }

            }

            var result = (
                from appointmentRecords in _context.AppointmentRecords
                join user in _context.Users
                    on appointmentRecords.UserId equals user.Id
                join slots in _context.AppointmentSlots
                    on appointmentRecords.SlotId equals slots.Id

                where (appointmentRecords.JyotishId == jyotishId
                       && DateTime.Compare(slots.Date.Date, DateTime.Now.Date) >= 0)
                orderby appointmentRecords.Id descending
                select new
                {
                    Id = appointmentRecords.Id,
                    uId = appointmentRecords.UserId,
                    userName = user.Name,
                    userMobile = user.Mobile,
                    problem = appointmentRecords.Problem,
                    userEmail = user.Email,
                    userProfile = user.ProfilePictureUrl,
                    appoinDate = slots.Date.ToString("dd-MM-yyyy"),
                    appoinTimeFrom = slots.TimeFrom,
                    appoinTimeTo = slots.TimeTo,
                    amount = appointmentRecords.Amount,
                    arriveStatus = appointmentRecords.ArrivedStatus,
                    BookMark = appointmentRecords.BookMark,
                    currentDate = DateTime.Now.ToString("dd-MM-yyyy"),
                    memberList = (
            from m in _context.ClientMembers
            where m.UId == appointmentRecords.UserId && m.status == 1
            select new
            {
                memberId = m.Id,
                memberName = m.Name,
                memberRelation = m.relation,
                memberDob = m.dob,
                memberGender = m.gender
            }
        ).ToList()

                }
                ).ToList();


            return result;
        }

        public dynamic GetUpcommingAppointmentById(int appointmentId)
        {
            var result = (
                from appointmentRecords in _context.AppointmentRecords
                join user in _context.Users
                    on appointmentRecords.UserId equals user.Id
                join slots in _context.AppointmentSlots
                    on appointmentRecords.SlotId equals slots.Id

                where (appointmentRecords.Id == appointmentId)
                orderby appointmentRecords.Id descending
                select new
                {
                    Id = appointmentRecords.Id,
                    uId = appointmentRecords.UserId,
                    userName = user.Name,
                    userMobile = user.Mobile,
                    problem = appointmentRecords.Problem,
                    userEmail = user.Email,
                    userProfile = user.ProfilePictureUrl,
                    appoinDate = slots.Date.ToString("dd-MM-yyyy"),
                    appoinTimeFrom = slots.TimeFrom,
                    appoinTimeTo = slots.TimeTo,
                    amount = appointmentRecords.Amount,
                    arriveStatus = appointmentRecords.ArrivedStatus,
                    BookMark = appointmentRecords.BookMark,
                    currentDate = DateTime.Now.ToString("dd-MM-yyyy"),

                }
                ).FirstOrDefault();


            return result;
        }

        public dynamic GetAllAppointmentHistory(int jyotishId)
        {
            var result = (
                from appointmentRecords in _context.AppointmentRecords
                join user in _context.Users
                    on appointmentRecords.UserId equals user.Id
                join slots in _context.AppointmentSlots
                    on appointmentRecords.SlotId equals slots.Id

                where (appointmentRecords.JyotishId == jyotishId
                       && DateTime.Compare(slots.Date.Date, DateTime.Now.Date) < 0)
                select new
                {
                    Id = appointmentRecords.Id,
                    uId = appointmentRecords.UserId,
                    userName = user.Name,
                    userMobile = user.Mobile,
                    problem = appointmentRecords.Problem,
                    userEmail = user.Email,
                    userProfile = user.ProfilePictureUrl,
                    appoinDate = slots.Date.ToString("dd-MM-yyyy"),
                    appoinTimeFrom = slots.TimeFrom,
                    appoinTimeTo = slots.TimeTo,
                    amount = appointmentRecords.Amount,
                    arriveStatus = appointmentRecords.ArrivedStatus,
                    BookMark = appointmentRecords.BookMark,
                    currentDate = DateTime.Now.ToString("dd-MM-yyyy"),
                    memberList = (
            from m in _context.ClientMembers
            where m.UId == appointmentRecords.UserId && m.status == 1
            select new
            {
                memberId = m.Id,
                memberName = m.Name,
                memberRelation = m.relation,
                memberDob = m.dob,
                memberGender = m.gender
            }
        ).ToList()

                }
                ).ToList();


            return result;
        }

        public bool updateArrivedClient(int appointmentId, int jyotishId)
        {
            var jyotish = _context.JyotishRecords.Where(e => e.Id == jyotishId).FirstOrDefault();
            if (jyotish == null)
            {
                return false;
            }
            var appointentDetail = _context.AppointmentRecords.Where(e => e.Id == appointmentId).FirstOrDefault();
            if (appointentDetail != null)
            {
                appointentDetail.ArrivedStatus = 1;
                _context.AppointmentRecords.Update(appointentDetail);
                if (_context.SaveChanges() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }



        public string AddProblemSolution(ProblemSolutionViewModel model)
        {
            if (model == null)
            {
                return "No data provided";
            }


            var appointment = _context.AppointmentRecords
                                         .FirstOrDefault(x => x.Id == model.AppointmentId);
            if (appointment == null)
            {
                return "Invalid Data: AppointmentId does not exist.";
            }
            //if (appointment.Date > DateTime.Now)
            //{
            //    return "Appointment is Upcoming";
            //}
            if (model.Problem.Length > 10)
            {
                return "Problem or Solution Limit Exceed";
            }
            string member = model.memberId != 0 ? model.memberId.ToString() : null;
            var isValid = _context.ProblemSolution.Where(x => x.AppointmentId == model.AppointmentId && x.memberId.ToString() == member).FirstOrDefault();
            if (isValid != null)
            {
                return "Record Already Present.";
            }
            string problems = "";
            foreach (var it in model.Problem)
            {
                problems += it + "$%^";
            }


            ProblemSolutionModel data = new ProblemSolutionModel
            {
                AppointmentId = appointment.Id,
                memberId = model.memberId == 0 ? null : model.memberId,
                Problem = problems,
                Solution = model.Solution
            };
            _context.ProblemSolution.Add(data);
            return _context.SaveChanges() > 0 ? "Successful" : "Internal Server Error";
        }


        public bool AddClientMembers(ClientMembersViewModel model)
        {
            var Appointment = _context.AppointmentRecords.Where(x => x.UserId == model.UId).FirstOrDefault();
            if (Appointment != null)
            {
                ClientMembers memebers = new ClientMembers
                {
                    Name = model.Name,
                    dob = model.dob,
                    gender = model.gender,
                    relation = model.relation,
                    status = 1,
                    UId = model.UId,
                    JId = model.JId
                };

                _context.ClientMembers.Add(memebers);
                if (_context.SaveChanges() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        public dynamic getClientMembers(int Id, int jyotishId)
        {
            var Appointment = _context.AppointmentRecords.Where(x => x.UserId == Id).FirstOrDefault();
            if (Appointment != null)
            {
                var clientMember = (
                    from user in _context.Users
                    where user.Id == Id
                    select new
                    {
                        uId = user.Id,
                        userName = user.Name,
                        memberList = (
            from m in _context.ClientMembers
            where m.UId == Id && m.status == 1 && m.JId == jyotishId
            orderby m.Id descending

            select new
            {
                id = m.Id,
                name = m.Name,
                relation = m.relation,
                dob = m.dob,
                gender = m.gender
            }
        ).ToList()
                    }
                    ).FirstOrDefault();

                return clientMember;
            }
            else
            {
                return "not found";
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
                }).OrderByDescending(e => e.Id)
                .ToList();

            return data;
        }
        public List<AppointmentDetailViewModel> GetAllProblemSolutionByUser(int jyotishId, int UId)
        {
            var data = _context.ProblemSolution
                .Include(x => x.Appointment)
                    .ThenInclude(y => y.UserRecord)
                .Where(x => x.Appointment.JyotishId == jyotishId && x.Appointment.UserId == UId)
                .Select(x => new
                {
                    ProblemSolution = x,
                    AppointmentSlot = _context.AppointmentSlots.FirstOrDefault(s => s.Id == x.Appointment.SlotId)
                })
                .Select(x => new AppointmentDetailViewModel
                {

                    UserName = x.ProblemSolution.Appointment.UserRecord.Name,
                    UserId = x.ProblemSolution.Appointment.UserRecord.Id,
                    Date = x.AppointmentSlot.Date,
                    Time = x.AppointmentSlot != null ? x.AppointmentSlot.TimeFrom : TimeOnly.MinValue,
                    AppointmentId = x.ProblemSolution.AppointmentId
                }).Distinct().OrderByDescending(e => e.Date)
                .ToList();

            return data;
        }

        public bool BookMark(int appointmentId)
        {
            var res = _context.AppointmentRecords.Where(e => e.Id == appointmentId).FirstOrDefault();

            if (res == null)
            {
                return false;
            }

            res.BookMark = res.BookMark == 1 ? 0 : 1;
            _context.AppointmentRecords.Update(res);
            return _context.SaveChanges() > 0;
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
            string Solutions = model.Solution;
            foreach (var it in model.Problem)
            {
                problems += it + "$%^";
            }


            existingRecord.Problem = problems;
            existingRecord.Solution = Solutions;

            _context.ProblemSolution.Update(existingRecord);

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
                           Amount = combined.record.Amount,
                           currentDate = DateTime.Now.Date.ToString("dd-MM-yyyy")
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
                if (model.JyotishId <= 0 || model.UserId <= 0 || model.ImageUrl == null)
                {
                    return "Invalid data provided for the attachment.";
                }

                var attachmentRecord = new JyotishUserAttachmentModel
                {
                    JyotishId = model.JyotishId,
                    UserId = model.UserId,
                    Image = model.ImageUrl,
                    Title = model.Title,
                    AppointmentId = model.appointmentId,
                    MemberId = model.member != 0 ? model.member : null,
                    Status = true


                };

                _context.JyotishUserAttachmentRecord.Add(attachmentRecord);


                return _context.SaveChanges() > 0 ? "Successful" : "Failed to save attachments.";
            }
            catch (Exception)
            {
                return "Internal Server Error";
            }
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

        public List<JyotishUserAttachmentJyotishViewModel> GetAllUserAttachments(int Id)
        {
            if (Id <= 0)
            {
                return new List<JyotishUserAttachmentJyotishViewModel>();
            }

            var record = _context.JyotishUserAttachmentRecord
                                 .Where(x => x.JyotishId == Id && x.Status)
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

            //// Optionally, delete the physical file if needed
            //string filePath = Path.Combine("/wwwroot", attachment.Image.TrimStart('/'));
            //if (File.Exists(filePath))
            //{
            //    File.Delete(filePath);
            //}

            // Remove the record from the database
            attachment.Status = false;
            _context.JyotishUserAttachmentRecord.Update(attachment);

            return _context.SaveChanges() > 0 ? "Successful" : "Deletion failed.";
        }


        public List<AppointmentSlotDetailsJyotish> AppointmentSlotDetails(int Id)
        {

            var Jyotish = _context.JyotishRecords.Where(x => x.Id == Id).FirstOrDefault();


            var appointmentSlots = _context.AppointmentSlots
                .Where(slot => slot.JyotishId == Id && slot.Date >= DateTime.Now)
                .OrderBy(slot => slot.Date)
                .ToList();


            if (!appointmentSlots.Any())
                return null;


            List<AppointmentSlotDetailsJyotish> records = new List<AppointmentSlotDetailsJyotish>();


            AppointmentSlotDetailsJyotish newRecord = new AppointmentSlotDetailsJyotish();

            for (int i = 0; i < appointmentSlots.Count; i++)
            {

                if (i == 0)
                {
                    newRecord.dateFrom = DateOnly.FromDateTime(appointmentSlots[i].Date);
                    newRecord.timeFrom = appointmentSlots[i].TimeFrom;
                }
                else
                {

                    if (appointmentSlots[i].TimeDuration != appointmentSlots[i - 1].TimeDuration)
                    {

                        newRecord.timeTo = appointmentSlots[i - 1].TimeTo;
                        newRecord.timeDuration = appointmentSlots[i - 1].TimeDuration;
                        newRecord.dateTo = DateOnly.FromDateTime(appointmentSlots[i - 1].Date);


                        records.Add(newRecord);


                        newRecord = new AppointmentSlotDetailsJyotish
                        {
                            dateFrom = DateOnly.FromDateTime(appointmentSlots[i].Date),
                            timeFrom = appointmentSlots[i].TimeFrom
                        };
                    }
                }


                if (i == appointmentSlots.Count - 1)
                {
                    newRecord.timeTo = appointmentSlots[i].TimeTo;
                    newRecord.timeDuration = appointmentSlots[i].TimeDuration;
                    newRecord.dateTo = DateOnly.FromDateTime(appointmentSlots[i].Date);
                    records.Add(newRecord);
                }
            }

            return records;
        }


        public dynamic SkipDateList(int Id)
        {

            var appointmentSlots = _context.AppointmentSlots
            .Where(x => x.JyotishId == Id && x.ActiveStatus == 0 && x.Date >= DateTime.Now)
            .GroupBy(x => x.Date.Date)
            .Select(group => new 
            {
                Date = group.Key,  
                TimeDuration = group.First().TimeDuration
            })
            .ToList().OrderByDescending(e=>Convert.ToDateTime(e.Date)).ToList();
            return appointmentSlots;
        }

        public List<JyotishNotificationDataViewModel> NotificationData(int Id)
        {
            var data = _context.AppointmentRecords
            .Where(x => x.JyotishId == Id)
            .OrderByDescending(x => x.Id)
            .Take(5)
            .Include(a => a.UserRecord)  // Include related UserRecord
            .Include(a => a.AppointmentSlotData) // Include related AppointmentSlot data
            .Select(x => new JyotishNotificationDataViewModel
            {

                Name = x.UserRecord.Name,
                BookingDate = x.Date.ToString("dd-MM-yyyy HH:mm"),
                AppointmentDate = x.AppointmentSlotData.Date.ToString("dd-MM-yyyy"),
                AppointmentTime = x.AppointmentSlotData.TimeFrom.ToString(@"hh\:mm"),
                TimeDuration = x.AppointmentSlotData.TimeDuration
            })
            .ToList();

            return data;
        }


        public LayoutDataViewModel LayoutData(int Id)
        {
            var record = _context.JyotishRecords.Where(x => x.Id == Id && x.Status == true && x.Role == "Jyotish").FirstOrDefault();
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
                    email = record.Email,
                    Image = record.ProfileImageUrl
                };
                return layoutData;
            }
        }

        public dynamic getPlan(int Id)
        {
            var data = (from plan in _context.PackageManager
                        join subscription in _context.Subscriptions on plan.SubscriptionId equals subscription.SubscriptionId
                        join mngsub in _context.ManageSubscriptionModels on plan.SubscriptionId equals mngsub.SubscriptionId
                        join feature in _context.SubscriptionFeatures on mngsub.FeatureId equals feature.FeatureId
                        where plan.JyotishId == Id && plan.Status && subscription.Status && feature.Status && mngsub.Status
                        select new
                        {
                            planName = subscription.Name,
                            planId = subscription.SubscriptionId,
                            url = feature.ServiceUrl,
                            serviceName = feature.Name,
                            serviceCount = mngsub.ServiceCount
                        }
                       ).ToList();


            return data;
        }

        //country Code
        public int countryCode(int country)
        {
            var res = _context.CountryCode.Where(e => e.country == country).Select(e => e.countryCode).FirstOrDefault();
            return res;
        }

        public float purchaseWithReadmCode(string redeamCode, int JyotishId, int planId)
        {
            var res = _context.RedeamCode.Where(e => e.ReadeamCode == redeamCode && e.PlanId == planId && e.jyotishId == JyotishId && e.status && e.appstatus).FirstOrDefault();

            if (res == null)
            {
                return 0;
            }
            if (DateTime.Compare(DateTime.Now.Date, res.endDate.Date) <= 0)
            {
                return res.discountAmount;

            }
            else
            {
                res.status = false;
                _context.RedeamCode.Update(res);
                return 0;
            };


        }

        public dynamic getRedeemCode(int jyotishId)
        {
            var res = (from redeem in _context.RedeamCode
                       join plan in _context.Subscriptions on redeem.PlanId equals plan.SubscriptionId
                       where redeem.jyotishId == jyotishId && redeem.status && plan.Status && redeem.appstatus
                       select new
                       {
                           redeemCode = redeem.ReadeamCode,
                           expiryDate = redeem.endDate.ToString("dd-MM-yyyy"),
                           discountAmount = redeem.discountAmount,
                           discount = redeem.discount,
                           planName = plan.Name,
                           planType = plan.PlanType
                       }).ToList();
            return res;
        }

        public JyotishDashboardDataViewModel JyotishDashboardData(int Id)
        {
            var Jyotish = _context.JyotishRecords.Where(x => x.Id == Id).FirstOrDefault();
            if (Jyotish == null)
            { return null; }

            var AppointmentClient = _context.AppointmentRecords.Where(x => x.JyotishId == Id).Include(x => x.UserRecord).Select(x => x.UserRecord.Id).Distinct().ToList();
            var ChatClient = _context.ChatedUser.Where(x => x.JyotishId == Id).Include(x => x.User).Select(x => x.User.Id).Distinct().ToList();

            var TotalClient = AppointmentClient.Union(ChatClient).ToList().Distinct().Count();

            var TodayAppointment = _context.AppointmentRecords.Where(x => x.JyotishId == Id && x.Date == DateTime.Now).Count();
            var UpcommingAppointment = _context.AppointmentRecords.Where(x => x.JyotishId == Id && x.Date > DateTime.Now).Count();
            var TotalCallTime = 0;
            var TotalChatTime = 0;
            var TotalPooja = 0;
            var TotalTeamMember = 0;
            var TotalRating = _context.JyotishRating.Where(x => x.JyotishId == Id && x.Status == true).Count();
            JyotishDashboardDataViewModel data = new JyotishDashboardDataViewModel();

            data.TotalClient = TotalClient;
            data.TodayAppointment = TodayAppointment;
            data.UpcommingAppointment = UpcommingAppointment;
            data.TotalCallTime = TotalCallTime;
            data.TotalChatTime = TotalChatTime;
            data.TotalTeamMember = TotalTeamMember;
            data.TotalRating = TotalRating;

            return data;

        }

        //send request for redeem code
        public bool SendRequest(RedeemCodeRequestViewModel model)
        {
            var res = _context.RedeemCodeRequest.Where(e => e.jyotishId == model.jyotishId && e.planId == model.planId && e.status && !e.RedeemStatus).FirstOrDefault();
            if (res != null)
            {
                return false;
            }

            RedeemCodeRequest request = new RedeemCodeRequest
            {
                jyotishId = model.jyotishId,
                planId = model.planId,
                status = true,
                RedeemStatus = false,
                RequestDate = DateTime.Now
            };
            _context.RedeemCodeRequest.Add(request);
            return _context.SaveChanges() > 0;
        }

        public bool AddAppointmentBookmark(AppointmentBookmarkViewModal modal)
        {
            var Trasaction = _context.Database.BeginTransaction();
            var Appointment = _context.AppointmentRecords.Where(x => x.Id == modal.AppointmentId).FirstOrDefault();
            if (Appointment == null) { return false; }

            var AppointmentBookmark = _context.AppointmentBookmark.Where(x => x.AppointmentId == modal.AppointmentId).FirstOrDefault();
            if (AppointmentBookmark != null)
            {
                AppointmentBookmark.EndDate = modal.EndDate;
                AppointmentBookmark.Reason = modal.Reason;
                AppointmentBookmark.Status = true;
                _context.AppointmentBookmark.Update(AppointmentBookmark);
                if (_context.SaveChanges() > 0)
                {
                    Appointment.BookMark = 1;
                    _context.AppointmentRecords.Update(Appointment);
                    if (_context.SaveChanges() > 0)
                    {
                        Trasaction.Commit();
                        return true;
                    }
                    else
                    {
                        Trasaction.Rollback();
                        return false;
                    }
                }
                else { return false; }
            }
            else
            {
                AppointmentBookmarkModal Data = new AppointmentBookmarkModal();
                Data.EndDate = modal.EndDate;
                Data.Reason = modal.Reason;
                Data.AppointmentId = modal.AppointmentId;
                Data.JyotishId = modal.JyotishId;
                Data.Status = true;
                _context.AppointmentBookmark.Add(Data);
                if (_context.SaveChanges() > 0)
                {
                    Appointment.BookMark = 1;
                    _context.AppointmentRecords.Update(Appointment);
                    if (_context.SaveChanges() > 0)
                    {
                        Trasaction.Commit();
                        return true;
                    }
                    else
                    {
                        Trasaction.Rollback();
                        return false;
                    }


                }
                else { return false; }
            }


        }
        public bool DeleteAppointmentBookmark(int Id)
        {
            var Trasaction = _context.Database.BeginTransaction();
            var Appointment = _context.AppointmentRecords.Where(x => x.Id == Id).FirstOrDefault();
            if (Appointment == null)
            { return false; }

            Appointment.BookMark = 0;
            _context.AppointmentRecords.Update(Appointment);
            if (_context.SaveChanges() > 0)
            {
                var AppointmentBookmark = _context.AppointmentBookmark.Where(x => x.AppointmentId == Appointment.Id).FirstOrDefault();
                if (AppointmentBookmark == null)
                {
                    Trasaction.Rollback();
                    return false;
                }

                AppointmentBookmark.Status = false;
                _context.AppointmentBookmark.Update(AppointmentBookmark);
                if (_context.SaveChanges() > 0)
                {
                    Trasaction.Commit();
                    return true;
                }
                else
                {
                    Trasaction.Rollback();
                    return false;
                }


            }
            else { return false; }
        }

        public AppointmentBookmarkViewModal GetAppointmentBookmark(int Id)
        {
            var AppointmentBookmark = _context.AppointmentBookmark.Where(x => x.AppointmentId == Id).FirstOrDefault();
            if (AppointmentBookmark == null) { return null; }
            AppointmentBookmarkViewModal Record = new AppointmentBookmarkViewModal();
            Record.Reason = AppointmentBookmark.Reason;
            Record.AppointmentId = Id;
            Record.EndDate = AppointmentBookmark.EndDate;
            Record.JyotishId = AppointmentBookmark.JyotishId;
            Record.Id = AppointmentBookmark.Id;
            return Record;
        }
        public bool AddJyotishPooja(JyotishPoojaViewModel model)
        {
            var jdata = _context.JyotishRecords.Where(e => e.Id == model.JyotishId && e.Status).FirstOrDefault();
            if (jdata == null)
            {
                return false;
            }
            else
            {
                if (!(bool)jdata.Pooja)
                {
                    jdata.Pooja = true;
                }
            }
            var totalUsedServiceCount = _context.JyotishPooja.Count(e => e.JyotishId == model.JyotishId && e.status);
            var serviceCount = (from package in _context.PackageManager
                                join managepk in _context.ManageSubscriptionModels on package.SubscriptionId equals managepk.SubscriptionId
                                join feature in _context.SubscriptionFeatures on managepk.FeatureId equals feature.FeatureId
                                where package.JyotishId == model.JyotishId && package.Status && feature.ServiceUrl.ToLower() == "/jyotish/pooja"
                                select managepk.ServiceCount).FirstOrDefault();
            if (totalUsedServiceCount >= serviceCount)
            {
                return false;
            }

            var res = _context.JyotishPooja.Where(e => e.poojaType == model.poojaType && e.JyotishId == model.JyotishId && e.status).FirstOrDefault();

            if (res != null)
            {
                return false;
            }

            JyotishPoojaModel pooja = new JyotishPoojaModel
            {
                poojaType = model.poojaType,
                JyotishId = model.JyotishId,
                amount = model.amount,
                date = DateTime.Now,
                status = true
            };
            _context.JyotishPooja.Add(pooja);
            return _context.SaveChanges() > 0;
        }
        public bool UpdateJyotishPooja(JyotishPoojaViewModel model)
        {
            var res = _context.JyotishPooja.Where(e => e.Id == model.Id && e.status).FirstOrDefault();

            if (res != null)
            {
                res.poojaType = model.poojaType;
                res.amount = model.amount;
                _context.JyotishPooja.Update(res);
            }
            return _context.SaveChanges() > 0;
        }

        public dynamic getJyotishPoojaList(int Id)
        {
            var res = (from pooja in _context.JyotishPooja
                       join list in _context.PoojaList on pooja.poojaType equals list.Id
                       where pooja.status && list.Status && pooja.JyotishId == Id
                       orderby pooja.Id descending
                       select new
                       {
                           Id = pooja.Id,
                           poojaTypeId = pooja.poojaType,
                           poojaType = list.Name,
                           amount = pooja.amount
                       }).ToList();
            return res;
        }

        public bool removeJyotishPooja(int PoojaId)
        {
            var res = _context.JyotishPooja.Where(e => e.Id == PoojaId && e.status).FirstOrDefault();
            if (res != null)
            {
                res.status = false;
                _context.JyotishPooja.Update(res);
            }
            return _context.SaveChanges() > 0;
        }

        public string changeJyotishActiveStatus(int jyotishId, bool status)
        {
            var res = _context.JyotishRecords.Where(e => e.Id == jyotishId && e.Status).FirstOrDefault();
            if (res != null)
            {
                if (res.ActiveStatus && status)
                {
                    return "successfull";
                }
                _context.Entry(res).Reload();
                res.ActiveStatus = status;
                _context.JyotishRecords.Update(res);
                if (_context.SaveChanges() > 0)
                {
                    return "successfull";
                }
                else
                {
                    return "some error";

                }
            }
            return "not found";
        }


        public bool changeJyotishServiceStatus(int jyotishId, bool status)
        {
            var res = _context.JyotishRecords.Where(e => e.Id == jyotishId && e.Status).FirstOrDefault();
            if (res != null)
            {
                res.ServiceStatus = status;
                _context.JyotishRecords.Update(res);
                return _context.SaveChanges() > 0;
            }
            return false;
        }

        public bool getJyotishserviceStatus(int jyotishId)
        {
            var res = _context.JyotishRecords.Where(e => e.Id == jyotishId && e.Status).Select(e => e.ServiceStatus).FirstOrDefault();
            if (res != null)
            {
                return (bool)res;
            }
            return false;
        }

        public dynamic getJyotishDashboardRecord(int jyotishId)
        {
            var totalAppointment = _context.AppointmentRecords.Count(e => e.JyotishId == jyotishId);
            var pendingAppointment = _context.AppointmentRecords.Count(e => e.ArrivedStatus == 0 && e.JyotishId == jyotishId);
            var todayAppointment = _context.AppointmentSlots.Count(e => e.Date.Date == DateTime.Now.Date && e.Status.ToLower() == "booked" && e.JyotishId == jyotishId);
            var totalPooja = _context.BookedPoojaList.Count(e => e.jyotishId == jyotishId && e.status);
            var totalcall = _context.UserServiceRecord.Where(e => e.JyotishId == jyotishId && e.Action == 2).Sum(e => e.Count);
            var totalChat = _context.UserServiceRecord.Where(e => e.JyotishId == jyotishId && e.Action == 1).Sum(e => e.Count);
            var totalVideo = _context.JyotishVideos.Count(e => e.JyotishId == jyotishId);
            var totalGallery = _context.JyotishGallery.Count(e => e.JyotishId == jyotishId);


            var currentYear = DateTime.Now.Year;
            var totalAmountForCurrentYearByMonth = _context.UserServiceRecord
                .Where(e => e.date.Year == currentYear && e.Action == 2 && e.JyotishId == jyotishId)
                .GroupBy(e => e.date.Month)  // Group by month
                .Select(g => new
                {
                    Month = g.Key,
                    total = g.Sum(e => e.Count)  // Sum of Amount for each month
                })
                .ToList();
            var totalAmountForsYearByMonthForChat = _context.UserServiceRecord
    .Where(e => e.date.Year == currentYear && e.Action == 1 && e.JyotishId == jyotishId)
    .GroupBy(e => e.date.Month)  // Group by month
    .Select(g => new
    {
        Month = g.Key,
        total = g.Sum(e => e.Count)  // Sum of Amount for each month
    })
    .ToList();

            return new
            {
                totalAppointment = totalAppointment,
                pendingAppointment = pendingAppointment,
                todayAppointment = todayAppointment,
                totalPooja = totalPooja,
                totalCall = totalcall,
                callStatics = totalAmountForCurrentYearByMonth,
                chatStatics = totalAmountForsYearByMonthForChat,
                totalChat = totalChat,
                totalVideo = totalVideo,
                totalGallery = totalGallery
            };
        }

        public dynamic GetTopTenWalletHistory(int JyotishId)
        {
            var Jyotish = (from wallet in _context.WalletHistroy
                           join user in _context.Users on wallet.UId equals user.Id into userGroup
                           from user in userGroup.DefaultIfEmpty()
                           where wallet.JId == JyotishId
                           orderby wallet.Id descending
                           select new
                           {
                               UserName = wallet.UId != null ? user.Name : null,
                               paymentDate = wallet.date,
                               amount = wallet.amount,
                               paymentId = wallet.PaymentId,
                               paymentBy = wallet.PaymentBy,
                               paymentAction = wallet.PaymentAction,
                               profile = wallet.UId != null ? user.ProfilePictureUrl : null,
                               paymentFor = wallet.PaymentFor,
                               paymentStatus = wallet.PaymentStatus,
                               Id = wallet.Id
                           }

                          ).Take(10);

            return Jyotish;
        }

        public dynamic getUserServiceRevordForJyotish(int jyotishId)
        {
            var res = _context.UserServiceRecord.Where(e => e.JyotishId == jyotishId && e.Status && e.Action!=0).OrderByDescending(e=>e.date).ToList();

            var totalAmountForCurrentYearByMonth = _context.UserServiceRecord
                .Where(e => e.Action == 2 && e.JyotishId == jyotishId && e.Status).Sum(e => e.Count);
               
            var totalAmountForsYearByMonthForChat = _context.UserServiceRecord
    .Where(e => e.Action == 1 && e.JyotishId == jyotishId && e.Status).Sum(e => e.Count);
            return new
            {
                totalCall = totalAmountForCurrentYearByMonth,
                totalChat = totalAmountForsYearByMonthForChat,
                serviceRecord = res
            };
        }

        public bool purchaseAdvertisement(PurchaseAdvertisementService ps)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var preAdv = _context.PurchaseAdvertisement.Where(e => e.adId == ps.adId && e.jyotishId == ps.jyotishId && e.status).FirstOrDefault();
                    if (preAdv != null)
                    {
                        return false;
                    }

                 string _uploadDirectory = Directory.GetCurrentDirectory();
                  
                    var fileName = Guid.NewGuid().ToString() + Path.GetFileName(ps.Banner.FileName);
                    string filePath = "wwwroot/Images/Advertisement";
                    string sqlPath ="Images/Advertisement/"+fileName;
                    string finalPath = Path.Combine(_uploadDirectory, filePath);
                    ps.BannerUrl = sqlPath;

                    if (!Directory.Exists(finalPath))
                    {
                        Directory.CreateDirectory(finalPath);
                    }

                    // Checking if the JyotishWallet exists with the correct status
                    var res = _context.JyotishWallets
                                    .Where(e => e.jyotishId == ps.jyotishId && e.status == 1)
                                    .FirstOrDefault();

                    if (res == null)
                    {
                        return false;
                    }

                    // Getting the package price
                    var packagePrice = _context.AdvertisementPackage
                                               .Where(e => e.Id == ps.adId && e.Status)
                                               .FirstOrDefault();

                    if (packagePrice == null)
                    {
                        return false;
                    }
                    float packageFinalPrice = (float)packagePrice.FinalPrice;
                    if (ps.AreaId?.Split(',').Length >packagePrice.MaxCountry && ps.AdvertisementArea.ToLower()=="country")
                    {
                        var count = 1;
                        foreach(var item in ps.AreaId.Split(','))
                        {
                            if (count > packagePrice.MaxCountry)
                            {
                                packageFinalPrice += packagePrice.FinalPrice;
                            }
                            count++;
                        }

                    }else if (ps.AreaId?.Split(',').Length > packagePrice.MaxState && ps.AdvertisementArea.ToLower() == "state")
                    {
                        var count = 1;
                        foreach (var item in ps.AreaId.Split(','))
                        {
                            if (count > packagePrice.MaxState)
                            {
                                packageFinalPrice += packagePrice.FinalPrice;
                            }
                            count++;
                        }

                    }
                    else if (ps.AreaId?.Split(',').Length > packagePrice.MaxCity && ps.AdvertisementArea.ToLower() == "city")
                    {
                        var count = 1;
                        foreach (var item in ps.AreaId.Split(','))
                        {
                            if (count > packagePrice.MaxCity)
                            {
                                packageFinalPrice += packagePrice.FinalPrice;
                            }
                            count++;

                        }
                    }
                        // Check if the wallet has sufficient balance
                    if (res.WalletAmount < packageFinalPrice)
                    {
                        return false;
                    }

                    // Deduct the package price from the wallet
                    res.WalletAmount -= (long)Math.Abs(Math.Ceiling(packageFinalPrice));

                    // Update wallet in the database
                    _context.JyotishWallets.Update(res);

                    // Save changes to the wallet
                    if (_context.SaveChanges() > 0)
                    {
                       
                       
                        // Creating a new PurchaseAdvertisement record
                        PurchaseAdvertisement pa = new PurchaseAdvertisement
                        {
                            AdvertisementArea = ps.AdvertisementArea,
                            adId = ps.adId,
                            jyotishId = ps.jyotishId,
                            StartDate = ps.StartDate,
                            BannerUrl = ps.BannerUrl,
                            CreatedDate = DateTime.Now,
                            AreaId = ps.AreaId,
                            activeStatus = false,
                            appStatus = false,
                            status = true
                        };

                        // Adding the advertisement package purchase
                        _context.PurchaseAdvertisement.Add(pa);

                        // Save changes for the advertisement purchase
                        if (_context.SaveChanges() > 0)
                        {
                            // Adding wallet history
                            WalletHistoryViewmodel wh = new WalletHistoryViewmodel
                            {
                                JId = ps.jyotishId,
                                amount = (long)Math.Abs(Math.Ceiling(packageFinalPrice)),
                                PaymentFor = "Purchase Advertisement Package",
                                UId = null,
                                status = 1,
                                PaymentAction = "Debit",
                                PaymentStatus = "success"
                            };

                            var walletresponse = AddWalletHistory(wh);
                            if (walletresponse == "Successful")
                            {
                                using (var stream = new FileStream(Path.Combine(_uploadDirectory, filePath+"/"+fileName), FileMode.Create))
                                {
                                    ps.Banner.CopyToAsync(stream);
                                }
                                // Commit transaction if everything is successful
                                transaction.Commit();
                                return true;
                            }
                            else
                            {
                                // If wallet history fails, rollback transaction
                                transaction.Rollback();
                                return false;
                            }
                        }
                    }
                    // If saving wallet changes failed, rollback transaction
                    transaction.Rollback();
                    return false;
                }
                catch (Exception ex)
                {
                    // In case of any exception, rollback the transaction
                    transaction.Rollback();
                    return false;
                }
            }

        }

        public dynamic getPurchasedAdvertisement(int jyotishId)
        {
            var res = _context.Database
                .SqlQueryRaw<DataService.Advertisementservice>(
                    "EXEC dbo.sp_AdvertisementManager @jyotishId = {0}, @action = {1}",
                    jyotishId, 1
                )
                .ToList();
            return res;
        }

       

    }
}
