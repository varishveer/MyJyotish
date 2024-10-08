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
                             .Where(record => record.Role == "pending")
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

        public List<AppointmentModel> GetAllAppointment()
        {
            var Records = _context.AppointmentRecords.ToList();
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
       
            Jyotish.Status = "Approved";
            Jyotish.Role = "Jyotish";
            _context.JyotishRecords.Update(Jyotish);
            var result = _context.SaveChanges();
            if (result > 0)
            {
                string message = "Hey "+ Jyotish.Name+ ", Your Account has been Activated Successfully.";
                string subject = " My Jyotish G";
                SendEmail(message, Jyotish.Email, subject);
                return true;
            }
            else { return false; }
            
        }
        
        public bool RejectJyotish(IdViewModel JyotishId)
        {
            var Jyotish = _context.JyotishRecords.Where(x => x.Id == JyotishId.Id).FirstOrDefault();
            if (Jyotish == null)
            { return false; }
            Jyotish.Status = "Rejected";
            _context.JyotishRecords.Update(Jyotish);
            var result = _context.SaveChanges();
            if (result > 0) 
            { return true;  }
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
            if (Jyotish == null || Jyotish.Status != "Rejected")
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
        public bool AddPoojaCategory(PoojaCategoryViewModel _pooja)
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

        }
        public List<PoojaCategoryModel> GetAllPoojaCategory()
        {
            var records = _context.PoojaCategory.ToList();
            return records;
        }

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
        public AppointmentModel AppointmentDetails(int id)
        {
            AppointmentModel Record =  _context.AppointmentRecords.Where(x=>x.Id == id).FirstOrDefault();
            if (Record == null)
            { return null; }
            else { return Record; }
        }

        public bool UpdateAppointment(AppointmentModel model)
        {
           var isDetailsValid = _context.AppointmentRecords.Where(x => x.Id == model.Id).FirstOrDefault();
            if (isDetailsValid == null)
            { return false; }
            isDetailsValid.Name = model.Name;
            isDetailsValid.Mobile = model.Mobile;
            isDetailsValid.DateTime = model.DateTime; 
            isDetailsValid.Email = model.Email;
            isDetailsValid.JyotishId = model.JyotishId;
            isDetailsValid.UserId = model.UserId;
            isDetailsValid.Problem = model.Problem;
            isDetailsValid.Solution = model.Solution;
            isDetailsValid.Status = model.Status;
            isDetailsValid.Amount = model.Amount;



            _context.AppointmentRecords.Update(isDetailsValid);
            var result = _context.SaveChanges();
            if (result > 0)
            { return true; }
            else
            { return false; }
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
            Record.Status = model.Status;
            Record.Otp = model.Otp;
            Record.Experience = model.Experience;
            Record.Pooja = model.Pooja;
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

        public string ApproveJyotishDocs(EmailDocumentViewModel model)
        {
            var jyotish = _context.JyotishRecords.Where(x => x.Id == model.Id).FirstOrDefault();
            if (jyotish == null) { return "Jyotish not found"; }
            var record = _context.Documents.Where(x => x.JId == jyotish.Id).FirstOrDefault();
            if(record == null) { return "documents not found"; }

            switch (model.ImageStatus)
            {
                case "IdProofStatus":
                    record.IdProofStatus = "Verified";
                   
                    break;

                case "AddressProofStatus":
                    record.AddressProofStatus = "Verified";
                    break;

                case "TenthCertificateStatus":
                    record.TenthCertificateStatus = "Verified";
                    break;

                case "TwelveCertificateStatus":
                    record.TwelveCertificateStatus = "Verified";
                    break;

                case "ProfessionalCertificateStatus":
                    record.ProfessionalCertificateStatus = "Verified";
                    break;

                default:
                    return "invalid data";
                    break;
            }
            _context.Documents.Update(record);
            _context.SaveChanges();
            AccountServices.SendEmail(model.Message, jyotish.Email, model.Subject);
           
            return "Successful";
        }
        public string RejectJyotishDocs(EmailDocumentViewModel model)
        {
            var jyotish = _context.JyotishRecords.Where(x => x.Id == model.Id).FirstOrDefault();
            if (jyotish == null) { return "Jyotish not found"; }
            var record = _context.Documents.Where(x => x.JId == jyotish.Id).FirstOrDefault();
            if (record == null) { return "documents not found"; }

            switch (model.ImageStatus)
            {
                case "IdProofStatus":
                    record.IdProofStatus = "Rejected";

                    break;

                case "AddressProofStatus":
                    record.AddressProofStatus = "Rejected";
                    break;

                case "TenthCertificateStatus":
                    record.TenthCertificateStatus = "Rejected";
                    break;

                case "TwelveCertificateStatus":
                    record.TwelveCertificateStatus = "Rejected";
                    break;

                case "ProfessionalCertificateStatus":
                    record.ProfessionalCertificateStatus = "Rejected";
                    break;

                default:
                    return "invalid data";
                    break;
            }
            _context.Documents.Update(record);
            _context.SaveChanges();


            AccountServices.SendEmail(model.Message, jyotish.Email, model.Subject);
            return "Successful";
        }

        public List<SlotModel> SlotList()
        {
            DateTime today = DateTime.Today;
            var Slots = _context.Slots.Where(slot => slot.Date >= today).ToList();
            return Slots;
        }
        public string AddSlot(SlotViewModel model)
        { 
           if(model == null)
           { return "Invalid Data"; }
            else
            {
                SlotModel slot = new SlotModel
                {
                    Time = model.Time,
                    Date = model.Date,
                    Status = "Vacant"
                };
                
                _context.Slots.Add(slot);
                var result = _context.SaveChanges();
                if (result > 0) { return "Successful"; }
                else { return "Internal Server Error"; }
            }
        }

      /*  public string ApproveDocument(DocUpdateViewModel model)
        {
            var Jyotish = _context.JyotishRecords.Where(x => x.Id == model.JyotishId).FirstOrDefault();

            if (Jyotish == null) { return "Jyotish Not Found"; }
            var Records = _context.Documents.Where(x => x.JId == Jyotish.Id).FirstOrDefault();
            if (Records == null) { return "Docs Not Found"; }

            

            switch (model.ImageStatus)
            {
                case "IdProofStatus":
                    Records.IdProofStatus = "Verified";
                    break;
                case "AddressProofStatus":
                    Records.AddressProofStatus = "Verified";
                    break;
                case "ProfessionalCertificateStatus":
                    Records.ProfessionalCertificateStatus = "Verified";
                    break;
                case "TenthCertificateStatus":
                    Records.TenthCertificateStatus = "Verified";
                    break;
                case "TwelveCertificateStatus":
                    Records.TwelveCertificateStatus = "Verified";
                    break;
                default:
                    return "Invalid ImageStatus";
                    break;
            }

            _context.Documents.Update(Records);
            var Result = _context.SaveChanges();
            if(Result > 0 )
            {
                string message = @$"{model.ImageStatus} is Approved";
                string subject = "Docs Approved";
                AccountServices.SendEmail(message, Jyotish.Email, subject);
                return "Successful"; }
            else
            { return "Data Not Saved"; }
        }


        public string RejectDocument(DocUpdateViewModel model)
        {
            var Jyotish = _context.JyotishRecords.Where(x => x.Id == model.JyotishId).FirstOrDefault();

            if (Jyotish == null) { return "Jyotish Not Found"; }
            var Records = _context.Documents.Where(x => x.JId == Jyotish.Id).FirstOrDefault();
            if (Records == null) { return "Docs Not Found"; }



            switch (model.ImageStatus)
            {
                case "IdProofStatus":
                    Records.IdProofStatus = "Reject";
                    break;
                case "AddressProofStatus":
                    Records.AddressProofStatus = "Reject";
                    break;
                case "ProfessionalCertificateStatus":
                    Records.ProfessionalCertificateStatus = "Reject";
                    break;
                case "TenthCertificateStatus":
                    Records.TenthCertificateStatus = "Reject";
                    break;
                case "TwelveCertificateStatus":
                    Records.TwelveCertificateStatus = "Reject";
                    break;
                default:
                    return "Invalid ImageStatus";
                    break;
            }

            _context.Documents.Update(Records);
            var Result = _context.SaveChanges();
            if (Result > 0)
            {
                string message = @$"{model.ImageStatus} is Rejected";
                string subject = "Docs Rejected";
                AccountServices.SendEmail(message, Jyotish.Email, subject);
                return "Successful"; }
            else
            { return "Data Not Saved"; }
        }*/

      
    }
}
