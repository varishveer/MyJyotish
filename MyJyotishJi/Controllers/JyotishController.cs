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
        public JyotishController( IJyotishServices jyotish, IWebHostEnvironment environment)
        {
            _jyotish = jyotish;
            _environment = environment;
        }
        [HttpGet("Appointment")]
        public IActionResult Appointment(string JyotishEmail) 
        {
            var records = _jyotish.Appointment( JyotishEmail);
            return Ok(new { data = records });        
        }
        [HttpGet("UpcomingAppointment")]
        public IActionResult UpcomingAppointment(string JyotishEmail)
        {
            var records = _jyotish.UpcomingAppointment( JyotishEmail);
            return Ok(new { data = records });
        }
        [HttpPost("AddAppointment")]
        public IActionResult AddAppointment(AppointmentViewModel appointment)
        {
            var result =_jyotish.AddAppointment(appointment);
            return Ok(new { data = result });
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
            return Ok( records );
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
        public IActionResult Expertise()
        {
            var Records = _jyotish.ExpertiseList();
            return Ok(new { data = Records });
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
        

    }
}
