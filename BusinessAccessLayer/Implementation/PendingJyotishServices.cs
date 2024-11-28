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
using System.IO;

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
        public async Task<JyotishModel> ProfileData(int Id)
        {

            var result = await _context.JyotishRecords.Where(x => x.Id == Id && x.Status == true).FirstOrDefaultAsync();
            if (result == null) { return null; }

            return result;
        }
        public async Task<JyotishTempRecord> ProfileTempData(int Id)
        {
           
            var records = await _context.jyotishTempRecords.Where(x => x.JyotishId == Id).FirstOrDefaultAsync();
            return records;
        }
        public string UploadProfileImage(UploadProfileImagePJViewModel model, string? path)
        {
            var record = _context.JyotishRecords.FirstOrDefault(x => x.Id == model.Id);
            if (record == null) return "Jyotish Not Found";

            var tempRecord = _context.jyotishTempRecords.FirstOrDefault(x => x.JyotishId == model.Id);
            if (model.Image == null) return "Invalid Data";

            // Generate the image file path and URL
            var uniqueFileName = $"{Guid.NewGuid()}_{model.Image.FileName}";
            var filePath = $"wwwroot/Images/Jyotish/{uniqueFileName}";
            var imageUrl = $"Images/Jyotish/{uniqueFileName}";
            var fullPath = Path.Combine(path, filePath);

            // Upload the file
            UploadFile(model.Image, fullPath);

            if (tempRecord == null)
            {
                // Create new temp record if it doesn't exist
                tempRecord = new JyotishTempRecord
                {
                    JyotishId = model.Id,
                    Image = imageUrl
                };
                _context.jyotishTempRecords.Add(tempRecord);
            }
            else
            {
                // Update existing temp record
                tempRecord.Image = imageUrl;
                _context.jyotishTempRecords.Update(tempRecord);
            }

            return _context.SaveChanges() > 0 ? "Successful" : "Internal Server Error.";
        }
        public string UpdateProfile(JyotishTempViewModel model, string? path)
        {
            if (model == null) return "Invalid Data";

            var jyotish = _context.JyotishRecords.Where(x => x.Id == model.Id).FirstOrDefault();
            // Find the existing record
            var existingRecord = _context.jyotishTempRecords
                .FirstOrDefault(x => x.JyotishId == model.Id);
           

            if (existingRecord == null)
            {
                JyotishTempRecord newRecord = new JyotishTempRecord();
                newRecord.JyotishId = model.Id;
                newRecord.BasicSection = true;
                newRecord.Name = model.Name;
                newRecord.Email = jyotish.Email;
                newRecord.Mobile = jyotish.Mobile;
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

                if (Result > 0)
                {

                    return "Successful";
                }
                else { return "Internal Server Error"; }

            }

            /*-----------------------------Basic-----------------------------------------*/
            if (model.BasicSection == true)
            {
                DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
                DateOnly dateOfBirth = (DateOnly)model.DateOfBirth;
                // Calculate the age by subtracting the birth year from the current year
                int age = currentDate.Year - dateOfBirth.Year;

                // Adjust the age if the birthday hasn't occurred yet this year
                if (currentDate.Month < dateOfBirth.Month ||
                    (currentDate.Month == dateOfBirth.Month && currentDate.Day < dateOfBirth.Day))
                {
                    age--;
                }

                // Check if the age is less than 20
                if (age < 20)
                {
                    return "Please select your Dob 20 or more than 20 year!";
                }

                var Record = _context.jyotishTempRecords.Where(x => x.JyotishId == model.Id).FirstOrDefault();
                if (Record == null) { return "Invalid Data"; }
                Record.BasicSection = true;
                Record.Name = model.Name;
                Record.Email = jyotish.Email;
                Record.Mobile = jyotish.Mobile;
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
                var Record = _context.jyotishTempRecords.Where(x => x.JyotishId == model.Id).FirstOrDefault();
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

                var Record = _context.jyotishTempRecords.Where(x => x.JyotishId == model.Id).FirstOrDefault();
                if (Record == null) { return "Invalid Data"; }

                Record.AvailbilitySection = true;
                Record.Pooja = model.Pooja;
                Record.Call = model.Call;
                Record.CallCharges = model.CallCharges;
                Record.Chat = model.Chat;
                Record.ChatCharges = model.ChatCharges;
                Record.Appointment = model.Appointment;
                Record.AppointmentCharges = model.AppointmentCharges;
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

                var Record = _context.jyotishTempRecords.Where(x => x.JyotishId == model.Id).FirstOrDefault();
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
                    isJyotishValid.ProfileImageUrl = tempData.Image!=null? tempData.Image: isJyotishValid.ProfileImageUrl;
                    isJyotishValid.Experience = tempData.Experience;
                    isJyotishValid.Pooja = tempData.Pooja;
                    isJyotishValid.Call = tempData.Call;
                    isJyotishValid.CallCharges = tempData.CallCharges;
                    isJyotishValid.Chat = tempData.Chat;
                    isJyotishValid.ChatCharges = tempData.ChatCharges;
                    isJyotishValid.Appointment = tempData.Appointment;
                    isJyotishValid.AppointmentCharges = tempData.AppointmentCharges;
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
                    {
                        var deleteTemp = _context.jyotishTempRecords.Where(x => x.JyotishId == model.JyotishId).FirstOrDefault();
                        _context.jyotishTempRecords.Remove(deleteTemp);
                        _context.SaveChanges();
                        return "Successful";
                    }
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
        public List<SlotListViewModel> SlotList()
        {
            var groupedSlots = _context.Slots
                .Where(slot => slot.Date >= DateOnly.FromDateTime(DateTime.Now.Date))
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
                        }).ToList()
                })
                .ToList();

            return groupedSlots;
        }
        public string AddSlotBooking(SlotBookingAddViewModel model)
        {
            var Jyotish = _context.JyotishRecords.Where(x => x.Id == model.JyotishId).FirstOrDefault();
            if (Jyotish == null) { return "Jyotish Not Found"; }

            //  DateTime today = DateTime.Today;
            var Slot = _context.SlotBooking.Where(x => x.JyotishId == model.JyotishId).FirstOrDefault();
            if (Slot != null)
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
        public SlotBookingDetailViewModel JyotishSlotDetails(int id)
        {
            var Jyotish = _context.JyotishRecords.Where(x => x.Id == id).FirstOrDefault();
            if (Jyotish == null) { return null; }
            var record = _context.SlotBooking
                .Where(x => x.JyotishId == id && x.Date >= DateTime.Today.Date)
                .FirstOrDefault();
            if (record == null) { return null; }
            var result = _context.Slots.Where(x => x.Id == record.SlotId).FirstOrDefault();
            if (result == null) { return null; }
            SlotBookingDetailViewModel data = new SlotBookingDetailViewModel();
            data.Date = result.Date;
            data.Time = result.Time;
            data.TimeDuration = result.TimeDuration;
            return data;
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

        public async Task<string> UploadRejectedDocumentAsync(DocumentViewModel model)
        {
            const long MaxFileSize = 5 * 1024 * 1024;
            var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };

            var isJyotishValid = await _context.JyotishRecords
                .FirstOrDefaultAsync(x => x.Id == model.JyotishId);

            if (isJyotishValid == null)
            {
                return "Invalid Jyotish";
            }

            var existingDocument = await _context.Documents
                .FirstOrDefaultAsync(x => x.JId == isJyotishValid.Id);

            if (existingDocument == null)
            {
                return "Document record not found for the given Jyotish ID";
            }

            try
            {
                var documentsToUpload = new[]
                {
                    (model.IdProof, (Action<string>)(path => existingDocument.IdProof = path), existingDocument.IdProofStatus),
                    (model.AddressProof, (Action<string>)(path => existingDocument.AddressProof = path), existingDocument.AddressProofStatus),
                    (model.TenthCertificate, (Action<string>)(path => existingDocument.TenthCertificate = path), existingDocument.TenthCertificateStatus),
                    (model.TwelveCertificate, (Action<string>)(path => existingDocument.TwelveCertificate = path), existingDocument.TwelveCertificateStatus),
                    (model.ProfessionalCertificate, (Action<string>)(path => existingDocument.ProfessionalCertificate = path), existingDocument.ProfessionalCertificateStatus),
                };

                foreach (var (file, setPath, status) in documentsToUpload)
                {
                    // Only proceed if file is not null and status is "Rejected"
                    if (file != null && status == "Rejected")
                    {
                        if (ValidateFile(file, MaxFileSize, allowedExtensions))
                        {
                            // Set the document status to "Unverified" for the specific document
                            if (file == model.IdProof)
                            {
                                existingDocument.IdProofStatus = "Unverified";
                                existingDocument.IdProofMessage = null;
                            }

                            else if (file == model.AddressProof) 
                            { 
                                existingDocument.AddressProofStatus = "Unverified";
                                existingDocument.AddressProofMessage = null;
                            }
                            else if (file == model.TenthCertificate)
                            {
                                existingDocument.TenthCertificateStatus = "Unverified";
                                existingDocument.TenthCertificateMessage = null;
                            }
                            else if (file == model.TwelveCertificate)
                            {
                                existingDocument.TwelveCertificateStatus = "Unverified";
                                existingDocument.TwelveCertificateMessage = null;
                            }

                            else if (file == model.ProfessionalCertificate) 
                            { 
                                existingDocument.ProfessionalCertificateStatus = "Unverified";
                                existingDocument.ProfessionalCertificateMessage = null;
                            }

                            var filePath = await SaveFileAsync(file);
                            setPath(filePath);
                        }
                        else
                        {
                            return $"Invalid File Extension or Size for {file.FileName}";
                        }
                    }
                }

                _context.Documents.Update(existingDocument);
                return await _context.SaveChangesAsync() > 0 ? "Successful" : "Internal server DB error";
            }
            catch (Exception ex)
            {
                return $"Internal server EX error = {ex.Message}";
            }
        }

      

        public LayoutDataViewModel LayoutData(int Id)
        {
            var record = _context.JyotishRecords.Where(x => x.Id == Id && x.Status == true && x.Role=="Pending").FirstOrDefault();
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
                    Image = record.ProfileImageUrl
                };
                return layoutData;
            }
        }


        public string RejectedMessage(int Id)
        {
            var record = _context.JyotishRecords.Where(x => x.Id == Id && x.Status == false && x.Role == "Rejected").FirstOrDefault();
            if(record == null)
            {
                return null;
            }
            else
            {
                return record.Message;
            }

        }






    }
}
