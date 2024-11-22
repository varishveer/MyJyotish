﻿using BusinessAccessLayer.Abstraction;
using DataAccessLayer.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Host.Mef;
using ModelAccessLayer.Models;
using ModelAccessLayer.ViewModels;

namespace MyJyotishGApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Policy4")]
    public class UserController : ControllerBase
    {

        private readonly IUserServices _services;
        private readonly IWebHostEnvironment _environment;
        public UserController(IUserServices services, IWebHostEnvironment environment)
        {
            _services = services;
            _environment = environment;
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

        [HttpGet("GetAllUserAttachments")]
        public IActionResult GetAllUserAttachments(int Id)
        {
            try
            {
                var result = _services.GetAllUserAttachments(Id);

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
        [HttpGet("GetProblemSolutionDetail")]
        public IActionResult GetProblemSolutionDetail(int appointmentId)
        {
            try
            {

                var result = _services.GetProblemSolutionDetail(appointmentId);



                return Ok(new { Status = 200, Data = result, Message = "Successful" });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("FilterAstrologer")]
        public IActionResult FilterAstrologer()
        {
            try
            {
                var httpRequest = HttpContext.Request;

                FilterModel fm = new FilterModel();
                fm.country= String.IsNullOrEmpty(httpRequest.Form["country"])?0: Convert.ToInt32(httpRequest.Form["country"]);

                fm.city= String.IsNullOrEmpty(httpRequest.Form["city"])?0: Convert.ToInt32(httpRequest.Form["city"]);

                fm.state= String.IsNullOrEmpty(httpRequest.Form["state"])?0: Convert.ToInt32(httpRequest.Form["state"]);
                fm.activity= String.IsNullOrEmpty(httpRequest.Form["activity"])?0: Convert.ToInt32(httpRequest.Form["activity"]);

                fm.rating= String.IsNullOrEmpty(httpRequest.Form["rating"])?0: Convert.ToInt32(httpRequest.Form["rating"]); 
                fm.expertise = String.IsNullOrEmpty(httpRequest.Form["expertise"])?0: Convert.ToInt32(httpRequest.Form["expertise"]);

                fm.experience = httpRequest.Form["experience"];
                fm.gender = httpRequest.Form["gender"];      

                var record = _services.FilterAstrologer(fm);
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
        [HttpPost("searchAstrologers")]
        public IActionResult SearchAstrologers()
        {
            var httpRequest = HttpContext.Request;

            var searchInp = String.IsNullOrEmpty(httpRequest.Form["searchInp"]) ? "" : httpRequest.Form["searchInp"].ToString();

            var res = _services.SearchAstrologer(searchInp);
            return Ok(new {status=200,message="data retrieved",data=res});
    }

        [AllowAnonymous]
        [HttpGet("SliderImageList")]
        public IActionResult SliderImageList(string Keyword)
        {
            try {
                var records = _services.SliderImageList(Keyword);
                if (records == null)
                {
                    return Ok(new { Status = 404, Message = "No Image found" });
                }
                else
                {
                    return Ok(new { Status = 200, Data = records, Message = Keyword });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message, Error = ex });
            }

        }
        [AllowAnonymous]
        [HttpGet("GetAstroListCallChat")]
        public IActionResult GetAstroListCallChat(string ListName)
        {
            try
            {
                var record = _services.GetAstroListCallChat(ListName);
                if (record.Count == 0)
                {
                    return StatusCode(500, new { Status = 400, Message = "List is empty" });
                }
                else { return Ok(new { Status = 200, message = "Succussfull", data = record }); }
            }
            catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
        }

        [AllowAnonymous]
        [HttpGet("SpecializationFilter")]
        public IActionResult SpecializationFilter(string Keyword)
        {
            try
            {
                var record = _services.SpecializationFilter(Keyword);
                if (record.Count == 0)
                {
                    return StatusCode(500, new { Status = 400, Message = "List is empty" });
                }
                else { return Ok(new { Status = 200, message = "Succussfull", data = record }); }
            }
            catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
        }



        [HttpPost("BookAppointment")]
        public IActionResult BookAppointment(AppointmentViewModel model)
        {
            try {
                var result = _services.BookAppointment(model);
                if (result == "invalid Data")
                {
                    return Ok( new { Status = 400, Message = result });
                }

                 else  if(result == "Successful") { return Ok(new { Status = 200, message = result}); }
                else if(result =="internal Server Error.") { return Ok(new { Status = 500, message = result }); }
                else { return Ok(new { Status = 500, message = result }); }
            }
            catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
        }

       
        [HttpGet("GetUserProfile")]
        public IActionResult GetUserProfile(int Id)
        {
            try
            {
                var result = _services.GetUserProfile(Id);
                if (result == null)
                {
                    return Ok(new { Status = 404, Message = "User Not Found" });
                }

                else { return Ok(new { Status = 200, Data = result }); }
               
            }
            catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
        }


        
        [HttpPost("UpdateProfile")]
        public IActionResult UpdateProfile(UserUpdateViewModel model)
        {
            try
            {
                string? path = _environment.ContentRootPath;
                var result = _services.UpdateProfile(model, path);
                if (result == null)
                {
                    return Ok(new { Status = 404, Message = "User Not Found" });
                }

                else { return Ok(new { Status = 200, Data = result }); }

            }
            catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
        }

        [HttpGet("getAllAppointment")]
        public IActionResult getAllAppointment(int Id)
        {
            try
            {
                var result = _services.getAllAppointment(Id);
                if (result == null)
                {
                    return Ok(new { Status = 404, Message = "User Not Found" });
                }

                else { return Ok(new { Status = 200, Data = result }); }

            }
            catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }

        }


        [HttpGet("GetAppointmentDetails")]
        public IActionResult GetAppointmentDetails(int Id)
        {
            try
            {
                var result = _services.GetAppointmentDetails(Id);
                if (result == null)
                {
                    return Ok(new { Status = 404, Message = "Not Found" });
                }

                else { return Ok(new { Status = 200, Data = result }); }

            }
            catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }

        }


        [HttpGet("UserPaymentrecords")]
        public IActionResult UserPaymentrecords(int Id)
        {
            try
            {
                var Result = _services.UserPaymentrecords(Id);
                if (Result == null)
                { return Ok(new { Status = 404, Message = "Data Not Found" }); }

                else
                { return Ok(new { Status = 200, Data = Result, Message = "Successful" }); }
            }
            catch (Exception ex)
            { return StatusCode(500, new { Status = 500, Message = "Internal Server Error ", Error = ex }); }
        }

        [HttpPost("PurchaseWithUserWallets")]
        public IActionResult PurchaseWithUserWallets(UserWalletViewmodel uv)
        {
            try
            {
                var Result = _services.PurchaseWithUserWallets(uv);
                if(Result == "Successful")
                 {
                    WalletHistoryViewmodel js = new WalletHistoryViewmodel
                    {
                        UId = (int)uv.userId,
                        JId = (int)uv.jyotishId,
                        amount = (long)uv.WalletAmount,
                        PaymentAction = "Debit",
                        PaymentStatus = "success",
                        PaymentFor = uv.paymentfor
                    };
                    var historyres = _services.AddWalletHistory(js);

                    return Ok(new { Status = 200, Message = "Successful" });
                   
                }
                 else
                {

                    return Ok(new { Status = 404, Message = "Some error occured" });
                }
            }
            catch (Exception ex)
            { return StatusCode(500, new { Status = 500, Message = "Internal Server Error ", Error = ex }); }
        }

        [AllowAnonymous]
        [HttpGet("selectAllCity")]
        public IActionResult selecAllCity()
        {
            var result = _services.selecAllCity();
          return Ok(new { Status = 200, Data = result });
        }

        [HttpGet("GetWallet")]
        public IActionResult GetWallet(int UserId)
        {
            try
            {
                var Result = _services.GetWallet(UserId);
                if (Result != null)
                { return Ok(new { Status = 200, Data = Result }); }
                else
                { return Ok(new { Status = 404, Message = "Some error occured" }); }
            }
            catch (Exception ex)
            { return StatusCode(500, new { Status = 500, Message = "Internal Server Error ", Error = ex }); }
        }
        //wallet histroy
        [HttpPost("AddWalletHistory")]
        public IActionResult AddWalletHistory(WalletHistoryViewmodel jw)
        {
            try
            {
                var Result = _services.AddWalletHistory(jw);
                if (Result == "Successful")
                { return Ok(new { Status = 200, Message = "Successful" }); }
                else
                { return Ok(new { Status = 404, Message = "Some error occured" }); }
            }
            catch (Exception ex)
            { return StatusCode(500, new { Status = 500, Message = "Internal Server Error ", Error = ex }); }
        }

        [HttpGet("GetWalletHistroy")]
        public IActionResult GetWallethistory(int UserId)
        {
            try
            {
                var Result = _services.GetWalletHistory(UserId);
                if (Result != null)
                { return Ok(new { Status = 200, Data = Result }); }
                else
                { return Ok(new { Status = 404, Message = "Some error occured" }); }
            }
            catch (Exception ex)
            { return StatusCode(500, new { Status = 500, Message = "Internal Server Error ", Error = ex }); }
        }

        [HttpGet("UserPaymentDetail")]
        public IActionResult UserPaymentDetail(int Id)
        {
            try
            {
                var Result = _services.UserPaymentDetail(Id);
                if (Result == null)
                { return Ok(new { Status = 404, Message = "Data Not Found" }); }

                else
                { return Ok(new { Status = 200, Data = Result, Message = "Successful" }); }
            }
            catch (Exception ex)
            { return StatusCode(500, new { Status = 500, Message = "Internal Server Error ", Error = ex }); }
        }

        [HttpGet("GetAllAppointmentSlot")]

        public IActionResult GetAllAppointmentSlot(int id)
        {
            try
            {
                var Result = _services.GetAllAppointmentSlot(id);
                if (Result.Count == 0)
                { return Ok(new { Status = 404, Message = "Data Not Found" }); }

                else
                { return Ok(new { Status = 200, Data = Result, Message = "Successful" }); }
            }
            catch (Exception ex)
            { return StatusCode(500, new { Status = 500, Message = "Internal Server Error ", Error = ex }); }

        }

       [HttpGet("GetAllProblemSolution")]
        public IActionResult GetAllProblemSolution(int Id)
        {
            try
            {
                var Result = _services.GetAllProblemSolution(Id);
                if (Result.Count == 0|| Result == null)
                { return Ok(new { Status = 404, Message = "Data Not Found" }); }

                else
                { return Ok(new { Status = 200, Data = Result, Message = "Successful" }); }
            }
            catch (Exception ex)
            { return StatusCode(500, new { Status = 500, Message = "Internal Server Error ", Error = ex }); }

        }

        [HttpGet("GetProblemSolution")]
        public IActionResult GetProblemSolution(int Id)
        {
            try
            {
                var result = _services.GetProblemSolution(Id);
                if (result == null)
                {
                    return Ok(new { Status = 404, Message = "Not Found" });
                }

                else { return Ok(new { Status = 200, Data = result }); }

            }
            catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }

        }
         
        [HttpGet("LayoutData")]
        public IActionResult LayoutData(int Id)
        {
            try
            {
                var result = _services.LayoutData(Id);
                if (result == null)
                {
                    return Ok(new { Status = 404, Message = "Not Found" });
                }

                else { return Ok(new { Status = 200, Data = result }); }

            }
            catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }

        }

        /* [AllowAnonymous]
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
 */



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
