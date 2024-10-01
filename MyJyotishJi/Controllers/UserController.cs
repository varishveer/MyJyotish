using BusinessAccessLayer.Abstraction;
using DataAccessLayer.Migrations;
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
        [HttpGet("TopAstrologer")]
        public IActionResult TopAstrologer(string city)
        {
            try
            {
                var records = _services.TopAstrologer(city);
                if(records == null)
                {
                    return Ok(new { Status = 404, Message = "Jyotish not found" });
                }
                else
                {
                    return Ok(new {Status = 200 , Data = records, Message = "Jyotish List"});
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { Message =  ex.Message, Error = ex });
            }

        }
        [AllowAnonymous]
        [HttpGet("AllAstrologer")]
        public IActionResult AllAstrologer()
        {
            try
            {
                var records = _services.AllAstrologer();
                if (records == null)
                {
                    return Ok(new { Status = 404, Message = "Jyotish not found" });
                }
                else
                {
                    return Ok(new { Status = 200, Data = records, Message = "Jyotish List" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message, Error = ex });
            }

        }

        [AllowAnonymous]
        [HttpGet("AstrologerProfile")]
        public IActionResult AstrologerProfile(int Id)
        {
            try
            {
                var record = _services.AstrologerProfile(Id);
                if (record == null)
                {
                    return Ok(new { Status = 404, Message = "Jyotish not found" });
                }
                else
                {
                    return Ok(new { Status = 200, Data = record, Message = "Jyotish Profile" });
                }
            } 
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message, Error = ex });
            }

        }

        [AllowAnonymous]
        [HttpGet("SearchAstrologer")]
        public IActionResult SearchAstrologer(string keyword)
        {
            try
            {
                var record = _services.SearchAstrologer(keyword);
                if (record == null)
                {
                    return Ok(new { Status = 404, Message = "Jyotish not found" });
                }
                else
                {
                    return Ok(new { Status = 200, Data = record, Message = "Jyotish List" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message, Error = ex });
            }

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
