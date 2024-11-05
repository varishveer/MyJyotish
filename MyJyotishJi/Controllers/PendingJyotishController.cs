using BusinessAccessLayer.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ModelAccessLayer.Models;
using ModelAccessLayer.ViewModels;
using NuGet.Protocol.Plugins;

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
        public IActionResult Documents(int Id)
        {
            try {
                DocumentModel Records = _pendingJyotishServices.Documents(Id);
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
                var result = await _pendingJyotishServices.UploadDocumentAsync(model);

                if (result == "Invalid Jyotish")
                {
                    return Ok(new { Status = 404, Message = result });
                }
                else if (result == "Invalid Data")
                {
                    return Ok(new { Status = 404, Message = result });
                }
                else if (result == "Invalid File Extension or Size")
                {
                    return Ok( new {Status = 400, Message = result });
                }
                else if(result == "Successful")
                {
                    return Ok(new { Status = 200, Message = "Files and data uploaded successfully." });
                }
                else
                {
                    return Ok(new { Status = 500, Message = result });
                }
            }
            catch { return StatusCode(500, new { Message = "Internal Server Error" }); }
        }

        [HttpGet("ProfileData")]
        public async Task<IActionResult> ProfileData(int Id)
        {
            var result = await _pendingJyotishServices.ProfileData(Id);
            if(result == null) { return Ok(new {Status =400 , Message ="No User Found"}); }
            else { return Ok(new { Status =200, data = result }); }
        }

        [HttpGet("ProfileTempData")]
        public async Task<IActionResult> ProfileTempData(int Id)
        {
            var result = await _pendingJyotishServices.ProfileTempData(Id);
            if (result == null) { return Ok(new { Status = 400, Message = "No User Found" }); }
            else { return Ok(new { Status = 200, data = result }); }
        }

        [HttpPost("UpdateProfile")]
        public IActionResult UpdateProfile(JyotishTempViewModel model)
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
                else if(Result == "Your Slot Already Booked")
                { return Ok(new { status = 409, message = Result }); }
                else if (Result == "This Slot Already Booked")
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
                    ErrorMessage = ex
                  
                });
            }
        }
        
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
                ErrorMessage = ex
            }); }
        }
       
        [HttpGet("JyotishSlotDetails")]
        public IActionResult JyotishSlotDetails(int Id)
        {
            try
            {
                var Records = _pendingJyotishServices.JyotishSlotDetails(Id);
                if (Records == null)
                {
                    return Ok(new { Status = 400, Message = "No Data Found" });
                }
                else
                {
                    return Ok(new { Status = 200, data = Records, Message = "Successful" });
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = 500,
                    Message = "Internal Server Error, No Data Found",
                    ErrorMessage = ex
                });
            }
        }

    }
}
