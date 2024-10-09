﻿using BusinessAccessLayer.Abstraction;
using DataAccessLayer.DbServices;
using ModelAccessLayer.Models;
using ModelAccessLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var records = _context.JyotishRecords.Where(a => a.Status == "Complete").Where(x => x.City.Contains(City)).ToList();
            if (records.Count == 0)
            {
                records = _context.JyotishRecords.Where(x => x.Role == "Jyotish").Where(x => x.Country.Contains("India")).ToList();
            }
            return records;
        }
        public List<JyotishModel> AllAstrologer()
        {
            List<JyotishModel> record = _context.JyotishRecords.Where(x => x.Status == "Complete").ToList();
            return record;
        }

        /*public JyotishProfileViewModel AstrologerProfile(int Id)
        {
            var Jyotish = _context.JyotishRecords.Where(x => x.Status == "Complete").Where(x => x.Id == Id).FirstOrDefault();
            var videos = _context.JyotishVideos.Where(x=>x.JyotishId == Id).ToArray();
            var gallery = _context.JyotishGallery.Where(x => x.JyotishId == Id).ToArray();

            return record;
        }*/


        public JyotishProfileViewModel AstrologerProfile(int Id)
        {
            // Fetch the Jyotish record based on the provided Id and status
            var jyotishRecord = _context.JyotishRecords
                                         .Where(x => x.Status == "Complete" && x.Id == Id)
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
            var result = _context.JyotishRecords.Where(x => x.Status == "Complete")
           .Where(record => record.Name.Contains(keyword) ||
                            record.Expertise.Contains(keyword) ||
                            record.Language.Contains(keyword) ||
                            record.Country.Contains(keyword) ||
                            record.State.Contains(keyword) ||
                            record.City.Contains(keyword) || record.Specialization.Contains(keyword) || record.Pooja.Contains(keyword))

           .ToList();

            return result;
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

    }
}
