using BusinessAccessLayer.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Host.Mef;
using ModelAccessLayer.Models;

namespace MyJyotishGApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserServices _services;
        public UserController(IUserServices services)
        {
            _services = services;
        }

        [AllowAnonymous]
        [HttpGet("HomePageSlider")]
        public IActionResult HomaPageSlider()
        {
            try {
                var record = _services.HomaPageSlider();
                if (record == null) { return BadRequest(); }
                return Ok(new {  Status =200, message = "Succussfull", data = record });
            }
            catch(Exception ex) { return StatusCode(500, new {Status = 500, Message = "Internal Server Error", Error = ex }); }
        }

        [AllowAnonymous]
        [HttpGet("OurAstrologer")]
        public IActionResult OurAstrologer()
        {
            try
            {
                var result = _services.OurAstrologer();
                var record = result.Select(j => new {j.Id, j.Name, j.ProfileImageUrl, j.Expertise, j.Language,j.Address,j.Experience }).ToList();
                return Ok(new { data = record, Status = 200, Message = "List Of Astrologer (Name, Profile Image , Expertise)" });
            }
            catch(Exception ex) { return StatusCode(500,new { Status = 500, Message = "Internal Server Error" , Error = ex  }); }
        }
       

        [AllowAnonymous]
        [HttpGet("GetAstroListCallChat")]
        public IActionResult GetAstroListCallChat(string ListName)
        {
            try
            {
                var record = _services.GetAstroListCallChat(ListName);
                if (record == null)
                {
                    return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = "List is empty" });
                }
                else { return Ok(new { Status = 200, message = "Succussfull", data = record }); }
            }
            catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
        }

        [AllowAnonymous]
        [HttpGet("BAPCategorySlider")]
        public IActionResult BAPCategorySlider()
        {
            try
            {
                var record = _services.PoojaListSlider();
                if (record == null) { return BadRequest(); }
                return Ok(new { Status = 200, message = "Succussfull", data = record });
            }
            catch(Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
        }
        [AllowAnonymous]
        [HttpGet("GetPoojaList")]
        public IActionResult GetPoojaList(int Id)
        {
            try
            {
                var record = _services.GetPoojaList(Id);
                if (record == null)
                { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = "List is empty" }); }
                else { return Ok(new { Status = 200, message = "Succussfull", data = record }); }
            }
            catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
        }
        [AllowAnonymous]
        [HttpGet("GetPoojaDetail")]
        public IActionResult GetPoojaDetail(int PoojaId)
        {
            try
            {
                var record = _services.GetPoojaDetail(PoojaId);
                if (record == null)
                { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = "List is empty" }); }
                else { return Ok(new { Status = 200, message = "Succussfull", data = record }); }
            }
            catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
        }

        /* [HttpGet("PoojaListSlider")]
         public IActionResult PoojaListSlider()
         {
             try
             {
                 var record = _services.PoojaListSlider();

                 return Ok(new { Status = 200, message = "Succussfull", data = record });
             }
             catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
         }
        */
        /*  [AllowAnonymous]
          [HttpGet("GetAllPoojaCategory")]
          public IActionResult GetAllPoojaCategory()
          {
              try
              {
                  var record = _services.GetAllPoojaCategory();
                  if (record == null)
                  { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = "List is empty" }); }
                  else { return Ok(new { Status = 200, message = "Succussfull", data = record }); }
              }
              catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
          }*/



    }
}
