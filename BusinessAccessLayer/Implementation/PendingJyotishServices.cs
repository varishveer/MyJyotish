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




      /*  public async Task<bool> UploadDocumentAsync(DocumentViewModel model)
        {
            var isJyotishValid = _context.PendingJyotishRecords.Where(x=>x.Email == model.JyotishEmail).FirstOrDefault();
            if(isJyotishValid == null)
            { return false; }
            var document = new DocumentModel
            {
                JyotishId = isJyotishValid.Id
            };

            try
            {
                // Process IdProof file
                if (model.IdProof != null)
                {
                    var idProofGuid = Guid.NewGuid().ToString();
                    var SqlPath = "/PendingJyotish/Document/" + idProofGuid + model.IdProof.FileName;
                    var idProofPath = Path.Combine(_uploadDirectory,SqlPath );
                    using (var stream = new FileStream(idProofPath, FileMode.Create))
                    {
                        await model.IdProof.CopyToAsync(stream);
                    }
                    document.IdProof = "/"+SqlPath;
                }

                // Process AddressProof file
                if (model.AddressProof != null)
                {
                    var addressProofGuid = Guid.NewGuid().ToString();
                    var SqlPath = "/PendingJyotish/Document/" + addressProofGuid + model.AddressProof.FileName;
                    var addressProofPath = Path.Combine(_uploadDirectory, SqlPath);
                   
                    using (var stream = new FileStream(addressProofPath, FileMode.Create))
                    {
                        await model.AddressProof.CopyToAsync(stream);
                    }
                    document.AddressProof = "/" + SqlPath;
                }

                // Process TenthCertificate file
                if (model.TenthCertificate != null)
                {
                    var tenthCertificateGuid = Guid.NewGuid().ToString();
                    var SqlPath = "/PendingJyotish/Document/" + tenthCertificateGuid + model.TenthCertificate.FileName;
                    var tenthCertificatePath = Path.Combine(_uploadDirectory, SqlPath);

                  
                    using (var stream = new FileStream(tenthCertificatePath, FileMode.Create))
                    {
                        await model.TenthCertificate.CopyToAsync(stream);
                    }
                    document.TenthCertificate = "/" + SqlPath;
                }

                // Process TwelveCertificate file
                if (model.TwelveCertificate != null)
                {
                    var twelveCertificateGuid = Guid.NewGuid().ToString();
                    var SqlPath = "/PendingJyotish/Document/" + twelveCertificateGuid + model.TwelveCertificate.FileName;
                    var twelveCertificatePath = Path.Combine(_uploadDirectory, SqlPath);

                  
                    using (var stream = new FileStream(twelveCertificatePath, FileMode.Create))
                    {
                        await model.TwelveCertificate.CopyToAsync(stream);
                    }
                    document.TwelveCertificate = "/" + SqlPath;
                }

                // Process ProfessionalCertificate file
                if (model.ProfessionalCertificate != null)
                {
                    var professionalCertificateGuid = Guid.NewGuid().ToString();
                    var SqlPath = "/PendingJyotish/Document/" + professionalCertificateGuid + model.ProfessionalCertificate.FileName;
                    var professionalCertificatePath = Path.Combine(_uploadDirectory, SqlPath);


                   
                    using (var stream = new FileStream(professionalCertificatePath, FileMode.Create))
                    {
                        await model.ProfessionalCertificate.CopyToAsync(stream);
                    }
                    document.ProfessionalCertificate = "/" + SqlPath;
                }

                // Save document data to database

                var IsRecordExist = _context.Documents.Where(X => X.JyotishId == document.JyotishId).FirstOrDefault();
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
            var isEmailValid =  _context.PendingJyotishRecords.Where(x => x.Email == email).FirstOrDefault();
            if (isEmailValid == null) { return null; }
            var isDocumentAvailable =  _context.Documents.Where(x=>x.JyotishId == isEmailValid.Id).FirstOrDefault();
            if (isDocumentAvailable == null) { return null; }
            else {
                DocumentModel model = new DocumentModel() 
                {
                    IdProof = isDocumentAvailable.IdProof,
                    AddressProof = isDocumentAvailable.AddressProof,
                    TenthCertificate=isDocumentAvailable.TenthCertificate,
                    TwelveCertificate = isDocumentAvailable.TwelveCertificate,
                    ProfessionalCertificate =isDocumentAvailable.ProfessionalCertificate
                };    
                return model; 
            }
        }*/

       /* public async Task<PendingJyotishModel> Profile(string email)
        {
            if (email == null) { return null; }
            var result = await _context.PendingJyotishRecords.Where(x => x.Email == email).FirstOrDefaultAsync();
            if (result == null) { return null; }
            return result;
        }

        public bool UpdateProfile(PendingJyotishViewModel model, string? path)
        {
            if (model == null) return false;

            // Find the existing record
            var existingRecord = _context.PendingJyotishRecords
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

            _context.PendingJyotishRecords.Update(existingRecord);
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
       */

    }
}
