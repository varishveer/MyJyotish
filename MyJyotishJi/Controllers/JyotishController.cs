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
        [HttpGet("Appointment")]
        public IActionResult Appointment(string JyotishEmail)
        {
            var records = _jyotish.Appointment(JyotishEmail);
            return Ok(new { data = records });
        }
        [HttpGet("UpcomingAppointment")]
        public IActionResult UpcomingAppointment(string JyotishEmail)
        {
            var records = _jyotish.UpcomingAppointment(JyotishEmail);
            return Ok(new { data = records });
        }
        [HttpPost("AddAppointment")]
        public IActionResult AddAppointment(AppointmentViewModel model)
        {
            try
            {
                var result = _jyotish.AddAppointment(model);
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
        [HttpPost("AddTeamMember")]
        public IActionResult AddTeamMember()
        {
            var name = Request.Form["name"];
            var mobile = Request.Form["mobile"];
            var email = Request.Form["email"];
            var jyotishEmail = Request.Form["jyotishEmail"];
            var profilePicture = Request.Form.Files["profilePicture"];

            TeamMemberViewModel team = new TeamMemberViewModel()
            {
                Name = name,
                Mobile = mobile,
                Email = email,
                JyotishEmail = jyotishEmail,
                ProfilePicture = profilePicture
            };

            string? path = _environment.ContentRootPath;
            var records = _jyotish.AddTeamMember(team, path);
            return Ok(new { data = records });
        }

        [HttpGet("TeamMember")]
        public IActionResult TeamMember(string JyotishEmail)
        {
            var records = _jyotish.TeamMember(JyotishEmail);
            return Ok(records);
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
        public IActionResult UpdateProfile( JyotishCompleteViewModel model)
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
        public IActionResult AddJyotishVideo([FromBody] JyotishVideosViewModel model)
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
        public IActionResult AddJyotishGallery([FromForm] JyotishGalleryViewModel model)
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
        [HttpGet("JyotishVideos/{id}")]
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
        [HttpGet("JyotishGallery/{id}")]
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



    }
}
