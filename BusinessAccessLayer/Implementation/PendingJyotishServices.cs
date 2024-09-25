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


       
        public async Task<bool> UploadDocumentAsync(DocumentViewModel model)
        {
            var isJyotishValid = _context.JyotishRecords.Where(x=>x.Email == model.JyotishEmail).FirstOrDefault();
            if(isJyotishValid == null)
            { return false; }
            
            var IsJyotishDocs = _context.Documents.Where(x=>x.JId == isJyotishValid.Id).FirstOrDefault();
            var document = new DocumentModel();
            if (IsJyotishDocs == null) {


                document.JId = isJyotishValid.Id;
               
            }
            else
            {
              
                document = IsJyotishDocs;
            }


            try
            {
                // Process IdProof file
                if (model.IdProof != null)
                {
                    var idProofGuid = Guid.NewGuid().ToString();
                    var SqlPath = "wwwroot/PendingJyotish/Document/" + idProofGuid + model.IdProof.FileName;
                    var idProofPath = Path.Combine(_uploadDirectory,SqlPath );
                    using (var stream = new FileStream(idProofPath, FileMode.Create))
                    {
                        await model.IdProof.CopyToAsync(stream);
                    }
                    document.IdProof = "/PendingJyotish/Document/" + idProofGuid + model.IdProof.FileName;
                    document.IdProofStatus = "Unverified";
                }
               

                // Process AddressProof file
                if (model.AddressProof != null)
                {
                    var addressProofGuid = Guid.NewGuid().ToString();
                    var SqlPath = "wwwroot/PendingJyotish/Document/" + addressProofGuid + model.AddressProof.FileName;
                    var addressProofPath = Path.Combine(_uploadDirectory, SqlPath);
                   
                    using (var stream = new FileStream(addressProofPath, FileMode.Create))
                    {
                        await model.AddressProof.CopyToAsync(stream);
                    }
                    document.AddressProof = "/PendingJyotish/Document/" + addressProofGuid + model.AddressProof.FileName;
                    document.AddressProofStatus = "Unverified";
                }

                // Process TenthCertificate file
                if (model.TenthCertificate != null)
                {
                    var tenthCertificateGuid = Guid.NewGuid().ToString();
                    var SqlPath = "wwwroot/PendingJyotish/Document/" + tenthCertificateGuid + model.TenthCertificate.FileName;
                    var tenthCertificatePath = Path.Combine(_uploadDirectory, SqlPath);

                  
                    using (var stream = new FileStream(tenthCertificatePath, FileMode.Create))
                    {
                        await model.TenthCertificate.CopyToAsync(stream);
                    }
                    document.TenthCertificate = "/PendingJyotish/Document/" + tenthCertificateGuid + model.TenthCertificate.FileName;
                    document.AddressProofStatus = "Unverified";
                }

                // Process TwelveCertificate file
                if (model.TwelveCertificate != null)
                {
                    var twelveCertificateGuid = Guid.NewGuid().ToString();
                    var SqlPath = "wwwroot/PendingJyotish/Document/" + twelveCertificateGuid + model.TwelveCertificate.FileName;
                    var twelveCertificatePath = Path.Combine(_uploadDirectory, SqlPath);

                  
                    using (var stream = new FileStream(twelveCertificatePath, FileMode.Create))
                    {
                        await model.TwelveCertificate.CopyToAsync(stream);
                    }
                    document.TwelveCertificate = "/PendingJyotish/Document/" + twelveCertificateGuid + model.TwelveCertificate.FileName;
                    document.TwelveCertificateStatus = "Unverified";
                }

                // Process ProfessionalCertificate file
                if (model.ProfessionalCertificate != null)
                {
                    var professionalCertificateGuid = Guid.NewGuid().ToString();
                    var SqlPath = "wwwroot/PendingJyotish/Document/" + professionalCertificateGuid + model.ProfessionalCertificate.FileName;
                    var professionalCertificatePath = Path.Combine(_uploadDirectory, SqlPath);


                   
                    using (var stream = new FileStream(professionalCertificatePath, FileMode.Create))
                    {
                        await model.ProfessionalCertificate.CopyToAsync(stream);
                    }
                    document.ProfessionalCertificate = "/PendingJyotish/Document/" + professionalCertificateGuid + model.ProfessionalCertificate.FileName;
                    document.ProfessionalCertificateStatus = "Unverified";
                }

                // Save document data to database

                var IsRecordExist = _context.Documents.Where(X => X.JId == document.JId).FirstOrDefault();
                if (IsRecordExist == null)
                { _context.Documents.Add(document); }
                else
                { _context.Documents.Update(document); }
                
                 var result = await _context.SaveChangesAsync();
                if(result> 0)
                { return true; }
                else { return false; }
                 
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false; // Failure in processing files or saving to the database
            }
        }

        public DocumentModel Documents(string email)
        {
            var isEmailValid =  _context.JyotishRecords.Where(x => x.Email == email).FirstOrDefault();
            if (isEmailValid == null) { return null; }
            var isDocumentAvailable =  _context.Documents.Where(x=>x.JId == isEmailValid.Id).FirstOrDefault();
            if (isDocumentAvailable == null) { return null; }
            else {
                isDocumentAvailable.Jyotish = null;
                return isDocumentAvailable; 
            }
        }

        public async Task<JyotishModel> Profile(string email)
        {
            if (email == null) { return null; }
            var result = await _context.JyotishRecords.Where(x => x.Email == email).FirstOrDefaultAsync();
            if (result == null) { return null; }
            return result;
        }

        public bool UpdateProfile(JyotishViewModel model, string? path)
        {
            if (model == null) return false;

            // Find the existing record
            var existingRecord = _context.JyotishRecords
                .FirstOrDefault(x => x.Email == model.Email);

            if (existingRecord == null) return false;

            // Fetch related entities only if necessary
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

            // Handle file upload
            string filePath = string.Empty;
            if (model.Image != null)
            {
                // Generate a unique file name
                var uniqueFileName = $"{Guid.NewGuid()}_{model.Image.FileName}";
                filePath = $"/Images/Jyotish/{uniqueFileName}";

                var fullPath = path + "/"+filePath;
                UploadFile(model.Image, fullPath);
            }

            // Update existing record
            existingRecord.Name = model.Name;
            existingRecord.Gender = model.Gender;
            existingRecord.Language = model.Language;
            existingRecord.Expertise = model.Expertise;
            existingRecord.DateOfBirth = model.DateOfBirth;
            existingRecord.Mobile = model.Mobile;
            existingRecord.Country = countryName;
            existingRecord.State = stateName;
            existingRecord.City = cityName;
            existingRecord.Role = "Jyotish";
           
            existingRecord.Status = "Pending";
            existingRecord.ProfileImageUrl = filePath;

            _context.JyotishRecords.Update(existingRecord);
            if(_context.SaveChanges() > 0)
            {
                return true;
            }
            // Save changes
            return false;
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
    }
}
