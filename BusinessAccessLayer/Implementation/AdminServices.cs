using BusinessAccessLayer.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.DbServices;
using ModelAccessLayer.Models;

using ModelAccessLayer.ViewModels;
using System.Net.Mail;
using System.Net;
using System.Reflection.Metadata;
using DataAccessLayer.Migrations;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;


namespace BusinessAccessLayer.Implementation
{
    public class AdminServices:IAdminServices
    {
        private readonly ApplicationContext _context;
        private readonly string _uploadDirectory;
        public AdminServices(ApplicationContext context)
        {
            _context = context;
            _uploadDirectory = Directory.GetCurrentDirectory();

            // Ensure directory exists
            if (!Directory.Exists(_uploadDirectory))
            {
                Directory.CreateDirectory(_uploadDirectory);
            }
        }

        public List<JyotishModel> GetAllJyotish()
        {
            var Records = _context.JyotishRecords
                             .Where(record => record.Role == "jyotish")
                             .ToList();
            return Records;
        }

        public List<JyotishModel> GetAllPendingJyotish()
        {
            var Records = _context.JyotishRecords
                             .Where(record => record.Role == "pending" && record.Status == true)
                             .ToList();
            return Records;
        }
        public List<ModelAccessLayer.Models.UserModel> GetAllUser()
        {
            var Records = _context.Users.ToList();
            return Records;
        }
        public List<TeamMemberModel> GetAllTeamMember()
        {
            var Records = _context.TeamMemberRecords.ToList();
            return Records;
        }
        public List<SpecializationListModel> GetSpecializationList()
        {
            var Records = _context.SpecializationList.ToList();
            return Records;
        }
        public List<AppointmentListAdminViewModel> GetAllAppointment()
        {
            // var Records = _context.AppointmentRecords.ToList();
            var Records = _context.AppointmentRecords
            .Join(_context.Users,
           record => record.UserId,
           user => user.Id,
           (record, user) => new { record, user })
     .Join(_context.AppointmentSlots,
           combined => combined.record.SlotId,

           slot => slot.Id,
           (combined, slot) => new AppointmentListAdminViewModel
           {
               Id = combined.record.Id,
               UserName = combined.user.Name,
               UserMobile = combined.user.Mobile,
               UserEmail = combined.user.Email,
               UserId = combined.record.UserId,
               JyotishId = (int)_context.AppointmentSlots.Where(x => x.Id == slot.Id).Select(y => y.JyotishId).FirstOrDefault(),
               JyotishName = _context.JyotishRecords.Where(x=>x.Id == _context.AppointmentSlots.Where(x => x.Id == slot.Id).Select(y => y.JyotishId).FirstOrDefault()).Select(z=>z.Name).FirstOrDefault(),
               JyotishEmail = _context.JyotishRecords.Where(x => x.Id == _context.AppointmentSlots.Where(x => x.Id == slot.Id).Select(y => y.JyotishId).FirstOrDefault()).Select(z => z.Email).FirstOrDefault(),
               Problem = combined.record.Problem,
               Date = slot.Date,
               Time = slot.TimeFrom,
               Status = combined.record.Status,
               Amount = combined.record.Amount
           })
     .ToList();
     return Records;
        }
        static string PasswordMethod(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            char[] buffer = new char[length];
            for (int i = 0; i < length; i++)
            {
                buffer[i] = chars[random.Next(chars.Length)];
            }
            return new string(buffer);
        }
        public bool ApproveJyotish(IdViewModel JyotishId )
        {
            var Jyotish = _context.JyotishRecords.Where(x => x.Id == JyotishId.Id).FirstOrDefault();
            if (Jyotish == null)
            { return false; }
       
            Jyotish.ApprovedStatus = "Approved";
            Jyotish.Role = "Jyotish";
            _context.JyotishRecords.Update(Jyotish);
            var result = _context.SaveChanges();
            if (result > 0)
            {
                string message = "Hey "+ Jyotish.Name+ ",You are selected as Jyotish in MyJyotish G. Your Account has been Activated Successfully.";
                string subject = " My Jyotish G";
                SendEmail(message, Jyotish.Email, subject);
                return true;
            }
            else { return false; }
            
        }
        
        public bool RejectJyotish(JyotishRejectMailViewModel model)
        {
            var Jyotish = _context.JyotishRecords.Where(x => x.Id == model.JyotishId).FirstOrDefault();
            if (Jyotish == null)
            { return false; }
            Jyotish.Role = "Rejected";
            Jyotish.Status =false;
            Jyotish.Message = model.Message;
            _context.JyotishRecords.Update(Jyotish);
            var result = _context.SaveChanges();
            if (result > 0) 
            {
                string message = model.Message;
                string subject =model.Subject;
                SendEmail(message, Jyotish.Email, subject);
                return true;
            }
            else
            { return false; }
        }

