using BusinessAccessLayer.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelAccessLayer.Models;
using ModelAccessLayer.ViewModels;

namespace MyJyotishGApi.Controllers
{
    [Authorize(Policy = "Policy3")]
    [Route("api/[controller]")]
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
                { return BadRequest(); }
                else { return Ok(new { data = Records }); }
                
            }
            catch { return BadRequest(); }
            
        }
       [AllowAnonymous]
        [HttpPost("UploadDocument")]
        public async Task<IActionResult> UploadDocument(DocumentViewModel model)
        {
            var success = await _pendingJyotishServices.UploadDocumentAsync(model);

            if (success)
            {
                return Ok(new { Message = "Files and data uploaded successfully." });
            }
            else
            {
                return StatusCode(500, "An error occurred while processing files.");
            }
        }

        [HttpGet("Profile")]
        public async Task<IActionResult> Profile(string Email)
        {
            var result = await _pendingJyotishServices.Profile(Email);
            if(result == null) { return BadRequest(); }
            else { return Ok(new { data = result }); }
        }

        [HttpPost("UpdateProfile")]
        public IActionResult UpdateProfile(JyotishViewModel model)
        {
            string path = _webHostEnvironment.ContentRootPath;
            var result = _pendingJyotishServices.UpdateProfile(model,path);

            
            
            if (result == true)
            { return Ok(); }
            else { return BadRequest(); }
        }

        [AllowAnonymous]
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
        }


    }
}
