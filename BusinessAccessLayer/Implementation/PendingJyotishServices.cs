using BusinessAccessLayer.Abstraction;
using DataAccessLayer.DbServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ModelAccessLayer.Models;
using ModelAccessLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace BusinessAccessLayer.Implementation
{
    public class PendingJyotishServices:IPendingJyotishServices
    {
        private readonly ApplicationContext _context;
        private readonly string _uploadDirectory;
        public PendingJyotishServices(ApplicationContext context)
        {
            _context = context;
            // Set the directory where files will be saved
            _uploadDirectory = Directory.GetCurrentDirectory() ;

            // Ensure directory exists
            if (!Directory.Exists(_uploadDirectory))
            {
                Directory.CreateDirectory(_uploadDirectory);
            }
        }




        public async Task<string> UploadDocumentAsync(DocumentViewModel model)
        {
            const long MaxFileSize = 5 * 1024 * 1024; 
            var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };

            var isJyotishValid = await _context.JyotishRecords
                .FirstOrDefaultAsync(x => x.Id == model.JyotishId);

            if(model.IdProof == null|| model.AddressProof == null|| model.ProfessionalCertificate == null)
            {
                return "Invalid Data";
            }
            if (isJyotishValid == null)
            {
                return "Invalid Jyotish";
            }

            var existingDocument = await _context.Documents
                .FirstOrDefaultAsync(x => x.JId == isJyotishValid.Id);

            var document = existingDocument ?? new DocumentModel { JId = isJyotishValid.Id };

            try
            {
                var documentsToUpload = new[]
                {
                    (model.IdProof, (Action<string>)(path => document.IdProof = path)),
                    (model.AddressProof, (Action<string>)(path => document.AddressProof = path)),
                    (model.TenthCertificate, (Action<string>)(path => document.TenthCertificate = path)),
                    (model.TwelveCertificate, (Action<string>)(path => document.TwelveCertificate = path)),
                    (model.ProfessionalCertificate, (Action<string>)(path => document.ProfessionalCertificate = path)),
                };

                foreach (var (file, setPath) in documentsToUpload)
                {
                    


                        // Update the corresponding status property
                        if (file != null && file == model.IdProof )
                        {
                            if(ValidateFile(file, MaxFileSize, allowedExtensions))
                           { document.IdProofStatus = "Unverified";
                            var filePath = await SaveFileAsync(file);
                            setPath(filePath);
                            }
                            else { return "Invalid File Extension or Size"; }
                        }
                        else if (file != null && file == model.AddressProof)
                        {
                            if (ValidateFile(file, MaxFileSize, allowedExtensions))
                               { document.AddressProofStatus = "Unverified";
                                var filePath = await SaveFileAsync(file);
                                setPath(filePath);
                                }
                            else { return "Invalid File Extension or Size"; }
                        }

                        else if (file != null && file == model.TenthCertificate)
                         {
                            if (ValidateFile(file, MaxFileSize, allowedExtensions))
                            {
                                document.TenthCertificateStatus = "Unverified";
                                var filePath = await SaveFileAsync(file);
                                setPath(filePath);
                            }
                            else { return "Invalid File Extension or Size"; }

                        }
                        else if (file != null && file == model.TwelveCertificate)
                        {
                            if (ValidateFile(file, MaxFileSize, allowedExtensions))
                            {
                                document.TwelveCertificateStatus = "Unverified";
                                var filePath = await SaveFileAsync(file);
                                setPath(filePath);
                            }
                            else { return "Invalid File Extension or Size"; }
                       
                        }
                        else if (file != null && file == model.ProfessionalCertificate)
                        {

                            if (ValidateFile(file, MaxFileSize, allowedExtensions))
                            {
                                document.ProfessionalCertificateStatus = "Unverified";
                                var filePath = await SaveFileAsync(file);
                                setPath(filePath);
                            }
                            else { return "Invalid File Extension or Size"; }
                       
                        }
                    
                }

                if (existingDocument == null)
                {
                    await _context.Documents.AddAsync(document);
                }
                else
                {
                    _context.Documents.Update(document);
                }
                if(await _context.SaveChangesAsync() > 0)
                {

                   var tempData =  _context.jyotishTempRecords.Where(x => x.JyotishId == isJyotishValid.Id).FirstOrDefault();
                    if(tempData == null)
                    {
                        return "Invalid Data";
                    }
           
                    isJyotishValid.AlternateMobile = tempData.AlternateMobile;
                    isJyotishValid.Name = tempData.Name;
                    isJyotishValid.Gender = tempData.Gender;
                    isJyotishValid.Language = tempData.Language;
                    isJyotishValid.Country = tempData.Country;
                    isJyotishValid.State = tempData.State;
                    isJyotishValid.City = tempData.City;
                    isJyotishValid.DateOfBirth = tempData.DateOfBirth;
                    isJyotishValid.ProfileImageUrl = tempData.Image;
                    isJyotishValid.Experience = tempData.Experience;
                    isJyotishValid.Pooja = tempData.Pooja;
                    isJyotishValid.Call = tempData.Call;
                    isJyotishValid.CallCharges = tempData.CallCharges;
                    isJyotishValid.Chat = tempData.Chat;
                    isJyotishValid.ChatCharges = tempData.ChatCharges;
                    isJyotishValid.TimeTo = tempData.TimeTo;
                    isJyotishValid.TimeFrom = tempData.TimeFrom;
                    isJyotishValid.About = tempData.About;
                    isJyotishValid.AwordsAndAchievement = tempData.AwordsAndAchievement;
                    isJyotishValid.Specialization = tempData.Specialization;
                    isJyotishValid.Expertise = tempData.Expertise;
                    isJyotishValid.Address = tempData.Address;
                    isJyotishValid.Pincode = tempData.Pincode;
                    isJyotishValid.NewStatus = false;
                    _context.JyotishRecords.Update(isJyotishValid);
                    if (_context.SaveChanges() > 0)
                    { return "Successful"; }
                    else
                    {
                        return "Internal Server Error.";
                    }
                   
                }
                else { return "Internal server DB error"; }
                
            }
            catch (Exception ex)
            {
               
                return $"Internal server EX error = {ex.Message}";
            }
        }

        public DocumentModel Documents(int Id)
        {
            var isJyotishValid =  _context.JyotishRecords.Where(x => x.Id == Id).FirstOrDefault();
            if (isJyotishValid == null) { return null; }
            var isDocumentAvailable =  _context.Documents.Where(x=>x.JId == Id).FirstOrDefault();
            if (isDocumentAvailable == null) { return null; }
            else {

                isDocumentAvailable.Jyotish = null;
                return isDocumentAvailable; 
            }
        }

        public async Task<JyotishModel> ProfileData(int Id)
        {
      
            var result = await _context.JyotishRecords.Where(x => x.Id == Id).FirstOrDefaultAsync();
            if (result == null) { return null; }

            return result;
        }


        public async Task<JyotishTempRecord> ProfileTempData(int Id)
        {

            var result = await _context.JyotishRecords.Where(x => x.Id == Id).FirstOrDefaultAsync();
            if (result == null) { return null; }
            var records = await _context.jyotishTempRecords.Where(x => x.JyotishId == result.Id).FirstOrDefaultAsync();
            return records;
        }


        public string UpdateProfile(JyotishTempViewModel model, string? path)
        {
            if (model == null) return "Invalid Data";

            // Find the existing record
            var existingRecord = _context.jyotishTempRecords
                .FirstOrDefault(x => x.JyotishId == model.Id);

            if (existingRecord == null)
            {
                JyotishTempRecord newRecord = new JyotishTempRecord();
                newRecord.BasicSection = true;
                newRecord.Name = model.Name;
                newRecord.Email = existingRecord.Email;
                newRecord.Mobile = existingRecord.Mobile;
                newRecord.AlternateMobile = model.AlternateMobile;
                newRecord.Gender = model.Gender;
                newRecord.Language = model.Language;
                newRecord.DateOfBirth = model.DateOfBirth;
                string filePath = string.Empty;
                string ImageUrl = string.Empty;
                if (model.Image != null)
                {
                    // Generate a unique file name
                    var uniqueFileName = $"{Guid.NewGuid()}_{model.Image.FileName}";
                    filePath = $"wwwroot/Images/Jyotish/{uniqueFileName}";
                    ImageUrl = $"Images/Jyotish/{uniqueFileName}";
                    var fullPath = path + "/" + filePath;
                    UploadFile(model.Image, fullPath);


                    newRecord.Image = ImageUrl;
                }
                _context.jyotishTempRecords.Add(newRecord);
               
               
                var Result = _context.SaveChanges();

                if (Result > 0) {

                    return "Successful"; }
                else { return "Internal Server Error"; }

            }

            /*-----------------------------Basic-----------------------------------------*/
            if (model.BasicSection == true)
            {
                var Record = _context.jyotishTempRecords.Where(x => x.JyotishId == existingRecord.Id).FirstOrDefault();
                if(Record == null) { return "Invalid Data"; }
                Record.BasicSection = true;
                Record.Name = model.Name;
                Record.Email = existingRecord.Email;
                Record.Mobile = existingRecord.Mobile;
                Record.AlternateMobile = model.AlternateMobile;
                Record.Gender = model.Gender;
                Record.Language = model.Language;
                Record.DateOfBirth = model.DateOfBirth;
                string filePath = string.Empty;
                string ImageUrl = string.Empty;
                if (model.Image != null)
                {
                    // Generate a unique file name
                    var uniqueFileName = $"{Guid.NewGuid()}_{model.Image.FileName}";
                    filePath = $"wwwroot/Images/Jyotish/{uniqueFileName}";
                    ImageUrl = $"Images/Jyotish/{uniqueFileName}";
                    var fullPath = path + "/" + filePath;
                    UploadFile(model.Image, fullPath);


                    Record.Image = ImageUrl;
                }
                _context.jyotishTempRecords.Update(Record);
                var Result = _context.SaveChanges();
                if (Result > 0) { return "Successful"; }
                else { return "Internal Server Error"; }
            }
            /*-----------------------------Address-----------------------------------------*/
            if (model.AddressSection == true) 
            {
                var Record = _context.jyotishTempRecords.Where(x => x.JyotishId == existingRecord.Id).FirstOrDefault();
                if (Record == null) { return "Invalid Data"; }

                var countryName = _context.Countries
                    .Where(x => x.Id == model.Country)
                    .Select(x => x.Name)
                    .FirstOrDefault();

                var stateName = _context.States
                    .Where(x => x.Id == model.State)
                    .Select(x => x.Name)
                    .FirstOrDefault();

                var cityName = _context.Cities
                    .Where(x => x.Id == model.City)
                    .Select(x => x.Name)
                    .FirstOrDefault();
                Record.AddressSection = true;
                Record.City = cityName;
                Record.State = stateName;
                Record.Country = countryName;
                Record.Address = model.Address;
                Record.Pincode = model.pincode;

                _context.jyotishTempRecords.Update(Record);
                var Result = _context.SaveChanges();
                if (Result > 0) { return "Successful"; }
                else { return "Internal Server Error"; }


            }
            /*----------------------------------Availbility---------------------------------------*/

            if (model.AvailbilitySection == true)
            {

                var Record = _context.jyotishTempRecords.Where(x => x.JyotishId == existingRecord.Id).FirstOrDefault();
                if (Record == null) { return "Invalid Data"; }

                Record.AvailbilitySection = true;
                Record.Pooja = model.Pooja;
                Record.Call = model.Call;
                Record.CallCharges = model.CallCharges;
                Record.Chat = model.Chat;
                Record.ChatCharges = model.ChatCharges;
                Record.TimeFrom = model.TimeFrom;
                Record.TimeTo = model.TimeTo;
              
                _context.jyotishTempRecords.Update(Record);
                var Result = _context.SaveChanges();
                if (Result > 0) { return "Successful"; }
                else { return "Internal Server Error"; }
            }
            /*----------------------------------About---------------------------------------*/
            if (model.AboutSection == true)
            {

                var Record = _context.jyotishTempRecords.Where(x => x.JyotishId == existingRecord.Id).FirstOrDefault();
                if (Record == null) { return "Invalid Data"; }

                Record.AboutSection = true;
                Record.About = model.About;
                Record.AwordsAndAchievement = model.AwordsAndAchievement;
                Record.Specialization = model.Specialization;
                Record.Expertise = model.Expertise;
                Record.Experience = model.Experience;
         

                _context.jyotishTempRecords.Update(Record);
                var Result = _context.SaveChanges();
                if (Result > 0) { return "Successful"; }
                else { return "Internal Server Error"; }
            }
            
            return "Invalid Data";
        }

        public string Role(string Email)
        {
            var Jyotish = _context.JyotishRecords.Where(x => x.Email == Email).FirstOrDefault();
            if (Jyotish == null) { return null; }
            string Role = Jyotish.Role;
            return Role;
        }
        public string ProfileImage(string Email)
        {
            var Jyotish = _context.JyotishRecords.Where(x => x.Email == Email).FirstOrDefault();
            if (Jyotish == null) { return null; }
           
            return Jyotish.ProfileImageUrl;
        }

        public string AddSlotBooking(SlotBookingAddViewModel model)
           {
               var Jyotish = _context.JyotishRecords.Where(x => x.Id == model.JyotishId).FirstOrDefault();
               if (Jyotish == null) { return "Jyotish Not Found"; }

               DateTime today = DateTime.Today;
               var Slot = _context.SlotBooking.Where(x=>x.JyotishId == model.JyotishId).FirstOrDefault();
               if( Slot != null)
               { return "Your Slot Already Booked"; }


      
                var slot = _context.Slots.Where(y => y.Id == model.SlotId).FirstOrDefault();
          
            if (slot.Status == "Booked")
            {
                return "This Slot Already Booked";
            }
            slot.Status = "Booked";

            SlotBookingModel NewRecord = new SlotBookingModel();

            NewRecord.Date = DateTime.Now;
               NewRecord.JyotishId = model.JyotishId;
            NewRecord.SlotId = model.SlotId;
                _context.Slots.Update(slot);
               _context.SlotBooking.Add(NewRecord);
               if (_context.SaveChanges() > 0) 
               {
                   string message = $@"Dear {Jyotish.Name}, \n

   We are pleased to inform you that your interview has been scheduled. Below are the details for your upcoming virtual interview:\n

   Date: {slot.Date}\n
   Time: {slot.Time}\n
   The meeting link and further instructions will be shared with you closer to the interview date.\n

   If you have any questions or need to reschedule, feel free to reply to this email or contact us at myjyotishG@gmail.com.\n

   We look forward to speaking with you!";
                   string subject = " Interview Scheduled – MyJyotishG";
                   AccountServices.SendEmail(message, Jyotish.Email, subject);
                    
                   return "Successful";
               }
               else { return "Data Not Saved"; }
           }
       

        public List<SlotListViewModel> SlotList()
        {
            DateTime today = DateTime.Today;
            DateOnly todayDate = DateOnly.FromDateTime(today);

            var groupedSlots = _context.Slots
                .Where(slot => slot.Date >= todayDate)  
                .GroupBy(slot => slot.Date) 
                .OrderBy(group => group.Key)  
                .Select(group => new SlotListViewModel
                {
                    Date = group.Key,
                    Times = group
                        .OrderBy(slot => slot.Time)  
                        .Select(slot => new TimeStatusViewModel
                        {
                            Id = slot.Id,
                            Time = slot.Time,  
                            Status = slot.Status
                        })
                        .ToList()
                })
                .ToList();

            return groupedSlots;
        }
        public SlotBookingModel JyotishSlotDetails(int id)
        {
            var Jyotish = _context.JyotishRecords.Where(x => x.Id == id).FirstOrDefault();
            if (Jyotish == null) { return null; }
            var record = _context.SlotBooking
                .Where(x => x.JyotishId == id && x.Date >= DateTime.Today)
                .FirstOrDefault();
            return record;
        }



        private async Task<string> SaveFileAsync(IFormFile file)
        {
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var relativePath = Path.Combine("PendingJyotish", "Document", uniqueFileName);
            var fullPath = Path.Combine(_uploadDirectory, "wwwroot", relativePath);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return "/" + relativePath; // Return relative URL
        }

        private bool ValidateFile(IFormFile file, long maxSize, string[] allowedExtensions)
        {
            return file.Length <= maxSize && allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower());
        }

        public void UploadFile(IFormFile file, string fullPath)
        {
            FileStream stream = new FileStream(fullPath, FileMode.Create);
            file.CopyTo(stream);

        }
    }
}
