using BusinessAccessLayer.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ModelAccessLayer.Models;
using ModelAccessLayer.ViewModels;

namespace MyJyotishGApi.Controllers
{
   
    [Route("api/[controller]")]
    [Authorize(Policy = "Policy3")]
    [ApiController]
    public class PendingJyotishController : ControllerBase
    {
        private readonly IPendingJyotishServices _pendingJyotishServices;
        private readonly IWebHostEnvironment _webHostEnvironment; 
        public PendingJyotishController(IPendingJyotishServices pendingJyotishServices, IWebHostEnvironment webHostEnvironment)
        {
            _pendingJyotishServices = pendingJyotishServices;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("Dashboard")]
        public IActionResult Dashboard(string Email) 
        {
            var result = _pendingJyotishServices.Role(Email);
            if(result == null)
            {
                result = "Jyotish Not Found";
            }
            return Ok(new { Status = 200 , data  = result , Message = "Successful"});
        }


        [HttpGet("Documents")]
        public IActionResult Documents(string Email)
        {
            try {
                DocumentModel Records = _pendingJyotishServices.Documents(Email);
                if(Records == null)
                { return Ok(new { Status = 409, Message = "Not Found" }); }
                else { return Ok(new { Status= 200, data = Records }); }
                
            }
            catch { return StatusCode(500, new { Message = "Internal Server Error" }); }
            
        }
     
        [HttpPost("UploadDocument")]
        public async Task<IActionResult> UploadDocument(DocumentViewModel model)
        {
            try
            {
                var success = await _pendingJyotishServices.UploadDocumentAsync(model);

                if (success)
                {
                    return Ok(new { Status = 200, Message = "Files and data uploaded successfully." });
                }
                else
                {
                    return StatusCode(500, new { Message = "Internal Server Error" });
                }
            }
            catch { return StatusCode(500, new { Message = "Internal Server Error" }); }
        }

        [HttpGet("Profile")]
        public async Task<IActionResult> Profile(string Email)
        {
            var result = await _pendingJyotishServices.Profile(Email);
            if(result == null) { return Ok(new {Status =400 , Message ="No User Found"}); }
            else { return Ok(new { Status =200, data = result }); }
        }

        [HttpPost("UpdateProfile")]
        public IActionResult UpdateProfile(JyotishViewModel model)
        {
            try
            {
                string path = _webHostEnvironment.ContentRootPath;
                var result = _pendingJyotishServices.UpdateProfile(model, path);
                if (result == "Successful")
                { return Ok(new { Status = 200, Message = "Successful" }); }
                if (result == "Invalid Data")
                { return Ok(new { Status = 400, Message = result }); }
                if (result == "Jyotish Not Found")
                { return Ok(new { Status = 400, Message = result }); }
                else { return Ok(new { Status = 400, Message = result }); }
            }
            catch
            {
                return StatusCode(500,new { Status = 500, Message = "Internal Server Error" });
            }

        }

       /* [AllowAnonymous]
        [HttpGet("GetProfileImage")]
        public IActionResult GetProfileImage(string fileName)
        {
            // Construct the path to the image file
            string path = Path.Combine(_webHostEnvironment.ContentRootPath, "Assets", "Images", "Jyotish", fileName);

            // Check if the file exists
            if (!System.IO.File.Exists(path))
            {
                return NotFound(); // Return 404 if the file doesn't exist
            }
             
            // Determine the content type of the file
            var contentType = "image/jpeg"; // Change this if you're serving different types of images

            // Return the file as a response with the appropriate content type
            return File(System.IO.File.ReadAllBytes(path), contentType);
        }*/

        [HttpGet("ProfileImage")]
        public IActionResult GetProfileImage(string Email)
        {
            try
            {
                var Image = _pendingJyotishServices.ProfileImage(Email);
                if(Image == null)
                { return Ok(new { Status = 200, data = "Images/Placeholder/ProfileImage.jpg", Message = "PlaceHolderImage" }); }
                else { return Ok(new {Status = 200, data = Image, Message= "Jyotish Image" }); }
            }
            catch
            {
                return StatusCode(500, new { Message = "Internal Server Error" });
            }

        }
      /*  [AllowAnonymous]
        [HttpPost("AddSlotBooking")]
        public IActionResult AddSlotBooking(SlotBookingViewModel model)
        {
            try
            {
                var Result = _pendingJyotishServices.AddSlotBooking(model);
                if (Result == "Successful")
                {
                    return Ok(new { status = 200, message = Result });
                }
                else if (Result == "Jyotish Not Found")
                { return Ok(new { status = 400, message = Result }); }
                else if(Result == "Already Booked")
                { return Ok(new { status = 409, message = Result }); }
                else
                {
                    return Ok(new { Status = 500, Message = Result });
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = 500,
                    Message = "Internal Server Error ",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,       // Stack trace to determine the point of failure
                    Source = ex.Source,               // The name of the object or application that caused the error
                    TargetSite = ex.TargetSite?.Name, // Method or property name where the error occurred
                    InnerException = ex.InnerException?.Message,  // The message of the inner exception, if any
                    HResult = ex.HResult,             // Numerical value associated with the error
                    AdditionalData = ex.Data
                });
            }
        }
        [AllowAnonymous]
        [HttpGet("SlotList")]
        public IActionResult SlotList()
        {
            try
            {
                var Records = _pendingJyotishServices.SlotList();
                if (Records == null)
                {
                    return Ok(new { Status = 400, Message = "Invalid Data" });
                }
                else
                {
                    return Ok(new { Status = 200, data = Records, Message = "Successful" });
                }

            }
            catch(Exception ex) { return StatusCode(500, new { 
                Status = 500, 
                Message = "Internal Server Error, No Data Found",
                ErrorMessage = ex.Message,
                StackTrace = ex.StackTrace,       // Stack trace to determine the point of failure
                Source = ex.Source,               // The name of the object or application that caused the error
                TargetSite = ex.TargetSite?.Name, // Method or property name where the error occurred
                InnerException = ex.InnerException?.Message,  // The message of the inner exception, if any
                HResult = ex.HResult,             // Numerical value associated with the error
                AdditionalData = ex.Data
            }); }
        }*/

    }
}