        public void SendEmail(string MessageBody, string Mail,string Subjectbody)
        {
            try
            {
                // Sender's email address and credentials
                string senderEmail = "varishveer123@gmail.com";
                string senderPassword = "vjcx xhzp oumw xhdh";

                // Recipient's email address
                string recipientEmail = Mail;

                // SMTP server address and port number
                string smtpServer = "smtp.gmail.com";
                int smtpPort = 587; // Use 587 for TLS or 465 for SSL

                // Create a new SMTP client
                using (SmtpClient client = new SmtpClient(smtpServer, smtpPort))
                {
                    client.EnableSsl = true; // Enable SSL/TLS
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(senderEmail, senderPassword);

                    // Create a new email message
                    MailMessage message = new MailMessage(senderEmail, recipientEmail)
                    {
                        Subject = Subjectbody,
                        Body = MessageBody,
                    };

                    // Send the email
                    client.Send(message);

                    Console.WriteLine("Email sent successfully.");
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email. Error message: {ex.Message}");
            }
        }

        public bool RemoveJyotish(IdViewModel JyotishId)
        {
            var Jyotish = _context.JyotishRecords.Where(x => x.Id == JyotishId.Id).FirstOrDefault();
            if (Jyotish == null || Jyotish.ApprovedStatus != "Rejected")
            { return false; }
            _context.JyotishRecords.Remove(Jyotish);
            var result = _context.SaveChanges();
            if (result > 0) { return true; }
            else
            { return false;}

        }

       
        public bool AddExpertise(ExpertiseModel _expertise)
        {
            var isExpertiseValid = _context.ExpertiseRecords.Where(x => x.Name == _expertise.Name).FirstOrDefault();
            if (isExpertiseValid != null) { return false; }
            _expertise.DateAdded = DateTime.Now;
            _context.ExpertiseRecords.Add(_expertise);
            var result = _context.SaveChanges();
            if (result > 0)
            { return true; }
            else
            { return false; }

        }
      /*  public bool AddPoojaCategory(PoojaCategoryViewModel _pooja)
        {
            var isPoojaValid = _context.PoojaCategory.Where(x => x.Name == _pooja.Name).FirstOrDefault();
            if (isPoojaValid != null) { return false; }
            PoojaCategoryModel model = new PoojaCategoryModel();
            model.Name = _pooja.Name;
            model.DateAdded = DateTime.Now;
            _context.PoojaCategory.Add(model);
            var result = _context.SaveChanges();
            if (result > 0)
            { return true; }
            else
            { return false; }

        }*/
      /*  public List<PoojaCategoryModel> GetAllPoojaCategory()
        {
            var records = _context.PoojaCategory.ToList();
            return records;
        }*/

      /*  public bool AddNewPoojaList(PoojaListViewModel model)
        {
            if(model == null)
            { return false; }

            var IsPoojaValid = _context.PoojaCategory.Where(x=>x.Name == model.Name).FirstOrDefault();
            if(IsPoojaValid != null)
            { return false; }

            PoojaRecordModel record = new PoojaRecordModel()
            {
                Name = model.Name,
                PoojaCategoryId = model.PoojaCategoryId,
            };
            _context.PoojaRecord.Add(record);
            var result = _context.SaveChanges();
            if(result>0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }*/
        public List<PoojaRecordModel> PoojaRecord()
        {
            var Records = _context.PoojaRecord.ToList();
            return Records;
        }
        public List<ExpertiseModel> GetAllExpertise()
        {
            var records = _context.ExpertiseRecords.ToList();
            return records;
        }

        public string AddAppointmentSlot(AppointmentSlotViewModel model)
        {
            if (DateTime.Compare(model.Date, DateTime.Now) < 0)
            { return "Invalid Date"; }

            var existingSlots = _context.AppointmentSlots
         .Where(x => x.JyotishId == model.JyotishId &&
                      x.Date >= model.Date)
         .Select(e => e.Date)
         .ToList();

            if (DateTime.Compare(model.Date, model.DateTo) <= 90)
            {
                List<SlotModel> slotsToAdd = new List<SlotModel>();
                for (DateTime date = model.Date; date <= model.DateTo; date = date.AddDays(1))
                {
                    if (existingSlots.Contains(date.Date))
                    {
                        continue;
                    }
                        TimeOnly timeFrom = TimeOnly.FromDateTime(Convert.ToDateTime(model.TimeFrom));
                        TimeOnly timeTo = TimeOnly.FromDateTime(Convert.ToDateTime(model.TimeTo));

                    TimeOnly startTime =timeFrom;
                    TimeOnly endTime =timeTo;


                    bool isSaturdaySkipped = model.saturday == 1 && date.DayOfWeek == DayOfWeek.Saturday;
                    bool isSundaySkipped = model.sunday == 2 && date.DayOfWeek == DayOfWeek.Sunday;
                    bool isSkipDateMatched = model.skipDate != null && model.skipDate.ToString() != "1001-01-01" &&
                    DateTime.Compare((DateTime)model.skipDate, date) == 0;

                    for (TimeOnly time = startTime; time <= endTime; time = time.AddMinutes(model.TimeDuration))
                    {
                        DateOnly dateonly = DateOnly.FromDateTime(date);

                        var data = new SlotModel
                        {
                            Date = dateonly,
                            Time = time,
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
                    _context.Slots.AddRange(slotsToAdd);
                }
            }
            if (_context.SaveChanges() > 0) { return "Successful"; }
            else { return "Data Not Saved"; }
        }

        public string RemoveSlotWithskipDates(AppointmentSlotViewModel model)
        {
           

            var slots = _context.Slots
                .Where(x => x.ActiveStatus == 1)
                .ToList();

            List<SlotModel> slotsToUpdate = new List<SlotModel>();

            DateTime? skipDate = model.skipDate;
            bool isSkipDateValid = skipDate.HasValue && skipDate.Value != new DateTime(1001, 1, 1);

            DateOnly slotDate = DateOnly.FromDateTime(skipDate.Value.Date);
            foreach (var slot in slots)
            {
                bool shouldDeactivate = false;

                if ((model.saturday == 1 && slot.Date.DayOfWeek == DayOfWeek.Saturday) ||
                    (model.sunday == 2 && slot.Date.DayOfWeek == DayOfWeek.Sunday) ||
                    (isSkipDateValid && slot.Date == slotDate))
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
                _context.Slots.UpdateRange(slotsToUpdate);
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
        public AdminModel Profile(string email)
        {
            var record = _context.AdminRecords.Where(x => x.Email == email).FirstOrDefault();
            if (record == null)
            {
                return null;
              
            }
            else
            {
                return record;
            }
        }

      /*  public AdminDashboardViewModal Dashboard()
        {
            int Users = _context.Users.Count();
            int Jyotish = _context.JyotishRecords.Count();
            int Pending = _context.PendingJyotishRecords.Count();
            int Reject = _context.PendingJyotishRecords.Where(x => x.Status == "Rejected").Count();
            AdminDashboardViewModal model = new AdminDashboardViewModal() 
            {
                TotalUsers = Users,
                TotalJyotish = Jyotish,
                RejectedJyotish = Reject,
                PendingJyotish = Pending
            };

            if (model == null)
            {
                return null;
            }
            else
            {
                return model;
            }
        }*/

      
        public List<ChattingModel> ChattingRecord()
        {
            var Records = _context.ChatingRecords.ToList();
            return Records;
        }
        public List<CallingModel> CallingRecord()
        {
            var Records = _context.CallingRecords.ToList();
            return Records;
        }
        public AppointmentDetailViewModel AppointmentDetails(int id)
        {
            AppointmentDetailViewModel Record = _context.AppointmentRecords.Where(x => x.Id == id).Select(x => new AppointmentDetailViewModel
            {
                    Id = x.Id,
                    UserName = _context.Users.Where(u => u.Id == x.UserId).Select(u => u.Name).FirstOrDefault(),
                    UserMobile = _context.Users.Where(u => u.Id == x.UserId).Select(u => u.Mobile).FirstOrDefault(),
                    UserEmail = _context.Users.Where(u => u.Id == x.UserId).Select(u => u.Email).FirstOrDefault(),
                    UserId = x.UserId,
                    Problem = x.Problem,
                    Date = _context.AppointmentSlots.Where(u => u.Id == x.SlotId).Select(u => u.Date).FirstOrDefault(),
                    Time = _context.AppointmentSlots.Where(u => u.Id == x.SlotId).Select(u => u.TimeFrom).FirstOrDefault(),
                    Status = x.Status,
                    Amount = x.Amount,

            }).FirstOrDefault();

            if (Record == null)
            { return null; }

            else { return Record; }
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

        public bool AddCountry(ModelAccessLayer.Models.Country _country)
        {
            var isCountryValid = _context.Countries.Where(x => x.Name == _country.Name);
            if (isCountryValid != null)
            { return false; }
            else
            {
                _context.Countries.Add(_country);
                var reuslt = _context.SaveChanges();
                if (reuslt != 0)
                { return true; }
                else
                { return false; }
            }
        }
        public bool AddState(ModelAccessLayer.Models.State _state)
        {
            var isStateValid = _context.States.Where(x => x.Name == _state.Name);
            var isCountryValid = _context.Countries.Where(x => x.Id == _state.CountryId);
            if (isStateValid != null && isCountryValid == null)
            { return false; }
            else
            {
                _context.States.Add(_state);
                var reuslt = _context.SaveChanges();
                if (reuslt != 0)
                { return true; }
                else
                { return false; }
            }
        }

        public bool AddCity(City _city) 
        {

            var isCityValid = _context.Cities.Where(x => x.Name == _city.Name);
            var isStateValid = _context.States.Where(x => x.Id == _city.StateId);
            if (isCityValid != null && isStateValid == null)
            { return false; }
            else
            {
                _context.Cities.Add(_city);
                var reuslt = _context.SaveChanges();
                if (reuslt != 0)
                { return true; }
                else
                { return false; }
            }
        }

        public bool AddSlider(SliderImagesViewModel model)
        {
            SliderImagesModel slider = new SliderImagesModel();
            if (model.HomePage != null)
            {
                var HomePageGuid = Guid.NewGuid().ToString();
                var SqlPath = "/Images/Slider/" + HomePageGuid + model.HomePage.FileName;
                var HomePagePath = Path.Combine(_uploadDirectory, "wwwroot"+SqlPath);
                using (var stream = new FileStream(HomePagePath, FileMode.Create))
                {
                     model.HomePage.CopyTo(stream);
                }
                slider.HomePage = SqlPath;
            }

            if (model.BookPoojaCategory != null)
            {
                var BookPoojaCategoryGuid = Guid.NewGuid().ToString();
                var SqlPath = "/Images/Slider/" + BookPoojaCategoryGuid + model.BookPoojaCategory.FileName;
                var BookPoojaCategoryPath = Path.Combine(_uploadDirectory, "wwwroot"+SqlPath);
                using (var stream = new FileStream(BookPoojaCategoryPath, FileMode.Create))
                {
                    model.BookPoojaCategory.CopyTo(stream);
                }
                slider.BookPoojaCategory =   SqlPath;
            }

            if (model.PoojaList != null)
            {
                var PoojaListGuid = Guid.NewGuid().ToString();
                var SqlPath = "/Images/Slider/" + PoojaListGuid + model.PoojaList.FileName;
                var PoojaListPath = Path.Combine(_uploadDirectory, "wwwroot"+SqlPath);
                using (var stream = new FileStream(PoojaListPath, FileMode.Create))
                {
                    model.PoojaList.CopyTo(stream);
                }
                slider.PoojaList = SqlPath;
            }

            _context.Sliders.Add(slider);
            var result = _context.SaveChanges();
            if(result > 0)
            { return true; }
            else { return false; }
            
        }

        public List<SliderImagesModel> SliderImageList()
        {
            var Records = _context.Sliders.ToList();
            if(Records.Count == 0)
            { return null; }
            return Records;
        }

      /*  public bool AddPoojaDetail(PoojaRecordViewModel model)
        {
            if (model == null)
            {
                return false;
            }
          
           
            var IsPoojaListValid = _context.PoojaRecord.Where(x => x.Id == model.PoojaCategoryId).FirstOrDefault();
            if (IsPoojaListValid == null)
            { return false; }
            else
            {
                var PoojaCategory = _context.PoojaCategory.Where(x => x.Id == IsPoojaListValid.PoojaCategoryId).FirstOrDefault();
                PoojaRecordModel record = new PoojaRecordModel()
                {
                    Category = PoojaCategory.Name,
                   
                    PoojaCategoryId = model.PoojaCategoryId,
                    DateTime = model.DateTime,
                    Name = model.Name,
                    Headline = model.Headline,
                    Description = model.Description,
                    Benefits = model.Benefits,
                    Procedure = model.Procedure,
                    AboutGod = model.AboutGod,
                };
                _context.PoojaRecord.Add(record);
                var result = _context.SaveChanges();
                if (result > 0)
                { return true; }
                else { return false; }
            }
        }*/

        public List<DocumentModel> GetAllJyotishDocument()
        {
                var Records = _context.Documents.ToList();
                return Records;   
        }

        public List<CallingModel> GetJyotishCalls(int id)
        {
            var isJyotishValid = _context.JyotishRecords.Where(x => x.Id == id).FirstOrDefault();
            if(isJyotishValid == null)
            {
                return null;
            }
            
            var CallList = _context.CallingRecords.Where(x => x.JyotishId == id).ToList();
            foreach (var Call in CallList)
            {
                Call.Jyotish = null;
            }
            return CallList;
        }
        public List<ChattingModel> GetJyotishChats(int id)
        {
            var isJyotishValid = _context.JyotishRecords.Where(x => x.Id == id).FirstOrDefault();
            if (isJyotishValid == null)
            {
                return null;
            }
            var ChatList = _context.ChatingRecords.Where(x => x.JyotishId == id).ToList();

            foreach (var Chat in ChatList)
            {
                Chat.Jyotish = null;
            }
            return ChatList;
        }

        public DocumentModel GetJyotishDocs(int id)
        {
            var isJyotishValid = _context.JyotishRecords.Where(x => x.Id == id).FirstOrDefault();
            if (isJyotishValid == null)
            {
                return null;
            }
            var Docs = _context.Documents.Where(x => x.JId == id).FirstOrDefault();
            if(Docs == null) { return null; }
            Docs.Jyotish = null;
           
            return Docs;
        }


        public JyotishModel GetJyotishDetails(int id)
        {
            var jyotish = _context.JyotishRecords.Where(x => x.Id == id).FirstOrDefault();
            if(jyotish == null)
            {
                return null;
            }
            return jyotish;
        }

        public string UpdateJyotishDetails(JyotishDetailsViewModel model)
        {
            var Record = _context.JyotishRecords.Where(x => x.Id == model.Id).FirstOrDefault();
            if (Record == null)
            {
                return "Jyotish not found";
            }
            Record.Email = model.Email;
            Record.Mobile = model.Mobile;
            Record.Role = model.Role;
            Record.Name = model.Name;
            Record.Gender = model.Gender;
            Record.Language = model.Language;
            Record.Expertise = model.Expertise;
            Record.Country = model.Country;
            Record.State = model.State;
            Record.City = model.City;
            Record.Password = model.Password;
            Record.DateOfBirth = model.DateOfBirth;
            Record.ProfileImageUrl = model.ProfileImageUrl;
            Record.ApprovedStatus = model.Status;
            Record.Otp = model.Otp;
            Record.Experience = model.Experience;
         
            Record.Call = model.Call;
            Record.CallCharges = model.CallCharges;
            Record.Chat = model.Chat;
            Record.ChatCharges = model.ChatCharges;
            Record.Address = model.Address;
            Record.TimeFrom = model.TimeFrom;
            Record.TimeTo = model.TimeTo;
            _context.JyotishRecords.Update(Record);
            var result = _context.SaveChanges();
            if(result >0) { return "Successful"; }
            else { return "Data Not Update"; }
        }

        public string ApproveJyotishDocs(JyotishDocApproveViewModel model)
        {
            var jyotish = _context.JyotishRecords.FirstOrDefault(x => x.Id == model.Id);
            if (jyotish == null) return "Jyotish not found";

            var record = _context.Documents.FirstOrDefault(x => x.JId == jyotish.Id);
            if (record == null) return "Documents not found";

            var statusMapping = new Dictionary<string, Action>
            {
                { "idProofStatus", () => record.IdProofStatus = "Verified" },
                { "addressProofStatus", () => record.AddressProofStatus = "Verified" },
                { "tenthCertificateStatus", () => record.TenthCertificateStatus = "Verified" },
                { "twelveCertificateStatus", () => record.TwelveCertificateStatus = "Verified" },
                { "professionalCertificateStatus", () => record.ProfessionalCertificateStatus = "Verified" }
            };

            if (statusMapping.ContainsKey(model.ImageStatus))
            {
                statusMapping[model.ImageStatus]();
            }
            else
            {
                return "Invalid data";
            }

            _context.Documents.Update(record);
            _context.SaveChanges();

           

            return "Successful";
        }
        /* public string RejectJyotishDocs(EmailDocumentViewModel model)
         {
             var jyotish = _context.JyotishRecords.FirstOrDefault(x => x.Id == model.Id);
             if (jyotish == null) return "Jyotish not found";

             var record = _context.Documents.FirstOrDefault(x => x.JId == jyotish.Id);
             if (record == null) return "Documents not found";

             var statusMapping = new Dictionary<string, Action>
                 {
                     { "idProofStatus", () => record.IdProofStatus = "Rejected" },
                     { "addressProofStatus", () => record.AddressProofStatus = "Rejected" },
                     { "tenthCertificateStatus", () => record.TenthCertificateStatus = "Rejected" },
                     { "twelveCertificateStatus", () => record.TwelveCertificateStatus = "Rejected" },
                     { "professionalCertificateStatus", () => record.ProfessionalCertificateStatus = "Rejected" }
                 };

             if (statusMapping.ContainsKey(model.ImageStatus))
             {
                 statusMapping[model.ImageStatus]();
             }
             else
             {
                 return "Invalid data";
             }

             _context.Documents.Update(record);
             _context.SaveChanges();

             try
             {
                 AccountServices.SendEmail(model.Message, jyotish.Email, model.Subject);
             }
             catch (Exception ex)
             {
                 // Log the exception if needed
                 return "Email sending failed: " + ex.Message;
             }

             return "Successful";
         }*/

        public string RejectJyotishDocs(EmailDocumentViewModel model)
        {
            var jyotish = _context.JyotishRecords.FirstOrDefault(x => x.Id == model.Id);
            if (jyotish == null) return "Jyotish not found";

            var record = _context.Documents.FirstOrDefault(x => x.JId == jyotish.Id);
            if (record == null) return "Documents not found";

            var statusMapping = new Dictionary<string, Action>
    {
        { "idProofStatus", () => { record.IdProofStatus = "Rejected"; record.IdProofMessage = model.Message; } },
        { "addressProofStatus", () => { record.AddressProofStatus = "Rejected"; record.AddressProofMessage = model.Message; } },
        { "tenthCertificateStatus", () => { record.TenthCertificateStatus = "Rejected"; record.TenthCertificateMessage = model.Message; } },
        { "twelveCertificateStatus", () => { record.TwelveCertificateStatus = "Rejected"; record.TwelveCertificateMessage = model.Message; } },
        { "professionalCertificateStatus", () => { record.ProfessionalCertificateStatus = "Rejected"; record.ProfessionalCertificateMessage = model.Message; } }
    };

            if (statusMapping.ContainsKey(model.ImageStatus))
            {
                statusMapping[model.ImageStatus]();
            }
            else
            {
                return "Invalid data";
            }

            _context.Documents.Update(record);
            _context.SaveChanges();

            try
            {
                AccountServices.SendEmail(model.Message, jyotish.Email, model.Subject);
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return "Email sending failed: " + ex.Message;
            }

            return "Successful";
        }


        public List<SlotModel> SlotList()
        {
            DateTime today = DateTime.Today;
            DateOnly todayDate = DateOnly.FromDateTime(today);

            var slots = _context.Slots
                 .Where(slot => slot.Date >= todayDate && slot.ActiveStatus==1 && slot.Status.ToLower()=="vacant")
                .ToList();

            return slots;
        }


        public string AddSlot(SlotViewModel model)
        {
            if (model == null)
            {
                return "Invalid Data";
            }

         
            TimeOnly startTime = model.Time;
            TimeSpan duration = TimeSpan.FromMinutes(model.TimeDuration); 

          
            DateTime startDateTime = model.Date.ToDateTime(startTime);
            DateTime endDateTime = startDateTime.Add(duration); 

           
            var conflictingSlot = _context.Slots
                .Where(x => x.Date == model.Date) 
                .AsEnumerable()
                .Where(x =>
                   
                    (x.Date.ToDateTime(x.Time) >= startDateTime && x.Date.ToDateTime(x.Time) < endDateTime) || 
                    (x.Date.ToDateTime(x.Time).AddMinutes(x.TimeDuration) > startDateTime && x.Date.ToDateTime(x.Time).AddMinutes(x.TimeDuration) <= endDateTime) 
                )
                .FirstOrDefault();

            if (conflictingSlot != null)
            {
                
                return "Slot overlaps with an existing booking.";
            }

           
            SlotModel slot = new SlotModel
            {
                Time = model.Time,
                TimeDuration = model.TimeDuration,
                Date = model.Date,
                Status = "Vacant"
            };

            _context.Slots.Add(slot);
            var result = _context.SaveChanges();

            if (result > 0)
            {
                return "Successful";
            }
            else
            {
                return "Internal Server Error";
            }
        }




        public string AddFeature(SubscriptionFeatureViewModel model)
        {
            if(model ==null)
            { return "Invalid Data"; }
            SubscriptionFeaturesModel NewData = new SubscriptionFeaturesModel()
            {
                Name = model.Name,
                ServiceUrl = model.ServiceUrl,
                 Status = true
            };

            _context.SubscriptionFeatures.Add(NewData);
            if (_context.SaveChanges() >0)
            { return "Successful"; }
            else
            {
                return "Internal Server Error.";
            }

        }
        public List<SubscriptionFeatureViewModel> GetAllFeatures()
        {
            
           
                
            var record = _context.SubscriptionFeatures.Where(x=>x.Status == true).OrderByDescending(e=>e.FeatureId).Select(x=> new SubscriptionFeatureViewModel
            { 
                Id = x.FeatureId,
                Name = x.Name,
                ServiceUrl= x.ServiceUrl
            }).ToList();     
            return record;
        }

        public string DeleteFeature(int Id)
        {
            var record = _context.SubscriptionFeatures.Where(x => x.FeatureId == Id && x.Status == true).FirstOrDefault();
            if(record==null)
            { return "Invalid Data"; }

            record.Status = false;
            _context.SubscriptionFeatures.Update(record);
            if (_context.SaveChanges() > 0) { return "Successful"; }
            else { return "Internal Server Error."; }
        }

        public string UpdateFeature(SubscriptionFeatureViewModel model)
        {
            var record = _context.SubscriptionFeatures.Where(x => x.FeatureId == model.Id && x.Status == true).FirstOrDefault();
            if (record == null)
            { return "Invalid Data"; }


            record.Name = model.Name;
            record.ServiceUrl = model.ServiceUrl;

            _context.SubscriptionFeatures.Update(record);
            if (_context.SaveChanges() > 0) { return "Successful"; }
            else { return "Internal Server Error."; }
        }

        public string AddSubscription(SubscriptionViewModel model)
        {
         
            if (model == null)
            {
                return "Invalid Data";
            }

            var subscriptionData = new SubscriptionModel
            {
                Name = model.Name,
                OldPrice = model.OldPrice,
                NewPrice = model.NewPrice,
                Discount = model.Discount,
                Gst = model.Gst,
                DiscountAmount = model.DiscountAmount,
                PlanType = model.PlanType,
                description = model.description,
                Status = true
            };

            _context.Subscriptions.Add(subscriptionData);

            
            if (_context.SaveChanges() > 0)
            {
                return "Successful";
            }
            else
            {
                return "Internal Server Error.";
            }
        }


        public string UpdateSubscription(SubscriptionViewModel model)
        {
            if (model == null)
            {
                return "Invalid Data";
            }

            var record = _context.Subscriptions
                                 .FirstOrDefault(x => x.SubscriptionId == model.SubscriptionId);

            if (record == null)
            {
                return "Subscription Not Found";
            }

            // Update fields
            record.Name = model.Name;
            record.OldPrice = model.OldPrice;
            record.NewPrice = model.NewPrice;
            record.Discount = model.Discount;
            record.Gst = model.Gst;
            record.DiscountAmount = model.DiscountAmount;
            record.PlanType = model.PlanType;
            record.description = model.description; 
          

            _context.Subscriptions.Update(record);

 
            _context.SaveChanges();
                  
            return "Successful";
               
        }

        public List<SubscriptionViewModel> GetAllSubscription()
        {
            var records = _context.Subscriptions.Where(x=>x.Status == true)
                                  .Select(subscription => new SubscriptionViewModel
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
                                     
                                  })
                                  .ToList();
            return records;
        }

        public string DeleteSubsciption(int Id)
        {
            var Subscription = _context.Subscriptions.Where(x => x.SubscriptionId == Id).FirstOrDefault();
            if(Subscription == null) { return "Data Not Found"; }
            Subscription.Status = false;
            _context.Subscriptions.Update(Subscription);

          /*  var Features = _context.ManageSubscriptionModels.Where(x => x.SubscriptionId == Id).ToList();

            foreach (var feature in Features)
            {
                feature.Status = false; // Set status to false for each feature
            }*/

            
            if (_context.SaveChanges() > 0) { return "Successful"; }
            else { return "internal Server Error."; }
        }
        public SubscriptionGetViewModel GetSubscription(int Id)
        {
            var records = _context.Subscriptions.Where(x=>x.SubscriptionId == Id && x.Status ==  true)
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
                                 .FirstOrDefault();

            return records ?? null;
        }

        public bool AddSpecialization(string specName)
        {
            SpecializationListModel name = new SpecializationListModel();
            name.Name = specName;
            _context.Add(name);
            if(_context.SaveChanges() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
                }

        public string AddManageSubscriptionData(ManageSubscriptionViewModel model)
        {
            var Subscription = _context.Subscriptions.Where(x => x.SubscriptionId == model.SubscriptionId).FirstOrDefault();
            var OldRecord = _context.ManageSubscriptionModels.Where(x => x.SubscriptionId == model.SubscriptionId && x.Status == true).Select(x => x.FeatureId).ToList();
            if (OldRecord.Contains(model.FeatureId))
            {
                return "Record Already Present";

            }
            List<ManageSubscriptionModel> mngsub = new List<ManageSubscriptionModel>();
           
                mngsub.Add(new ManageSubscriptionModel
                {
                    SubscriptionId = model.SubscriptionId,
                    FeatureId = model.FeatureId,
                    ServiceCount = model.ServiceCount,
                    Status = true
                });
           
            _context.ManageSubscriptionModels.AddRange(mngsub);
            if (_context.SaveChanges() > 0)
            {
                return "Successful";
            }
            else
            {
                return "Already Exists";
            }

        }

        public string UpdateManageSubscriptionData(ManageSubscriptionViewModel model)
        {
            var Subscription = _context.Subscriptions.Where(x => x.SubscriptionId == model.SubscriptionId && x.Status == true).FirstOrDefault();

            var OldRecord = _context.ManageSubscriptionModels.Where(x => x.SubscriptionId == model.SubscriptionId && x.Status == true).Select(x=>x.FeatureId). ToList();
            if (OldRecord.Contains(model.FeatureId))
            {
                return "Nothing For Update";

            }
            if (Subscription == null & OldRecord == null)

            {
                return "Invalid Data";
            }   
          


            List<ManageSubscriptionModel> mngsub = new List<ManageSubscriptionModel>();
          
               
                    mngsub.Add(new ManageSubscriptionModel
                    {
                        SubscriptionId = model.SubscriptionId,
                        FeatureId = model.FeatureId,
                        ServiceCount = model.ServiceCount,
                        Status = true
                    });
                
              
            
            _context.ManageSubscriptionModels.UpdateRange(mngsub);
            if (_context.SaveChanges() > 0)
            { 
                return "Successful";
            }
            else
            {
                return "Nothing For Update";
            }
        }

        public List<ManageSubscriptionViewModel> GetAllManageSubscriptionData()
        {

            var Record = _context.ManageSubscriptionModels.Where(x => x.Status == true).Include(x => x.Subscription).Include(x => x.Feature).Select(x=> new ManageSubscriptionViewModel
            {
                Id = x.Id,
                SubscriptionId = x.Subscription.SubscriptionId,
                FeatureId = x.FeatureId,
                SubscriptionName = x.Subscription.Name,
                FeatureName = (from feature in _context.SubscriptionFeatures where feature.FeatureId==x.FeatureId && feature.Status select feature.Name).FirstOrDefault(),
                ServiceCount = x.ServiceCount
            }).OrderByDescending(e=>e.Id).ToList();

            return Record;
        }

        public string DeleteManageSubscriptionData(int Id)
        {
            var Record = _context.ManageSubscriptionModels.Where(x => x.Id == Id).FirstOrDefault();
            if(Record == null) { return "Invalid Data"; }
            Record.Status = false;
            _context.ManageSubscriptionModels.Update(Record);
            if (_context.SaveChanges() > 0)
            {
                return "Successful";
            }
            else
            {
                return "Internal Server Error";
            }
        }



        public List<JyotishPaymentRecordModel> JyotishPaymentrecords()
        {
            
            var record = _context.JyotishPaymentRecord.ToList();
            if (record.Count > 0) { return record; }
            else { return null; }
        }
        public JyotishPaymentRecordModel JyotishPaymentDetail(int Id)
        {
           
            var record = _context.JyotishPaymentRecord.Where(x => x.Id == Id).FirstOrDefault();
            if (record != null) { return record; }
            else { return null; }
        }
        public List<UserPaymentRecordModel> UserPaymentrecords()
        {
            
            var record = _context.UserPaymentRecord.ToList();
            if (record.Count > 0) { return record; }
            else { return null; }
        }
        public UserPaymentRecordModel UserPaymentDetail(int Id)
        {
            
            var record = _context.UserPaymentRecord.Where(x => x.Id == Id).FirstOrDefault();
            if (record != null) { return record; }
            else { return null; }
        }

        public List<ProblemSolutionAdminViewModel> GetAllProblemSolution()
        {
            var result = _context.ProblemSolution
                .Include(ps => ps.Appointment)
                    .ThenInclude(a => a.UserRecord)
                .Include(ps => ps.Appointment)
                    .ThenInclude(a => a.JyotishRecord)
                .AsEnumerable() // Switch to in-memory evaluation for processing
                .GroupBy(ps => new { ps.Appointment.UserId, ps.Appointment.JyotishId, ps.Appointment.Id })
                .Select(group =>
                {
                    var first = group.FirstOrDefault();
                    var slot = _context.AppointmentSlots.FirstOrDefault(s => s.Id == first.Appointment.SlotId);

                    return new ProblemSolutionAdminViewModel
                    {
                        Id = first.Id,
                        UserId = group.Key.UserId,
                        UserName = first.Appointment.UserRecord.Name,
                        UserEmail = first.Appointment.UserRecord.Email,
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
                .ToList();

            return result;
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
                            purchaseDate=plan.PurchaseDate,
                            expiryDate=plan.ExpiryDate
                        }
                       ).FirstOrDefault();


            return data;
        }




        public ProblemSolutionAdminViewModel GetProblemSolution(int id)
        {
            var result = _context.ProblemSolution
                .Where(x => x.Id == id)
                .Include(ps => ps.Appointment)
                    .ThenInclude(a => a.UserRecord)
                .Include(ps => ps.Appointment)
                    .ThenInclude(a => a.JyotishRecord)
                .AsEnumerable() // Switch to in-memory evaluation for processing
                .GroupBy(ps => new { ps.Appointment.UserId, ps.Appointment.JyotishId, ps.Appointment.Id })
                .Select(group =>
                {
                    var first = group.FirstOrDefault();
                    var slot = _context.AppointmentSlots.FirstOrDefault(s => s.Id == first.Appointment.SlotId);

                    return new ProblemSolutionAdminViewModel
                    {
                        Id = first.Id,
                        UserId = group.Key.UserId,
                        UserName = first.Appointment.UserRecord.Name,
                        UserEmail = first.Appointment.UserRecord.Email,
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

        public string ReschduledInterview(ReschduledInterviewViewModel model)
        {
            var Jyotish = _context.JyotishRecords.Where(x => x.Id == model.Id).FirstOrDefault();
            if(Jyotish == null) { return "Jyotish Not Found"; }

            var Slot = _context.Slots.Where(x => x.Id == model.SlotId).FirstOrDefault();
            if(Slot == null) { return "Slot Not Found"; }
            
            var SlotBook = _context.SlotBooking.Where(x => x.JyotishId == model.Id).FirstOrDefault();
            if (SlotBook == null) { return "Jyotish slot details not found"; }
            Slot.Status = "Booked";
           
            SlotBook.SlotId = model.SlotId;
            SlotBook.Date = DateTime.Now;
            _context.SlotBooking.Update(SlotBook);
            _context.Slots.Update(Slot);
            if (_context.SaveChanges() > 0)
            {
                return "Successful";
            }
            else
            {
                return "Internal Server Error.";
            }
        }

        public List<InteviewListViewModel> InteviewList()
        {
            var data = _context.JyotishRecords.Where(x=>x.Status == true)
             .Include(j => j.Slots)
             .ThenInclude(s => s.SlotRecords)
             .SelectMany(j => j.Slots.Select(s => new InteviewListViewModel
             {
                 Id = j.Id,
                 Name = j.Name,
                 Mobile = j.Mobile,
                 Email = j.Email,
                 Expertise = j.Expertise,
                 Experience = j.Experience,
                 Language = j.Language,
                 Date = s.SlotRecords.Date,
                 Time = s.SlotRecords.Time,
                 SlotId = s.SlotId
             }))
             .OrderByDescending(i => i.Date) // Order by date in descending order
             .ToList();

            return data;
        }

        public JyotishModel JyotishProfile(int Id)
        {
            var data = _context.JyotishRecords.Where(x => x.Id == Id).FirstOrDefault();
            return data;
        }

        public List<AdminNoticationDataViewModel> NotificationData()
        {
            var Data = _context.SlotBooking.Include(x => x.SlotRecords).Include(x => x.JyotishRecords).Where(x => x.SlotRecords.Date == DateOnly.FromDateTime(DateTime.Now.Date) && x.SlotRecords.Status == "Booked").Take(5).Select(x => new AdminNoticationDataViewModel
            {
                Name = x.JyotishRecords.Name,
                Date = x.SlotRecords.Date.ToString(),
                Time = x.SlotRecords.Time.ToString(),
                TimeDuration = x.SlotRecords.TimeDuration,

            }).ToList();

            return Data;

        }

        //country code
        public bool countryCode(CountryCodeViewModel model)
        {
            CountryCode code = new CountryCode
            {
                country = model.country,
                countryCode = model.countryCode
            };
            _context.CountryCode.Add(code);
            return _context.SaveChanges() > 0;
        }
        public string AddInterviewFeedback(InterviewFeedbackViewModel feedback)
        {
            var record = _context.InterviewFeedback.Where(x => x.InterviewId == feedback.InterviewId).FirstOrDefault();
            if (record != null) { return "Invalid Data"; }

            InterviewFeedbackModel Data = new InterviewFeedbackModel();
            Data.InterviewId = feedback.InterviewId;
            Data.Message = feedback.Message;
            Data.ApprovedStatus = feedback.ApprovedStatus;
            Data.JyotishId = feedback.JyotishId;
            Data.Date = DateTime.Now;
            Data.Status = true;
            _context.InterviewFeedback.Add(Data);
            if ( _context.SaveChanges()>0 ) 
            {
                var jyotish = _context.JyotishRecords.Where(x => x.Id == feedback.JyotishId).FirstOrDefault();
                if (feedback.ApprovedStatus == true)
                {
                    jyotish.ApprovedStatus = "Complete";
                    jyotish.Role = "Jyotish";
                    var message = "I am pleased to formally accept your offer for the Jyotish role at My Jyotish G. I am excited to join your team and contribute to the success of the company. I truly appreciate the opportunity and the confidence you have shown in me.";
                    var subject = "Interview Outcome";
                    SendEmail(message, jyotish.Email, subject);
                }
                else {
                    jyotish.ApprovedStatus = "Rejected";
                    jyotish.Role = "Rejected";
                    jyotish.Status = false;
                    var message = "Thank you very much for taking the time to interview for the Jyotish role with My Jyotish G. After careful consideration, we regret to inform you that we will not be moving forward with your application at this time.";
                    var subject = "Interview Outcome";
                    SendEmail(message, jyotish.Email, subject);
                }
                _context.JyotishRecords.Update(jyotish);
                _context.SaveChanges();
                return "Successful";
            }
            return "Internal Server Error";
        }

        public dynamic getJyotishByEmail(string email)
        {
            var res = (from jyotish in _context.JyotishRecords
                       join plan in _context.PackageManager on jyotish.Id equals plan.JyotishId
                       join subs in _context.Subscriptions on plan.SubscriptionId equals subs.SubscriptionId where jyotish.Email==email && plan.Status && jyotish.Status select new
                       {
                           name=jyotish.Name,
                           mobno=jyotish.Mobile,
                           country=jyotish.Country,
                           state=jyotish.State,
                           city=jyotish.City,
                           gender=jyotish.Gender,
                           planName=subs.Name,
                           planType=subs.PlanType,
                           planPurchaseDate=plan.PurchaseDate,
                           planExpiryDate=plan.ExpiryDate
                       }).FirstOrDefault();

            return res;


        }

        public List<InterviewFeedbackViewModel> GetAllInterviewFeedback()
        {
            var records = _context.InterviewFeedback.Where(x => x.Status == true).Include(x => x.Jyotish).Include(x => x.Slot).Select(x => new InterviewFeedbackViewModel
            {
                Id = x.Id,
                InterviewId = x.InterviewId,
                Message = x.Message,
                ApprovedStatus = x.ApprovedStatus,
                JyotishName = x.Jyotish.Name,
                Date = DateTime.Parse(x.Slot.Date + " " + x.Slot.Time),
            }
            ).ToList();
            return records;
        }

        public dynamic getCountryCode()
        {
            var res = (from code in _context.CountryCode join country in _context.Countries on code.country equals country.Id select new
            {
                id=code.Id,
                country=country.Name,
                countryId=country.Id,
                code=code.countryCode
            }).ToList();
          
            return res;
        }

        public bool AddRedeamCode(redeamCodeViewModel model)
        {
            var res = _context.JyotishRecords.Where(e => e.Email == model.email && e.Status).FirstOrDefault();
            if (res == null)
            {
                redeamCode rcode = new redeamCode
                {
                    PlanId = model.PlanId,
                    jyotishId = res.Id,
                    discount = model.discount,
                    ReadeamCode = model.ReadeamCode,
                    discountAmount = model.discountAmount,
                    date=DateTime.Now,
                    status = true
                };

                _context.RedeamCode.Add(rcode);
                return _context.SaveChanges() > 0;
            }
            else
            {
                return false;
            }
        }

    }
}
