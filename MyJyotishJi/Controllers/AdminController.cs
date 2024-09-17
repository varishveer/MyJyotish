using BusinessAccessLayer.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ModelAccessLayer.Models;
using ModelAccessLayer.ViewModels;

namespace MyJyotishJiApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Policy1")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminServices _admin;
        public AdminController(IAdminServices admin)
        {
            _admin = admin;
        }
        [HttpGet("Profile")]
        public IActionResult Profile([FromQuery] string email)
        {
            var record = _admin.Profile(email);
            if (record == null)
            {
                return BadRequest();
            }
            else
            { return Ok(new { Success = true, data = record }); }

        }
       /* [HttpGet("Dashboard")]
        public IActionResult Dashboard()
        {
            var record = _admin.Dashboard();
            if (record == null) { return BadRequest(); }
            else { return Ok(new { success = true, data = record }); }
        }*/

        [HttpGet("Jyotish")]
        public IActionResult AllJyotishRecord()
        {
            var Records = _admin.GetAllJyotish();
            return Ok(new { Success = true, data = Records });
        }
        [HttpGet("PendingJyotish")]
        public IActionResult AllPendingJyotishRecord()
        {
            var Records = _admin.GetAllPendingJyotish();
            return Ok(new { Success = true, data = Records });
        }
        [HttpGet("User")]
        public IActionResult AllUser()
        {
            var Records = _admin.GetAllUser();

            return Ok(new { Success = true, data = Records });

        }
        [HttpGet("TeamMember")]
        public IActionResult AllTeamMember()
        {
            var Records = _admin.GetAllTeamMember();
            return Ok(new { Success = true, data = Records });
        }
        [HttpGet("Appointment")]
        public IActionResult AllAppointment()
        {
            List<AppointmentModel> Records = _admin.GetAllAppointment();

            return Ok(new { Success = true, data = Records });
        }

        [HttpPost("ApproveJyotish")]
        public IActionResult ApproveJyotish(IdViewModel model)
        {
            var Records = _admin.ApproveJyotish(model);
            if (Records == true)
            { return Ok(new { Status  = 200, Message = "Successfully Approved" }); }
            else { return BadRequest(); }

        }
        [HttpPost("RejectJyotish")]
        public IActionResult RejectJyotish(IdViewModel model)
        {
            var Records = _admin.RejectJyotish(model);
            if (Records == true)
            { return Ok(new { Status = 200, Message = "Successfully Rejected" }); }
            else { return BadRequest(); }
        }
        [HttpPost("RemoveJyotish")]
        public IActionResult RemoveJyotish(IdViewModel model)
        {
            var Records = _admin.RemoveJyotish(model);
            if (Records == true)
            { return Ok(new { Success = true }); }
            else { return BadRequest(); }
        }

        [HttpPost("AddPoojaCategory")]
        public IActionResult AddPoojaCategory(PoojaCategoryModel pooja)
        {
            var result = _admin.AddPoojaCategory(pooja);
            if (result == true)
            {
                return Ok(new { success = true });
            }
            else { return BadRequest(); }
        }
        [AllowAnonymous]
        [HttpPost("AddPoojaList")]
        public IActionResult AddPoojaList(PoojaListViewModel pooja)
        {
            try {
                var result = _admin.AddNewPoojaList(pooja);
                if(result)
                { return Ok(); }
                else { return BadRequest(); }
            }
            catch { return BadRequest(); }
        }

        [HttpPost("AddExpertise")]
        public IActionResult AddExpertise(ExpertiseModel expertise)
        {
            var result = _admin.AddExpertise(expertise);
            if (result == true)
            {
                return Ok(new { success = true });
            }
            else { return BadRequest(); }
        }

        [HttpGet("PoojaCategoryList")]
        public IActionResult PoojaCategoryList()
        {
            var Records = _admin.GetAllPoojaCategory();
            return Ok(new { Success = true, data = Records });

        }
        [HttpGet("ExpertiseList")]
        public IActionResult ExpertiseList()
        {
            var Records = _admin.GetAllExpertise();
            return Ok(new { Success = true, data = Records });

        }
        [HttpGet("PoojaRecord")]
        public IActionResult PoojaRecord()
        {
            var Records = _admin.PoojaRecord();
            return Ok(new { Success = true, data = Records });
        }

        [HttpGet("ChattingRecord")]
        public IActionResult ChattingRecord()
        {
            var Records = _admin.ChattingRecord();
            return Ok(new { Success = true, data = Records });
        }

        [HttpGet("CallingRecord")]
        public IActionResult CallingRecord()
        {
            var Records = _admin.CallingRecord();
            return Ok(new { Success = true, data = Records });
        }
        [HttpGet("AppointmentDetail")]
        public IActionResult AppointmentDetail(IdViewModel model)
        {
            var Record = _admin.AppointmentDetails(model.Id);
            if (Record == null)
            { return BadRequest(); }
            else
            { return Ok(new { data = Record }); }
        }
        [HttpPost("UpdateAppointment")]
        public IActionResult UpdateAppointment(AppointmentModel model)
        {
            var Record = _admin.UpdateAppointment(model);
            if (Record == false)
            { return BadRequest(); }
            else
            { return Ok(Record); }
        }

        [HttpPost("AddCountry")]
        public IActionResult AddCountry(Country country)
        {
            var result = _admin.AddCountry(country);
            if(result == false) 
            { return BadRequest(); }
            else { return Ok(); }
           
        }

        [HttpPost("AddState")]
        public IActionResult AddState(State state)
        {
            var result = _admin.AddState(state);
            if (result == false)
            { return BadRequest(); }
            else { return Ok(); }

        }

        [HttpPost("AddCity")]
        public IActionResult AddCity(City city)
        {
            var result = _admin.AddCity(city);
            if (result == false)
            { return BadRequest(); }
            else { return Ok(); }

        }
        [AllowAnonymous]
        [HttpPost("AddSlider")]
        public IActionResult AddSlider(SliderImagesViewModel model)
        {
            try {
                var result = _admin.AddSlider(model);
                if(result)
                { return Ok(); }
                else
                {
                    return BadRequest();
                }
            }
            catch { return BadRequest(); }
        }

        [AllowAnonymous]
        [HttpPost("AddPoojaDetail")]
        public IActionResult AddPoojaDetail(PoojaRecordViewModel model)
        {
            try
            {
                var result = _admin.AddPoojaDetail(model);
                if (result)
                { return Ok(); }
                else { return BadRequest(); }
            }
            catch { return BadRequest(); }
        }
    }
}
