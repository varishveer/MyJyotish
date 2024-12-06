using BusinessAccessLayer.Abstraction;
using DataAccessLayer.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Identity.Client;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using ModelAccessLayer.Models;
using ModelAccessLayer.ViewModels;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Security.Principal;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyJyotishJiApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Policy1")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminServices _admin;
        private readonly string _uploadDirectory;
        private readonly IAccountServices _account;

        public AdminController(IAdminServices admin, IAccountServices account)
        {
            _admin = admin;
            _uploadDirectory = Directory.GetCurrentDirectory();
            _account = account;
        }
        [HttpGet("Profile")]
        public IActionResult Profile([FromQuery] string email)
        {
            try
            {
                var Records =_admin.Profile(email);
                if (Records == null)
                {
                    return Ok(new { Status = 400, data = Records, Message ="Admin Not found " });
                }
                else
                { return Ok(new { Status = 200, data = Records, Message = "Success" }); }
            }
            catch { return Ok(new { Status = 500,  Message = "Internal Server Error " }); }
           

        }
       /* [HttpGet("Dashboard")]
        public IActionResult Dashboard()
        {
            var Records =_admin.Dashboard();
            if (record == null) { return BadRequest(); }
            else { return Ok(new { success = true, data = record }); }
        }*/

        [HttpGet("Jyotish")]
        public IActionResult AllJyotishRecord()
        {
            try {
                var Records =_admin.GetAllJyotish();
                return Ok(new { Status = 200, data = Records, Message = "Success" });
            }
            catch { return Ok(new { Status = 500, Message = "Internal Server Error " }); }
        }
        [HttpGet("PendingJyotish")]
        public IActionResult AllPendingJyotishRecord()
        {
            try {
                var Records =_admin.GetAllPendingJyotish();
                return Ok(new { Status = 200, data = Records, Message = "Success" });
            }
              catch { return Ok(new { Status = 500, Message = "Internal Server Error " }); }
        }
        [HttpGet("User")]
        public IActionResult AllUser()
        {
            try { var Records =_admin.GetAllUser(); return Ok(new { Status = 200, data = Records, Message = "Success" });
            }
            catch { return Ok(new { Status = 500, Message = "Internal Server Error " }); }

        }
        [HttpGet("TeamMember")]
        public IActionResult AllTeamMember()
        {
            
            try
            {
                var Records =_admin.GetAllTeamMember();
                return Ok(new { Status = 200, data = Records, Message = "Success" });
            }
            catch { return Ok(new { Status = 500, Message = "Internal Server Error " }); }
          
        }
        [HttpGet("Appointment")]
        public IActionResult AllAppointment()
        {
            try
            {
                var Records =_admin.GetAllAppointment();
                return Ok(new { Status = 200, data = Records, Message = "Success" });
            }
            catch { return Ok(new { Status = 500, Message = "Internal Server Error " }); }
        }

        [HttpPost("ApproveJyotish")]
        public IActionResult ApproveJyotish(IdViewModel model)
        {
           
            try
            {
                var Records = _admin.ApproveJyotish(model);
                if (Records == true)
                { return Ok(new { Status = 200, Message = "Success" }); }
                else { return Ok(new { Status = 400, Message = "Bad Request" }); }
               
            }
            catch { return Ok(new { Status = 500, Message = "Internal Server Error " }); }

        }
        [HttpPost("RejectJyotish")]
        public IActionResult RejectJyotish(JyotishRejectMailViewModel model)
        {
          

            try
            {
                var Records = _admin.RejectJyotish(model);
                if (Records == true)
                { return Ok(new { Status = 200, Message = "Success" }); }
                else { return Ok(new { Status = 400, Message = "Bad Request" }); }
            }
            catch { return Ok(new { Status = 500, Message = "Internal Server Error " }); }
        }
        [HttpPost("RemoveJyotish")]
        public IActionResult RemoveJyotish(IdViewModel model)
        {
            
            try
            {
                var Records = _admin.RemoveJyotish(model);
                if (Records == true)
                { return Ok(new { Status = 200, Message = "Success" }); }
                else { return Ok(new { Status = 400, Message = "Bad Request" }); }
            }
            catch { return Ok(new { Status = 500, Message = "Internal Server Error " }); }
        }

       /* [HttpPost("AddPoojaCategory")]
        public IActionResult AddPoojaCategory(PoojaCategoryViewModel pooja)
        {
            try
           { var result = _admin.AddPoojaCategory(pooja);
                if (result == true)
                {
                    return Ok(new { Status = 200, Message = "Success" });
                }
                else { return Ok(new { Status = 400, Message = "Bad Request" }); }
            }
            catch { return Ok(new { Status = 500, Message = "Internal Server Error " }); }
        }*/
       /* [AllowAnonymous]
        [HttpPost("AddPoojaList")]
        public IActionResult AddPoojaList(PoojaListViewModel pooja)
        {
            try {
                var result = _admin.AddNewPoojaList(pooja);
                if(result)
                { return Ok(new { Status = 200, Message = "Success" }); }
                else { return Ok(new { Status = 400, Message = "Bad Request" }); }
            }
            catch { return Ok(new { Status = 500, Message = "Internal Server Error " }); }
        }
*/
        [HttpPost("AddExpertise")]
        public IActionResult AddExpertise(ExpertiseModel expertise)
        {
            try
            {
                var result = _admin.AddExpertise(expertise);
                if (result == true)
                {
                    return Ok(new { Status = 200, Message = "Success" });
                }
                else { return Ok(new { Status = 400, Message = "Bad Request" }); }
            }
            catch { return Ok(new { Status = 500, Message = "Internal Server Error " }); }
        }

      /*  [HttpGet("PoojaCategoryList")]
        public IActionResult PoojaCategoryList()
        {
            try
            {
                var Records =_admin.GetAllPoojaCategory();
                return Ok(new { Status = 200, data = Records, Message = "Success" });
            }
            catch {return Ok(new { Status = 500, Message = "Internal Server Error " }); }
        }*/
        [HttpGet("ExpertiseList")]
        public IActionResult ExpertiseList()
        {
            try
           { var Records =_admin.GetAllExpertise();
                return Ok(new { Status = 200, data = Records, Message = "Success" });
            }
            catch { return Ok(new { Status = 500, Message = "Internal Server Error " }); }
        }
        [HttpGet("PoojaRecord")]
        public IActionResult PoojaRecord()
        {
            try
            {
                var Records = _admin.PoojaRecord();
                return Ok(new { Status = 200, data = Records, Message = "Success" });
            }
            catch {return Ok(new { Status = 500, Message = "Internal Server Error " }); }

        }

      /*  [HttpGet("ChattingRecord")]
        public IActionResult ChattingRecord()
        {
           try
            { var Records = _admin.ChattingRecord();
                return Ok(new { Status = 200, data = Records, Message = "Success" });
            }
            catch { return Ok(new { Status = 500, Message = "Internal Server Error " }); }
        }

        [HttpGet("CallingRecord")]
        public IActionResult CallingRecord()
        {
           
            try
            {
                var Records = _admin.CallingRecord();
                return Ok(new { Status = 200, data = Records, Message = "Success" });
            }
            catch { return Ok(new { Status = 500, Message = "Internal Server Error " }); }
        }*/
        [HttpGet("AppointmentDetail")]
        public IActionResult AppointmentDetail(IdViewModel model)
        {
            try
            {
            var Records =_admin.AppointmentDetails(model.Id);
                if (Records == null)
                { return Ok(new { Status = 400, Message = "Record Not Found" }); }
                else
                { return Ok(new { Status = 200, data = Records, Message = "Success" }); }
            }
            catch { return Ok(new { Status = 500, Message = "Internal Server Error " }); }
        }
      /*  [HttpPost("UpdateAppointment")]
        public IActionResult UpdateAppointment(UpdateAppointmentJyotishViewModel model)
        {
            try
            {
                var result = _admin.UpdateAppointment(model);
                if (result == "invalid Data")
                {
                    return Ok(new { Status = 400, Message = result });
                }

                else if (result == "Successful") { return Ok(new { Status = 200, message = result }); }
                else if (result == "internal Server Error.") { return Ok(new { Status = 500, message = result }); }
                else { return Ok(new { Status = 500, message = result }); }
            }
            catch {return Ok(new { Status = 500, Message = "Internal Server Error " }); }
        }*/

      
        [HttpPost("AddSlider")]
        public IActionResult AddSlider(SliderImagesViewModel model)
        {
            try {
                var result = _admin.AddSlider(model);
                if(result)
                { return Ok(new { Status = 200, Message = "Success" }); }
                else
                {
                    return Ok(new { Status = 400, Message = "Bad Request" });
                }
            }
            catch(Exception ex) { return Ok(new { Status = 500, Message = "Internal Server Error ", Error = ex }); }
        }
        [HttpGet("SliderImageList")]
        public IActionResult SliderImageList()
        {
            try
            {
                var result = _admin.SliderImageList();
                if (result == null)
                { return Ok(new { Status = 400, Message = "No Data Found" }); }
                else
                {
                    return Ok(new { Status = 200,Data = result, Message = "Successful" });
                   
                }
            }
            catch (Exception ex) { return Ok(new { Status = 500, Message = "Internal Server Error ", Error = ex }); }
        }

      /*  [AllowAnonymous]
        [HttpPost("AddPoojaDetail")]
        public IActionResult AddPoojaDetail(PoojaRecordViewModel model)
        {
            try
            {
                var result = _admin.AddPoojaDetail(model);
                if (result)
                { return Ok(new { Status = 200, Message = "Success" }); }
                else { return Ok(new { Status = 400, Message = "Bad Request" }); }
            }
            catch { return Ok(new { Status = 500, Message = "Internal Server Error " }); }
        }*/
        [HttpGet("GetAllJyotishDocument")]
        public IActionResult GetAllJyotishDocument()
        {
            try
            {
                var Records = _admin.GetAllJyotishDocument();
                return Ok(new {Status = 200 , Data = Records, Message = "All Jyotish Document"});
            }
            catch { return Ok(new { Status = 500, Message = "Internal Server Error " }); }
        }


      /*  [HttpGet("GetJyotishCalls")]
        public IActionResult GetJyotishCalls(int id)
        {
            try {
                var Records = _admin.GetJyotishCalls(id);
                if(Records == null)
                {
                    return Ok(new { Status = 400, Message = "No Record found." });
                }
                return Ok(new { Status = 200, Data = Records, Message = "Jyotish Calls" });
            }
            catch { return Ok(new { Status = 500, Message = "Internal Server Error " }); }
        }

        [HttpGet("GetJyotishChats")]
        public IActionResult GetJyotishChats(int id)
        {
            try
            {
                var Records = _admin.GetJyotishChats(id);
                if (Records == null)
                {
                    return Ok(new { Status = 400, Message = "No Record found." });
                }
                return Ok(new { Status = 200, Data = Records, Message = "Jyotish Chats" });
            }
            catch { return Ok(new { Status = 500, Message = "Internal Server Error " }); }
        }*/
        
        [HttpGet("GetJyotishDocs")]
        public IActionResult GetJyotishDocs(int id)
        {
            try
            {
                var Records = _admin.GetJyotishDocs(id);
                if (Records == null)
                {
                    return Ok(new { Status = 400, Message = "No Record found." });
                }
                return Ok(new { Status = 200, Data = Records, Message = "Jyotish Docs" });
            }
            catch { return Ok(new { Status = 500, Message = "Internal Server Error " }); }
        }
        [HttpGet("GetJyotishDetails")]
        public IActionResult GetJyotishDetails(int id)
        {
            try
            {
                var Records = _admin.GetJyotishDetails(id);
                if (Records == null)
                {
                    return Ok(new { Status = 400, Message = "No Record found." });
                }
                return Ok(new { Status = 200, Data = Records, Message = "Jyotish Details" });
            }
            catch { return Ok(new { Status = 500, Message = "Internal Server Error " }); }
        }

      /*  [HttpPost("UpdateJyotishDetails")]
        public IActionResult UpdateJyotishDetails(JyotishDetailsViewModel model)
        {

            try
            {
                var Records = _admin.UpdateJyotishDetails(model);
                if (Records == "Jyotish not found")
                {
                    return Ok(new { Status = 400, Message = Records });
                }
                else if(Records == "Successful")
                {
                    return Ok(new { Status = 200, Message = Records });
                }
                else { return Ok(new { Status = 500, Message = Records }); }
            }
            catch { return Ok(new { Status = 500, Message = "Internal Server Error " }); }
        }*/

        [HttpPost("ApproveJyotishDocs")]
        public IActionResult  ApproveJyotishDocs(JyotishDocApproveViewModel model)
        {
            try
            {
                var Result = _admin.ApproveJyotishDocs(model);
                if (Result == "Successful")
                {
                    return Ok(new { Status = 200, Message = "Successful" });
                  
                }
                return Ok(new { Status = 404, Message = Result });
            }
            catch
            {
                return Ok(new { Status = 500, Message = "Internal Server Error " });
            }

        }

        [HttpPost("RejectJyotishDocs")]
        public IActionResult RejectJyotishDocs(EmailDocumentViewModel model)
        {
            try
            {
                var Result = _admin.RejectJyotishDocs(model);
                if (Result == null)
                {
                    return Ok(new { Status = 400, Message = "Jyotish not found" });
                }
                return Ok(new { Status = 200, Message = "Successful" });
            }
            catch
            {
                return Ok(new { Status = 500, Message = "Internal Server Error " });
            }

        }
        [HttpPost("AddSlot")]
        public IActionResult AddSlot(SlotViewModel slot)
        {
            try
            {
                var Result = _admin.AddSlot(slot);
                if (Result == "Successful")
                {
                    return Ok(new { status = 200, message = "Successful" });
                }
                else if (Result == "Invalid Data")
                { return Ok(new { status = 400, message = Result }); }
                else
                {
                    return Ok(new { Status = 500, Message = "Internal Server Error " });
                }
            }
            catch
            {
                return StatusCode(500, new { Status = 500, Message = "Internal Server Error " });
            }
        }

        [HttpPost("AddAppointmentSlot")]
        public IActionResult AddAppointmentSlot(AppointmentSlotViewModel model)
        {
            try
            {
                var Result = _admin.AddAppointmentSlot(model);
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
                else
                { return Ok(new { Status = 500, Message = Result }); }
            }
            catch (Exception ex)
            { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
        }

        [HttpPost("removeAppointmentSlot")]
        public IActionResult RemoveAppointmentSlot(AppointmentSlotViewModel model)
        {
            try
            {
                var result = _admin.RemoveSlotWithskipDates(model);
                if (result == "Changes applied successfully")
                {
                    return Ok(new { Status = 200, Message = "Changes applied successfully" });
                }
                else
                {
                    return Ok(new { Status = 404, Message = "No changes found" });
                }
            }
            catch (Exception ex)
            { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
        }


        [HttpGet("SlotList")]
        public IActionResult SlotList()
        { try
            {
                var Records = _admin.SlotList();
                 if(Records == null)
                {
                    return Ok(new { Status =400, Message = "Invalid Data"});
                }
                 else 
                {
                    return Ok(new { Status = 200, data = Records, Message = "Successful" });
                }
                
            }
            catch { return StatusCode(500, new { Status = 500, Message = "Internal Server Error " }); }
        }
        [HttpPost("ApproveDocument")]
        public IActionResult ApproveDocument(JyotishDocApproveViewModel model)
        {
            try 
            {
                var Result = _admin.ApproveJyotishDocs(model);
                if(Result == "Jyotish not found")
                { return Ok(new { Status = 409, Message = Result }); }
                if(Result == "documents not found")
                { return Ok(new { Status = 409, Message = Result }); }
                if(Result == "Successful")
                { return Ok(new { Status = 200, Message = Result }); }
                else
                { return Ok(new { Status = 500, Message = Result }); } 


            }

            catch { return StatusCode(500, new { Status = 500, Message = "Internal Server Error " }); }

        }

        [HttpPost("RejectDocument")]
        public IActionResult RejectDocument(EmailDocumentViewModel model)
        {
            try
            {
                var Result = _admin.RejectJyotishDocs(model);
                if (Result == "Jyotish not found")
                { return Ok(new { Status = 409, Message = Result }); }
                if (Result == "documents not found")
                { return Ok(new { Status = 409, Message = Result }); }
                if (Result == "Successful")
                { return Ok(new { Status = 200, Message = Result }); }
                else
                { return Ok(new { Status = 500, Message = Result }); }


            }

            catch { return StatusCode(500, new { Status = 500, Message = "Internal Server Error " }); }

        }

        [HttpPost("AddFeature")]
        public IActionResult AddFeature(SubscriptionFeatureViewModel model)
        {
            try
            {
               
                var Result = _admin.AddFeature(model);
                if (Result == "Invalid Data")
                { return Ok(new { Status = 409, Message = Result }); }
                if (Result == "Successful")
                { return Ok(new { Status = 200, Message = Result }); }
                else
                { return Ok(new { Status = 500, Message = Result }); }


            }

            catch { return StatusCode(500, new { Status = 500, Message = "Internal Server Error " }); }

        }

        [HttpGet("GetAllFeatures")]
        public IActionResult GetAllFeatures()
        {
            try
            {
                var Result = _admin.GetAllFeatures();
                if (Result == null)
                { return Ok(new { Status = 404, Message = "Data Not Found" }); }
               
                else
                { return Ok(new { Status = 200, Data = Result , Message = "Successful" }); }


            }

            catch { return StatusCode(500, new { Status = 500, Message = "Internal Server Error " }); }

        }

        [HttpGet("DeleteFeature")]
        public IActionResult DeleteFeature(int Id)
        {
            try
            {
                var Result = _admin.DeleteFeature(Id);
                if (Result == "Invalid Data")
                { return Ok(new { Status = 409, Message = Result }); }
                if (Result == "Successful")
                { return Ok(new { Status = 200, Message = Result }); }
                else
                { return Ok(new { Status = 500, Message = Result }); }


            }

            catch { return StatusCode(500, new { Status = 500, Message = "Internal Server Error " }); }

        }
        [HttpPost("UpdateFeature")]
        public IActionResult UpdateFeature(SubscriptionFeatureViewModel model)
        {
            try
            {
                var Result = _admin.UpdateFeature(model);
                if (Result == "Invalid Data")
                { return Ok(new { Status = 409, Message = Result }); }
                if (Result == "Successful")
                { return Ok(new { Status = 200, Message = Result }); }
                else
                { return Ok(new { Status = 500, Message = Result }); }


            }

            catch { return StatusCode(500, new { Status = 500, Message = "Internal Server Error " }); }

        }


        [HttpPost("AddManageSubscriptionData")]
        public IActionResult AddManageSubscriptionData()
        {
            try
            {
                var httpRequest = HttpContext.Request;
                ManageSubscriptionViewModel model = new ManageSubscriptionViewModel
                {
                    FeatureId = Convert.ToInt32(httpRequest.Form["features"]),

                    SubscriptionId = Convert.ToInt32(httpRequest.Form["plan"]),
                    ServiceCount = Convert.ToInt32(httpRequest.Form["serviceCount"])
                };

                var Result = _admin.AddManageSubscriptionData(model);
                if (Result == "Invalid Data")
                { return Ok(new { Status = 409, Message = Result }); }
                if (Result == "Successful")
                { return Ok(new { Status = 200, Message = "Record Added Successfully" }); }
                else
                { return Ok(new { Status = 500, Message = Result }); }


            }

            catch { return StatusCode(500, new { Status = 500, Message = "Internal Server Error " }); }

        }

        [HttpPost("UpdateManageSubscriptionData")]
        public IActionResult UpdateManageSubscriptionData()
        {
            try
            {
                var httpRequest = HttpContext.Request;
                ManageSubscriptionViewModel model = new ManageSubscriptionViewModel
                {
                    FeatureId = Convert.ToInt32(httpRequest.Form["features"]),
                    SubscriptionId = Convert.ToInt32(httpRequest.Form["plan"]),
                    ServiceCount = Convert.ToInt32(httpRequest.Form["serviceCount"])
                };
                var Result = _admin.UpdateManageSubscriptionData(model);
                if (Result == "Invalid Data")
                { return Ok(new { Status = 409, Message = Result }); }
                if (Result == "Successful")
                { return Ok(new { Status = 200, Message = "Record Updated Successfully" }); }
                else
                { return Ok(new { Status = 500, Message = Result }); }


            }

            catch { return StatusCode(500, new { Status = 500, Message = "Internal Server Error " }); }

        }

        [HttpGet("DeleteManageSubscriptionData")]
        public IActionResult DeleteManageSubscriptionData(int Id)
        {
            try
            {
                var Result = _admin.DeleteManageSubscriptionData(Id);
                if (Result == "Invalid Data")
                { return Ok(new { Status = 409, Message = Result }); }
                if (Result == "Successful")
                { return Ok(new { Status = 200, Message = Result }); }
                else
                { return Ok(new { Status = 500, Message = Result }); }


            }

            catch { return StatusCode(500, new { Status = 500, Message = "Internal Server Error " }); }

        }

        [HttpGet("GetAllManageSubscriptionData")]
        public IActionResult GetAllManageSubscriptionData()
        {
            try
            {
                var Result = _admin.GetAllManageSubscriptionData();
                if (Result == null)
                { return Ok(new { Status = 404, Message = "Data Not Found" }); }

                else
                { return Ok(new { Status = 200, Data = Result, Message = "Successful" }); }

            }

            catch { return StatusCode(500, new { Status = 500, Message = "Internal Server Error " }); }

        }

        [HttpPost("AddSpecialization")]
        public IActionResult AddSpecialization()
        {
            try
            {
                var httpRequest = HttpContext.Request;
                var name = httpRequest.Form["name"];
                if (!string.IsNullOrEmpty(name))
                {
                    var res = _admin.AddSpecialization(name);
                    if (res)
                    {
                        return Ok(new { status = 200, message = "Record added successfully" });
                    }
                    else
                    {
                        return Ok(new { status = 500, message = "some error occured" });

                    }
                }
                else
                {
                    return Ok(new { status = 400, message ="Specialization required" });

                }
            }
            catch { return StatusCode(500, new { Status = 500, Message = "Internal Server Error " }); }

        }
        [AllowAnonymous]
        [HttpGet("GetSpecializationList")]
        public IActionResult GetSpecializationList()
        {
            try
            {
                var records = _admin.GetSpecializationList();
                return Ok(new { Status = 200, Data = records });
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { Status = 500, Message = "An error occurred while fetching specialization list.", Error = ex.Message });
            }
        }

        [HttpPost("AddSubscription")]
        public IActionResult AddSubscription(SubscriptionViewModel model)
        {
            try
            {
                var Result = _admin.AddSubscription(model);
                if (Result == "Invalid Data")
                { return Ok(new { Status = 409, Message = Result }); }
                if (Result == "Successful")
                { return Ok(new { Status = 200, Message = Result }); }
                else
                { return Ok(new { Status = 500, Message = Result }); }


            }

            catch { return StatusCode(500, new { Status = 500, Message = "Internal Server Error " }); }
        }


        [HttpPost("UpdateSubscription")]
        public IActionResult UpdateSubscription(SubscriptionViewModel model)
        {
            try
            {
                var Result = _admin.UpdateSubscription(model);
                if (Result == "Invalid Data")
                { return Ok(new { Status = 409, Message = Result }); }
                if (Result == "Subscription Not Found")
                { return Ok(new { Status = 404, Message = Result }); }
                if (Result == "Successful")
                { return Ok(new { Status = 200, Message = Result }); }
                else
                { return Ok(new { Status = 500, Message = Result }); }


            }

            catch(Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error " ,Error = ex}); }
        }

        [HttpGet("GetAllSubscription")]
        public IActionResult GetAllSubscription()
        {
            try
            {
                var Result = _admin.GetAllSubscription();
                if (Result == null)
                { return Ok(new { Status = 404, Message = "Data Not Found" }); }
               
                else
                { return Ok(new { Status = 200, Data = Result  , Message = "Successful"}); }


            }

            catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error ", Error = ex }); }
        }

        [HttpDelete("DeleteSubsciption")]
        public IActionResult DeleteSubsciption( int Id)
        {
            try
            {
                var Result = _admin.DeleteSubsciption(Id);
                if (Result == "Data Not Found")
                { return Ok(new { Status = 404, Message = Result }); }
                if (Result == "Successful")
                { return Ok(new { Status = 200, Message = Result }); }
                else
                { return Ok(new { Status = 500, Message = Result }); }


            }

            catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error ", Error = ex }); }
        }

        [HttpGet("GetSubscription")]
        public IActionResult GetSubscription(int Id)
        {
            try
            {
                var Result = _admin.GetSubscription(Id);
                if (Result == null)
                { return Ok(new { Status = 404, Message = "Data Not Found" }); }

                else
                { return Ok(new { Status = 200, Data = Result, Message = "Successful" }); }


            }

            catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error ", Error = ex }); }
        }

        [HttpGet("JyotishPaymentrecords")]
        public IActionResult JyotishPaymentrecords()
        {
            try
            {
                var Result = _admin.JyotishPaymentrecords();
                if (Result == null)
                { return Ok(new { Status = 404, Message = "Data Not Found" }); }

                else
                { return Ok(new { Status = 200, Data = Result, Message = "Successful" }); }
            }
            catch (Exception ex) 
            { return StatusCode(500, new { Status = 500, Message = "Internal Server Error ", Error = ex }); }
        }

        [HttpGet("JyotishPaymentDetail")]
        public IActionResult JyotishPaymentDetail(int Id)
        {
            try
            {
                var Result = _admin.JyotishPaymentDetail(Id);
                if (Result == null)
                { return Ok(new { Status = 404, Message = "Data Not Found" }); }

                else
                { return Ok(new { Status = 200, Data = Result, Message = "Successful" }); }
            }
            catch (Exception ex)
            { return StatusCode(500, new { Status = 500, Message = "Internal Server Error ", Error = ex }); }
        }

        [HttpGet("UserPaymentrecords")]
        public IActionResult UserPaymentrecords()
        {
            try
            {
                var Result = _admin.UserPaymentrecords();
                if (Result == null)
                { return Ok(new { Status = 404, Message = "Data Not Found" }); }

                else
                { return Ok(new { Status = 200, Data = Result, Message = "Successful" }); }
            }
            catch (Exception ex)
            { return StatusCode(500, new { Status = 500, Message = "Internal Server Error ", Error = ex }); }
        }

        [HttpGet("UserPaymentDetail")]
        public IActionResult UserPaymentDetail(int Id)
        {
            try
            {
                var Result = _admin.UserPaymentDetail(Id);
                if (Result == null)
                { return Ok(new { Status = 404, Message = "Data Not Found" }); }

                else
                { return Ok(new { Status = 200, Data = Result, Message = "Successful" }); }
            }
            catch (Exception ex)
            { return StatusCode(500, new { Status = 500, Message = "Internal Server Error ", Error = ex }); }
        }


        [HttpGet("GetAllProblemSolution")]
        public IActionResult GetAllProblemSolution()
        {
            try
            {
                var Result = _admin.GetAllProblemSolution();
                if (Result.Count == 0 || Result == null)
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
                var Result = _admin.GetProblemSolution(Id);
                if ( Result == null)
                { return Ok(new { Status = 404, Message = "Data Not Found" }); }

                else
                { return Ok(new { Status = 200, Data = Result, Message = "Successful" }); }
            }
            catch (Exception ex)
            { return StatusCode(500, new { Status = 500, Message = "Internal Server Error ", Error = ex }); }

        }

        [HttpPost("RescheduleInterview")]
        public IActionResult RescheduleInterview( ReschduledInterviewViewModel model)
        {
            try
            {
                var result = _admin.ReschduledInterview(model);

                if (result == "Successful")
                {
                    return Ok(new { Status = 200, Message = result });
                }
                else if (result == "Jyotish Not Found" || result == "Slot Not Found" || result == "Jyotish slot details not found")
                {
                    return Ok(new { Status = 404, Message = result });
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


        [HttpGet("GetInterviewList")]
        public IActionResult GetInterviewList()
        {
            try
            {
                var interviewList = _admin.InteviewList(); 

                if (interviewList == null || !interviewList.Any())
                {
                    return Ok(new { Status = 404, Message = "No interview records found" });
                }

                return Ok(new { Status = 200, Data = interviewList, Message = "Successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex.Message });
            }
        }

        [HttpGet("JyotishProfile")]
        public IActionResult JyotishProfile(int Id)
        {
            try
            {
                var record = _admin.JyotishProfile(Id);

                if (record == null )
                {
                    return Ok(new { Status = 404, Message = "No record found" });
                }

                return Ok(new { Status = 200, Data = record, Message = "Successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex.Message });
            }
        }

        [HttpGet("getPlan")]
        public IActionResult GetPlan(int Id)
        {
            try
            {
                var result = _admin.getPlan(Id);
                if (result == null)
                {
                    return Ok(new { Status = 404, Message = "Not Found" });
                }

                else { return Ok(new { Status = 200, Data = result }); }

            }
            catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }

        }



        [HttpGet("NotificationData")]
        public IActionResult NotificationData()
        {
            try
            {
                var result = _admin.NotificationData();

                if (result == null || result.Count == 0)
                {
                    return Ok(new { Status = 400, Message = "No Data Found." });
                }

                return Ok(new { Status = 200, Data = result, Message = "Successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex.Message });
            }
        }

        //country code
        [HttpPost("countryCode")]
        public IActionResult countryCode()
        {
            try
            {
                var httpRequest = HttpContext.Request;

                CountryCodeViewModel model = new CountryCodeViewModel
                {
                    country = Convert.ToInt32(httpRequest.Form["country"]),
                    countryCode = Convert.ToInt32(httpRequest.Form["countryCode"])
                };
                var result = _admin.countryCode(model);

                if (result )
                {
                    return Ok(new { Status = 200, Message = "Record Added Successfully" });
                }

                return Ok(new { Status = 400, Data = result, Message = "something went wrong" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex.Message });
            }
        } 
        [HttpGet("getcountryCode")]
        public IActionResult GetcountryCode()
        {
            try
            {
                var result = _admin.getCountryCode();
                return Ok(new { Status = 200, Message = "data received",data=result });
             
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex.Message });
            }
        }

        [HttpPost ("AddInterviewFeedback")]
        public IActionResult AddInterviewFeedback(InterviewFeedbackViewModel Data)
        {
            try
            {
                var result = _admin.AddInterviewFeedback(Data);

                if (result == "Successful")
                {
                    return Ok(new { Status = 200, Message = "Record Added Successfully" });
                }
                if (result == "Invalid Data")
                {
                    return Ok(new { Status = 400, Message = result });
                }

                return Ok(new { Status = 400, Data = result, Message = "something went wrong" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex.Message });
            }
        }

        [HttpGet("GetAllInterviewFeedback")]
        public IActionResult GetAllInterviewFeedback() 
        {
            try
            {
                var result = _admin.GetAllInterviewFeedback();
                return Ok(new { Status = 200, Message = "data received", data = result });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex.Message });
            }
        } 
        [HttpGet("getJyotishByEmail")]
        public IActionResult getJyotishByEmail(string email) 
        {
            try
            {
                var result = _admin.getJyotishByEmail(email);
                return Ok(new { Status = 200, Message = "data received", data = result });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex.Message });
            }
        }
        [HttpPost("AddRedeamCode")]
        public IActionResult AddRedeamCode() 
        {
            try
            {
                var httpRequest = HttpContext.Request;
                redeamCodeViewModel rcode = new redeamCodeViewModel
                {
                    PlanId = Convert.ToInt32(httpRequest.Form["planId"]),
                    ReadeamCode = httpRequest.Form["redeamCode"],
                    discount= float.Parse((httpRequest.Form["discount"])),
                    discountAmount = float.Parse(httpRequest.Form["discountAmount"]),
                    email = httpRequest.Form["email"],
                    startDate = Convert.ToDateTime(httpRequest.Form["startDate"]),
                    endDate = Convert.ToDateTime(httpRequest.Form["endDate"])
                };
                var result = _admin.AddRedeamCode(rcode);
                if (result)
                {
                    return Ok(new { status = 200, message = "Record Added Successfully" });
                }
                else
                {
                    return Ok(new { status = 500, message = "some error occured" });

                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex.Message });
            }
        }

        [HttpGet("PendingRatingList")]
        public IActionResult PendingRatingList()
        {
            try {
                var record = _admin.PendingRatingList();
                if (record != null)
                { return Ok(new { Status = 200, Data = record, Message = "Pending Rating Records" }); }
                else { return Ok(new { Status = 500, Message = "Internal Server Error" }); }
            }
            catch(Exception ex) { return StatusCode(500, new { Status =500 , Message = "Internal Server Error", Error = ex.Message}); }
        }

        [HttpGet("ApprovedRatingList")]
        public IActionResult ApprovedRatingList()
        {
            try
            {
                var record = _admin.ApprovedRatingList();
                if (record != null)
                { return Ok(new { Status = 200, Data = record, Message = "Approved Rating Records" }); }
                else { return Ok(new { Status = 500, Message = "Internal Server Error" }); }
            }
            catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex.Message }); }
        }
        [HttpPost("ApproveRating")]
        public IActionResult ApproveRating(int Id)
        {
            try {
                var result = _admin.ApproveRating(Id);
                if (result == "Successful")
                { return Ok(new { Status = 200, Message =  "Successfully Record Updated"}); }
                else if(result == "Invalid Id")
                {
                    return Ok(new { Status = 400, Message = "Invalid Id" });
                }
                else
                {
                    return StatusCode(500, new {Status = 500, Message = result});
                }
            }
            catch(Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex.Message }); }
        }

        [HttpPost("DeleteRating")]
        public IActionResult DeleteRating(int Id)
        {
            try
            {
                var result = _admin.DeleteRating(Id);
                if (result == "Successfully Deleted")
                { return Ok(new { Status = 200, Message = "Successfully Record Deleted" }); }
                else if (result == "Invalid Id")
                {
                    return Ok(new { Status = 400, Message = "Invalid Id" });
                }
                else
                {
                    return StatusCode(500, new { Status = 500, Message = result });
                }
            }
            catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex.Message }); }
        }

        [HttpPost("AddDepartment")]
        public IActionResult AddDepartment()
        {
            var httpRequest = HttpContext.Request;
            DepartmentViewModel model = new DepartmentViewModel
            {
                DepartmentName = httpRequest.Form["departmentName"]
            };

            var res = _admin.AddDepartments(model);
            if (res)
            {
                return Ok(new { status= 200, message = "Department Added Successfully" });
            }
            else
            {
                return Ok(new { status = 500, message = "some error occured" });

            }
        }
        [HttpPost("Addlevels")]
        public IActionResult AddLevels()
        {
            var httpRequest = HttpContext.Request;
            LevelsViewModel model = new LevelsViewModel
            {
                LevelsName = httpRequest.Form["levelsName"]
            };

            var res = _admin.AddLevels(model);
            if (res)
            {
                return Ok(new { status= 200, message = "Levels Added Successfully" });
            }
            else
            {
                return Ok(new { status = 500, message = "some error occured" });

            }
        }

        [HttpGet("levelsList")]
        public IActionResult LevelList()
        {
            var res = _admin.LevelsList();
            return Ok(new {status=200,data=res});
        } 
        [HttpGet("DepartmentList")]
        public IActionResult DepartmentList()
        {
            var res = _admin.DepartmentList();
            return Ok(new {status=200,data=res});
        }

        [HttpPost("AddEmployees")]
        public IActionResult AddEmployee()
        {
            try
            {
                var httpRequest = HttpContext.Request;

                if (httpRequest.Form.Files["IdProof"] == null)
                {
                    return Ok(new { status = 500, message = "Id proof is required" });

                }

                EmployeesViewModel model = new EmployeesViewModel
                {
                    Name = httpRequest.Form["name"],
                    gender = httpRequest.Form["gender"],
                    DateOfBirth = httpRequest.Form["dob"],
                    Email = httpRequest.Form["email"],
                    mobile = Convert.ToInt64(httpRequest.Form["mobile"]),
                    Department = Convert.ToInt32(httpRequest.Form["department"]),
                    levels = Convert.ToInt32(httpRequest.Form["levels"])

                };

                var res = _admin.AddEmployees(model);
                dynamic resdocs = false;
                if (res)
                {


                    var ProfileGuid = Guid.NewGuid().ToString();

                    Dictionary<string, string> docsList = new Dictionary<string, string>();

                    if (httpRequest.Form.Files["IdProof"] != null)
                    {
                        IFormFile IdProof = httpRequest.Form.Files["IdProof"];
                        var SqlPath = "wwwroot/Images/admin/" + ProfileGuid + IdProof.FileName;
                        var ProfilePath = Path.Combine(_uploadDirectory, SqlPath);
                        using (var stream = new FileStream(ProfilePath, FileMode.Create))
                        {
                            IdProof.CopyTo(stream);
                        }
                        var ImageUrl = "Images/admin/" + ProfileGuid + IdProof.FileName;
                        docsList.Add("IdProof", ImageUrl);
                    }
                    if (httpRequest.Form.Files["metrics"] != null)
                    {
                        IFormFile metrics = httpRequest.Form.Files["metrics"];
                        var SqlPath = "wwwroot/Images/admin/" + ProfileGuid + metrics.FileName;
                        var ProfilePath = Path.Combine(_uploadDirectory, SqlPath);
                        using (var stream = new FileStream(ProfilePath, FileMode.Create))
                        {
                            metrics.CopyTo(stream);
                        }
                        var ImageUrl = "Images/admin/" + ProfileGuid + metrics.FileName;
                        docsList.Add("Metrics", ImageUrl);

                    }
                    if (httpRequest.Form.Files["postmetrics"] != null)
                    {
                        IFormFile postmetrics = httpRequest.Form.Files["postmetrics"];
                        var SqlPath = "wwwroot/Images/admin/" + ProfileGuid + postmetrics.FileName;
                        var ProfilePath = Path.Combine(_uploadDirectory, SqlPath);
                        using (var stream = new FileStream(ProfilePath, FileMode.Create))
                        {
                            postmetrics.CopyTo(stream);
                        }
                        var ImageUrl = "Images/admin/" + ProfileGuid + postmetrics.FileName;
                        docsList.Add("postmentrics", ImageUrl);
                    }
                    if (httpRequest.Form.Files["degrees"] != null)
                    {
                        IFormFile degree = httpRequest.Form.Files["degrees"];
                        var SqlPath = "wwwroot/Images/admin/" + ProfileGuid + degree.FileName;
                        var ProfilePath = Path.Combine(_uploadDirectory, SqlPath);
                        using (var stream = new FileStream(ProfilePath, FileMode.Create))
                        {
                            degree.CopyTo(stream);
                        }
                        var ImageUrl = "Images/admin/" + ProfileGuid + degree.FileName;
                        docsList.Add("Degree", ImageUrl);
                    }
                    foreach (var doc in docsList)
                    {
                        EmployeesDocsViewModel empDocs = new EmployeesDocsViewModel
                        {
                            url = doc.Value,
                            name = doc.Key,
                            email = model.Email
                        };
                        resdocs = _admin.AddEmployeesDocs(empDocs);
                    }



                }
                if (resdocs && res)
                {


                    return Ok(new { status = 200, message = "record added successfully" });
                }
                else
                {


                    return Ok(new { status = 500, message = "something went wrong" });

                }
            } 
            catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex.Message
    }); }

        }

        [HttpPost("AddAccessPages")]
        public IActionResult AddAccessPages()
        {
            try
            {
                var httpRequest = HttpContext.Request;
                EmployeesAccessPagesViewModel pages = new EmployeesAccessPagesViewModel
                {
                    pagesName = httpRequest.Form["pageName"],
                    pageUrl = httpRequest.Form["pageUrl"]
                };

                var res = _admin.AddAccessPages(pages);
                if (res)
                {
                    return Ok(new { status = 200, message = "Record Added Successfully" });
                }
                else
                {
                    return Ok(new { status = 500, message = "some error occured" });

                }
            } 
            catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex.Message
    }); }
        }
        [HttpPost("AccessPagesValidation")]
        public IActionResult AccessPagesValidation()
        {
            try
            {
                var httpRequest = HttpContext.Request;

                int[] pagesIds = httpRequest.Form["pages"].ToString().Split(',').Select(e => int.Parse(e)).ToArray();
                var res = false;
                foreach (var pageId in pagesIds)
                {
                    AccessPageValidation pages = new AccessPageValidation
                    {
                        DepartmentId = Convert.ToInt32(httpRequest.Form["Department"]),
                        PageId = pageId
                    };

                     res = _admin.AddPageAccessValidation(pages);
                }

              if (res)
                {
                    return Ok(new { status = 200, message = "Record Added Successfully" });
                }
                else
                {
                    return Ok(new { status = 500, message = "some error occured" });

                }
            } 
            catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex.Message}); }
        }

        [HttpGet("getAllAccessPages")]
        public IActionResult getAllAccesPages()
        {
            try
            {
                var res = _admin.getAccessPages();
                return Ok(new { status = 200, message = "data retrieve successfully", data = res });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = 500,
                    Message = "Internal Server Error",
                    Error = ex.Message
                });
            }
        }

        [HttpPost("AddInterviewMeeting")]
        public IActionResult AddInterviewMeeting(InterviewMeetingViewModel data)
        {
            try
            {
                var res = _admin.AddInterviewMeeting(data);
                if (res) { return Ok(new { Status = 200, Message = "Successful" }); }
                else { return Ok(new { Status = 400, Message = "Something went wrong" }); }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = 500,
                    Message = "Internal Server Error",
                    Error = ex.Message
                });
            }
        }

        [HttpGet("InterviewMeetingList")]
        public IActionResult InterviewMeetingList()
        {
            try
            {
                var res = _admin.InterviewMeetingList();
                if (res != null) { return Ok(new { Status = 200, Data=res ,  Message = "Successful" }); }
                else { return Ok(new { Status = 400, Message = "Something went wrong" }); }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = 500,
                    Message = "Internal Server Error",
                    Error = ex.Message
                });
            }
        }
        
        [HttpGet("InterviewMeetingListByJyotishId")]
        public IActionResult InterviewMeetingListByJyotishId(int JyotishId)
        {
            try
            {
                var res = _admin.InterviewMeetingListByJyotishId(JyotishId);
                if (res != null) { return Ok(new { Status = 200, Data=res ,  Message = "Successful" }); }
                else { return Ok(new { Status = 400, Message = "Something went wrong" }); }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = 500,
                    Message = "Internal Server Error",
                    Error = ex.Message
                });
            }
        }

        [HttpGet("StartAndEndInterviewTime")]
        public IActionResult StartAndEndInterviewTime(int SlotBookingId, bool Time)
        {
            try
            {
                var response = _admin.StartAndEndInterviewTime(SlotBookingId, Time);
                if(response) { return Ok(new { Status = 200, Message = "Successful" }); }
                else { return Ok(new { Status = 400, Message = "Something went wrong" }); }

            }
            catch(Exception ex)
            {
                return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex.Message });
            }
        }
        [HttpGet("startAndEndDateofMeating")]
        public IActionResult startAndEndDateofMeating(int SlotBookingId)
        {
            try
            {
                var response = _admin.startAndEndDateofMeating(SlotBookingId);
                if(response!=null) { return Ok(new { Status = 200, Message = "data retrieved", data=response }); }
                else { return Ok(new { Status = 400, Message = "Something went wrong" }); }

            }
            catch(Exception ex)
            {
                return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex.Message });
            }
        }

        [HttpPost("AddEmployeeInterviewFeedback")]
        public IActionResult AddEmployeeInterviewFeedback(EmployeeInterviewFeedbackViewModel data)
        {
            try
            {
                var res = _admin.AddEmployeeInterviewFeedback(data);
                if (res) { return Ok(new { Status = 200, Message = "Successful" }); }
                else { return Ok(new { Status = 400, Message = "Something went wrong" }); }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = 500,
                    Message = "Internal Server Error",
                    Error = ex.Message
                });
            }
        }

        [HttpGet("EmployeeInterviewFeedbackList")]
        public IActionResult EmployeeInterviewFeedbackList()
        {
            try
            {
                var Records = _admin.EmployeeInterviewFeedbackList();
                if (Records == null) { return Ok(new { Status = 400, Message = "Something went wrong." }); }
                else { return Ok(new { Status = 200, Data = Records, Message = "Successful" }); }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 500, Message = "Internal Server Error.", Error = ex.Message });
            }
        }
    }
}
