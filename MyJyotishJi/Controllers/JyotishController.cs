using BusinessAccessLayer.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ModelAccessLayer.Models;
using ModelAccessLayer.ViewModels;
using System.Net;

namespace MyJyotishGApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Policy2")]
    public class JyotishController : ControllerBase
    {
        private readonly IJyotishServices _jyotish;
        private readonly IWebHostEnvironment _environment;
        public JyotishController(IJyotishServices jyotish, IWebHostEnvironment environment)
        {
            _jyotish = jyotish;
            _environment = environment;
        }
        [HttpGet("GetAllAppointment")]
        public IActionResult GetAllAppointment(int Id)
        {
            var records = _jyotish.GetAllAppointment(Id);
            return Ok(new { Status =200,data = records });
        }
        [HttpGet("UpcomingAppointment")]
        public IActionResult UpcomingAppointment(int Id)
        {
            var records = _jyotish.UpcomingAppointment(Id);
            return Ok(new { data = records });
        }
        [HttpPost("AddAppointment")]
        public IActionResult AddAppointment(AddAppointmentJyotishModel model)
        {
            try
            {
                var result = _jyotish.AddAppointment(model);
                if (result == "invalid Data")
                {
                    return Ok(new { Status = 400, Message = result });
                }

                else if (result == "Successful") { return Ok(new { Status = 200, message = result }); }
             
                else { return Ok(new { Status = 500, message = result }); }
            }
            catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
        }

        [HttpGet("GetAppointment")]
        public IActionResult GetAppointment(int Id)
        {
            try
            {
                var result = _jyotish.GetAppointment(Id);
                if (result == null)
                {
                    return Ok(new { Status = 400, Message = "Appointment Not Found" });
                }
                else { return Ok(new { Status = 200, Data = result ,message = "Successful" }); }
            }
            catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
        }

        [HttpPost("UpdateAppointment")]
        public IActionResult UpdateAppointment(UpdateAppointmentJyotishViewModel model)
        {
            try
            {
                var result = _jyotish.UpdateAppointment(model);
                if (result == "invalid Data")
                {
                    return Ok(new { Status = 400, Message = result });
                }

                else if (result == "Successful") { return Ok(new { Status = 200, message = result }); }
                else if (result == "internal Server Error.") { return Ok(new { Status = 500, message = result }); }
                else { return Ok(new { Status = 500, message = result }); }
            }
            catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
        }


        
        [AllowAnonymous]
        [HttpGet("Country")]
        public IActionResult Country()
        {
            var Record = _jyotish.CountryList();
            return Ok(new { data = Record });
        }
        [AllowAnonymous]
        [HttpGet("State")]
        public IActionResult State(int Id)
        {
            var Record = _jyotish.StateList(Id);
            return Ok(new { data = Record });
        }
        [AllowAnonymous]
        [HttpGet("City")]
        public IActionResult City(int Id)
        {
            var Record = _jyotish.CityList(Id);
            return Ok(new { data = Record });
        }
        [AllowAnonymous]
        [HttpGet("Expertise")]
        public IActionResult GetExpertise()
        {
            try
            {
                var records = _jyotish.ExpertiseList();
                return Ok(new { Status = 200, Data = records });
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { Status = 500, Message = "An error occurred while fetching expertise.", Error = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("GetPoojaList")]
        public IActionResult GetPoojaList()
        {
            try
            {
                var records = _jyotish.GetPoojaList();
                return Ok(new { Status = 200, Data = records });
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { Status = 500, Message = "An error occurred while fetching pooja list.", Error = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("GetSpecializationList")]
        public IActionResult GetSpecializationList()
        {
            try
            {
                var records = _jyotish.GetSpecializationList();
                return Ok(new { Status = 200, Data = records });
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { Status = 500, Message = "An error occurred while fetching specialization list.", Error = ex.Message });
            }
        }



        [HttpGet("Dashboard")]
        public IActionResult DashBoard(string email)
        {
            try
            {
                var records = _jyotish.DashBoard(email);
                if (records == null) { return BadRequest(); }
                return Ok(new { data = records });
            }
            catch { return BadRequest(); }
        }
       
        [HttpPost("UpdateProfile")]
        public IActionResult UpdateProfile(JyotishUpdateViewModel model)
        {
            try
            {
                var Result = _jyotish.UpdateProfile(model);
                if (Result == "Jyotish Not Found") { return Ok(new { Status = 404, Message = Result }); }
                else if (Result == "Invalid Email") { return Ok(new { Status = 404, Message = Result }); }
                else if (Result == "Successful") { return Ok(new { Status = 200, Message = Result }); }
                else { return Ok(new { Status = 500, Message = Result }); }


            }
            catch (Exception ex) {
                return StatusCode(500, new { Status = 500, Message = ex.Message, Error = ex });
            }
        }



        [HttpPost("AddJyotishVideo")]
        public IActionResult AddJyotishVideo( JyotishVideosViewModel model)
        {
            try
            {
                var result = _jyotish.AddJyotishVideo(model);
                if (result == "invalid data") return BadRequest(new { Status = 400, Message = result });
                else if (result == "Successful") return Ok(new { Status = 200, Message = result });
                else return StatusCode(500, new { Status = 500, Message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 500, Message = ex.Message, Error = ex });
            }
        }

        // Method to add Jyotish gallery
        [HttpPost("AddJyotishGallery")]
        public IActionResult AddJyotishGallery( JyotishGalleryViewModel model)
        {
            try
            {
                var result = _jyotish.AddJyotishGallery(model);
                if (result == "invalid data") return BadRequest(new { Status = 400, Message = result });
                else if (result == "Successful") return Ok(new { Status = 200, Message = result });
                else return StatusCode(500, new { Status = 500, Message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 500, Message = ex.Message, Error = ex });
            }
        }

        // Method to fetch Jyotish videos by Jyotish ID
        [HttpGet("JyotishVideos")]
        public IActionResult GetJyotishVideos(int id)
        {
            try
            {
                var videos = _jyotish.JyotishVideos(id);
                return Ok(new { Status = 200, Data = videos });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 500, Message = ex.Message, Error = ex });
            }
        }

        // Method to fetch Jyotish gallery images by Jyotish ID
        [HttpGet("JyotishGallery")]
        public IActionResult GetJyotishGallery(int id)
        {
            try
            {
                var gallery = _jyotish.JyotishGallery(id);
                return Ok(new { Status = 200, Data = gallery });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 500, Message = ex.Message, Error = ex });
            }
        }

        [HttpGet("GetProfile")]
        public IActionResult GetProfile(int Id)
        {
            try
            {
                var record = _jyotish.GetProfile(Id);
                return Ok(new { Status = 200, Data = record });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 500, Message = ex.Message, Error = ex });
            }
        }

        [HttpGet("GetAllSubscription")]
        public IActionResult GetAllSubscription()
        {
            try
            {
                var Result = _jyotish.GetAllSubscription();
                if (Result == null)
                { return Ok(new { Status = 404, Message = "Data Not Found" }); }

                else
                { return Ok(new { Status = 200, Data = Result, Message = "Successful" }); }


            }

            catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error ", Error = ex }); }
        }
        [HttpPost("AddTeamMember")]
        public IActionResult AddTeamMember(TeamMemberViewModel team)
        {

            try
            {
                string? path = _environment.ContentRootPath;
                var records = _jyotish.AddTeamMember(team, path);
                return Ok(new { status = 200, message = records });


            }

            catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error ", Error = ex }); }

        }

        [HttpGet("TeamMember")]
        public IActionResult TeamMember(int Id)
        {


            try
            {
                var records = _jyotish.TeamMember(Id);
                return Ok(new { status = 200, data =records });

            }

            catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error ", Error = ex }); }
           
        }

        [HttpGet("JyotishPaymentrecords")]
        public IActionResult JyotishPaymentrecords(int Id)
        {
            try
            {
                var Result = _jyotish.JyotishPaymentrecords(Id);
                if (Result == null)
                { return Ok(new { Status = 404, Message = "Data Not Found" }); }

                else
                { return Ok(new { Status = 200, Data = Result, Message = "Successful" }); }
            }
            catch (Exception ex)
            { return StatusCode(500, new { Status = 500, Message = "Internal Server Error ", Error = ex }); }
        }
        [HttpPost("AddWallet")]
        public IActionResult AddWallet(JyotishWalletViewmodel jw)
        {
            try
            {
                var Result = _jyotish.AddWallet(jw);
                if (Result == "Successful")
                {
                    JyotishWalletHistoryViewmodel js = new JyotishWalletHistoryViewmodel
                    {
                        JId = jw.jyotishId,
                        amount=jw.WalletAmount,
                        PaymentAction="Credit",
                        PaymentStatus="Success"
                        
                    };
                   var res= AddWalletHistory(js);
                    return Ok(new { Status = 200, Message = "Successful" }); 
                }
                else
                { return Ok(new { Status = 404, Message ="Some error occured" }); }
            }
            catch (Exception ex)
            { return StatusCode(500, new { Status = 500, Message = "Internal Server Error ", Error = ex }); }
        }
        [HttpGet("GetWallet")]
        public IActionResult GetWallet(int JyotishId)
        {
            try
            {
                var Result = _jyotish.GetWallet(JyotishId);
                if (Result !=null)
                { return Ok(new { Status = 200, Data = Result}); }
                else
                { return Ok(new { Status = 404, Message ="Some error occured" }); }
            }
            catch (Exception ex)
            { return StatusCode(500, new { Status = 500, Message = "Internal Server Error ", Error = ex }); }
        }

        [HttpPost("AddWalletHistory")]
        public IActionResult AddWalletHistory(JyotishWalletHistoryViewmodel jw)
        {
            try
            {
                var Result = _jyotish.AddWalletHistory(jw);
                if (Result == "Successful")
                { return Ok(new { Status = 200, Message = "Successful" }); }
                else
                { return Ok(new { Status = 404, Message = "Some error occured" }); }
            }
            catch (Exception ex)
            { return StatusCode(500, new { Status = 500, Message = "Internal Server Error ", Error = ex }); }
        }

        [HttpGet("GetWalletHistroy")]
        public IActionResult GetWallethistory(int JyotishId)
        {
            try
            {
                var Result = _jyotish.GetWalletHistory(JyotishId);
                if (Result != null)
                { return Ok(new { Status = 200, Data = Result }); }
                else
                { return Ok(new { Status = 404, Message = "Some error occured" }); }
            }
            catch (Exception ex)
            { return StatusCode(500, new { Status = 500, Message = "Internal Server Error ", Error = ex }); }
        }

        [HttpGet("JyotishPaymentDetail")]
        public IActionResult JyotishPaymentDetail(int Id)
        {
            try
            {
                var Result = _jyotish.JyotishPaymentDetail(Id);
                if (Result == null)
                { return Ok(new { Status = 404, Message = "Data Not Found" }); }

                else
                { return Ok(new { Status = 200, Data = Result, Message = "Successful" }); }
            }
            catch (Exception ex)
            { return StatusCode(500, new { Status = 500, Message = "Internal Server Error ", Error = ex }); }
        }
        [HttpPost("AddAppointmentSlot")]
        public IActionResult AddAppointmentSlot(AppointmentSlotViewModel model)
        {
            try
            {
                var Result = _jyotish.AddAppointmentSlot(model);
                if (Result == "Invalid Date")
                { return Ok(new { Status = 400, Message = Result }); }

                else if(Result == "Invalid Jyotish")
                { return Ok(new { Status = 409,  Message =Result }); }
                else if (Result == "Invalid Data")
                { return Ok(new { Status = 409, Message = Result }); }

                else if (Result == "Data Not Saved")
                { return Ok(new { Status = 500, Message = Result }); }

                else if (Result == "Successful")
                { return Ok(new { Status = 200, Message = Result }); }
                else 
                { return Ok(new { Status = 500, Message = Result }); }
            }
            catch (Exception ex)
            { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
        }
        
        [HttpPost("UpdateAppointmentSlot")]
        public IActionResult UpdateAppointmentSlot(AppointmentSlotViewModel model)
        {
            try
            {
                var Result = _jyotish.UpdateAppointmentSlot(model);
                if (Result == "Invalid Date")
                { return Ok(new { Status = 400, Message = Result }); }

                else if (Result == "Invalid Jyotish")
                { return Ok(new { Status = 409, Message = Result }); }
                else if (Result == "Invalid Data")
                { return Ok(new { Status = 409, Message = Result }); }
                else if (Result == "Data Not Saved")
                { return Ok(new { Status = 500, Message = Result }); }
                else if (Result == "Successful")
                { return Ok(new { Status = 200, Message = Result }); }
                else if(Result == "Slot Has Been Booked It Can't Be Updated.")
                { return Ok(new { Status = 500, Message = Result }); }
                else
                { return Ok(new { Status = 500, Message = Result }); }
            }
            catch (Exception ex)
            { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
        }
        [HttpDelete("DeleteAppointmentSlot")]
        public IActionResult DeleteAppointmentSlot(int Id)
        {
            try
            {
                var Result = _jyotish.DeleteAppointmentSlot(Id);
                if (Result == "Invalid Id")
                { return Ok(new { Status = 400, Message = Result }); }

                else if (Result == "Internal Server Error.")
                { return Ok(new { Status = 500, Message = Result }); }
                else if (Result == "Successful")
                { return Ok(new { Status = 200, Message = Result }); }

                else
                { return Ok(new { Status = 500, Message = Result }); }
            }
            catch (Exception ex)
            { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
        }

        [HttpGet("GetAllAppointmentSlot")]
        public IActionResult GetAllAppointmentSlot(int Id)
        {
            try
            {
                var Result = _jyotish.GetAllAppointmentSlot(Id);
                if (Result.Count==0)
                { return Ok(new { Status = 400, Message = "Data Not Found" }); }


                else
                { return Ok(new { Status = 200,data = Result, Message = "Successful" }); }
            }
            catch (Exception ex)
            { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
        }


        [HttpPost("AddProblemSolution")]
        public IActionResult AddProblemSolution(ProblemSolutionViewModel model)
        {
            try
            {
                var Result = _jyotish.AddProblemSolution(model);
                if (Result == "No data provided")
                { return Ok(new { Status = 400, Message = Result }); }

                else if (Result == "Invalid Data: AppointmentId does not exist.")
                { return Ok(new { Status = 400, Message = Result }); }

                else if (Result == "Successful")
                { return Ok(new { Status = 200, Message = Result }); }
                else
                { return Ok(new { Status = 500, Message = "Internal Server Error", Error = Result }); }
            }
            catch (Exception ex)
            { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
        }

        [HttpGet("GetProblemSolution")]
        public IActionResult GetProblemSolution(int id)
        {
            try
            {
               
                var result = _jyotish.GetProblemSolution(id);

               
                if (result == null )
                {
                    return Ok(new { Status = 400, Message = "Data Not Found" });
                }

               
                return Ok(new { Status = 200, Data = result, Message = "Successful" });
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex.Message });
            }
        }


        [HttpPost("UpdateProblemSolution")]
        public IActionResult UpdateProblemSolution(ProblemSolutionViewModel model)
        {
            try
            {
                // Call the service layer to update the problem solution
                var result = _jyotish.UpdateProblemSolution(model);

                // Determine response based on the result
                switch (result)
                {
                    case "Invalid data provided":
                        return BadRequest(new { Status = 400, Message = result });

                    case "Record not found":
                        return NotFound(new { Status = 404, Message = result });

                    case "Update failed":
                        return StatusCode(500, new { Status = 500, Message = result });

                    case "successful":
                        return Ok(new { Status = 200, Message = result });

                    default:
                        return StatusCode(500, new { Status = 500, Message = "Unexpected error occurred" });
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions with a 500 status code
                return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex.Message });
            }
        }

        [HttpDelete("DeleteProblemSolution")]
        public IActionResult DeleteProblemSolution(int id)
        {
            try
            {
                var result = _jyotish.DeleteProblemSolution(id); // Ensure the method name matches

                switch (result)
                {
                    case "Invalid Id":
                        return BadRequest(new { Status = 400, Message = result });

                    case "Internal Server Error.":
                        return StatusCode(500, new { Status = 500, Message = result });

                    case "Successful":
                        return Ok(new { Status = 200, Message = result });

                    default:
                        return StatusCode(500, new { Status = 500, Message = "Unexpected error occurred" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex.Message });
            }
        }

        [HttpPost("AddUserAttachment")]
        public IActionResult AddUserAttachment()
        {
            try
            {
                var httpRequest = HttpContext.Request;

                // Collect Titles
                List<string> titleList = httpRequest.Form["Title"].ToList();

                // Collect Image URLs
                List<string> urlList = new List<string>();
                var images = httpRequest.Form.Files;

                // Ensure directory exists for file upload
                string uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Jyotish/ProblemSolution");
                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }

                // Process each uploaded file
                foreach (var image in images)
                {
                    string uniqueString = Guid.NewGuid().ToString("N").Substring(0, 10);

                    string filePath = Path.Combine(uploadsFolderPath,uniqueString+ image.FileName);
                    string accessPath = Path.Combine("/Images/Jyotish/ProblemSolution", uniqueString + image.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        image.CopyTo(stream);
                    }
                    urlList.Add(accessPath);
                }

                // Prepare ViewModel
                JyotishUserAttachmentViewModel model = new JyotishUserAttachmentViewModel
                {
                    UserId = int.Parse(httpRequest.Form["UserId"]),
                    JyotishId = int.Parse(httpRequest.Form["JyotishId"]),
                    Title = titleList,
                    ImageUrl = urlList
                };

                // Save to Database
                var result = _jyotish.AddUserAttachment(model);
                if (result == "Invalid data provided for the attachment.")
                {
                    return Ok(new { Status = 400, Message = result });
                }
                else if(result == "Failed to save attachments.")
                {
                    return Ok(new { Status = 500, Message = result });
                }
                else if( result == "Successful")
                {
                    return Ok(new { Status = 200, Message = result });
                }
                return Ok(new { Status = 500, Message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex.Message });
            }
        }


        [HttpGet("GetAllUserAttachments")]
        public IActionResult GetAllUserAttachments(int Id)
        {
            try
            {
                var result = _jyotish.GetAllUserAttachments(Id);

                if (result == null || result.Count == 0)
                {
                    return Ok(new { Status = 404, Message = "No attachments found for the provided JyotishId." });
                }

                return Ok(new { Status = 200, Data = result, Message = "Successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex.Message });
            }
        }
        [HttpPost("UpdateUserAttachment")]
        public IActionResult UpdateUserAttachment([FromForm] JyotishUserAttachmentJyotishUpdateViewModel model)
        {
            try
            {
                // Validate model data
                if (model == null || model.Id <= 0 || string.IsNullOrEmpty(model.Title))
                {
                    return BadRequest(new { Status = 400, Message = "Invalid data provided" });
                }

                // Call service method to update the attachment
                var result = _jyotish.UpdateUserAttachment(model);

                // Check the result of the update operation
                if (result == "Successful")
                {
                    return Ok(new { Status = 200, Message = result });
                }
                else if (result == "Record not found")
                {
                    return NotFound(new { Status = 404, Message = result });
                }
                else
                {
                    return StatusCode(500, new { Status = 500, Message = result });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex.Message });
            }
        }


        [HttpDelete("DeleteUserAttachment")]
        public IActionResult DeleteUserAttachment(int id)
        {
            try
            {
                var result = _jyotish.DeleteUserAttachment(id);

                if (result == "Invalid Id.")
                {
                    return BadRequest(new { Status = 400, Message = result });
                }
                else if (result == "Attachment not found.")
                {
                    return NotFound(new { Status = 404, Message = result });
                }
                else if (result == "Successful")
                {
                    return Ok(new { Status = 200, Message = result });
                }
                else
                {
                    return StatusCode(500, new { Status = 500, Message = result });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex.Message });
            }
        }
    }
}
