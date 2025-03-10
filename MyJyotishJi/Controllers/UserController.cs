﻿using BusinessAccessLayer.Abstraction;
using DataAccessLayer.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Host.Mef;
using ModelAccessLayer.Models;
using ModelAccessLayer.ViewModels;
using System.Collections.Generic;

namespace MyJyotishGApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Policy = "Policy4")]
	public class UserController : ControllerBase
	{

		private readonly IUserServices _services;
		private readonly IWebHostEnvironment _environment;
		private readonly IAdminServices _admin;
		public UserController(IUserServices services, IWebHostEnvironment environment,IAdminServices admin)
		{
			_services = services;
			_environment = environment;
			_admin = admin;
		}

		[AllowAnonymous]
		[HttpGet("TopAstrologer")]
		public IActionResult TopAstrologer()
		{
			try
			{
				var records = _services.TopAstrologer();
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

		[HttpGet("GetAllUserAttachmentsByAppointmentId")]
		public IActionResult GetAllUserAttachmentsByAppointment(int Id, int memberId)
		{
			try
			{
				var result = _services.GetAttachmentByAppointment(Id, memberId);

				if (result == null || result.Count == 0)
				{
					return Ok(new { Status = 404, Message = "No attachments found " });
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
				fm.country = String.IsNullOrEmpty(httpRequest.Form["country"]) ? 0 : Convert.ToInt32(httpRequest.Form["country"]);

				fm.city = String.IsNullOrEmpty(httpRequest.Form["city"]) ? 0 : Convert.ToInt32(httpRequest.Form["city"]);

				fm.state = String.IsNullOrEmpty(httpRequest.Form["state"]) ? 0 : Convert.ToInt32(httpRequest.Form["state"]);
				fm.activity = String.IsNullOrEmpty(httpRequest.Form["activity"]) ? 0 : Convert.ToInt32(httpRequest.Form["activity"]);

				fm.rating = String.IsNullOrEmpty(httpRequest.Form["rating"]) ? 0 : Convert.ToInt32(httpRequest.Form["rating"]);
				fm.expertise = String.IsNullOrEmpty(httpRequest.Form["expertise"]) ? 0 : Convert.ToInt32(httpRequest.Form["expertise"]);

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
			return Ok(new { status = 200, message = "data retrieved", data = res });
		}

		[AllowAnonymous]
		[HttpGet("SliderImageList")]
		public IActionResult SliderImageList()
		{
			try
			{
				var records = _services.SliderImageList();
				if (records == null)
				{
					return Ok(new { Status = 404, Message = "No Image found" });
				}
				else
				{
					return Ok(new { Status = 200, Data = records, Message = "Slider List" });
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
				 return Ok(new { Status = 200, message = "Succussfull", data = record }); 
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
			try
			{
				var result = _services.BookAppointment(model);
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



		[HttpPost("UpdateProfiles")]
		public IActionResult UpdateProfile(UserUpdateViewModel model)
		{
			try
			{

				//var httpRequest = HttpContext.Request;
				//UserUpdateViewModel model = new UserUpdateViewModel
				//{
				//	Id = Convert.ToInt32(httpRequest.Form["id"]),
				//	Mobile = httpRequest.Form["mobile"],
				//	Name = httpRequest.Form["name"],
				//	Gender = httpRequest.Form["gender"],
				//	DoB = Convert.ToDateTime(httpRequest.Form["doB"]),
				//	PlaceOfBirth = httpRequest.Form["placeOfBirth"],
				//	TimeOfBirth = httpRequest.Form["timeOfBirth"],
				//	CurrentAddress = httpRequest.Form["currentAddress"],
				//	State = Convert.ToInt32(httpRequest.Form["state"]),
				//	City = Convert.ToInt32(httpRequest.Form["city"]),
				//	Pincode = Convert.ToInt32(httpRequest.Form["pincode"]),
				//	ProfilePictureUrl = httpRequest.Form.Files["profilePictureUrl"],

				//};

				string? path = _environment.ContentRootPath;
				var result = _services.UpdateProfile(model, path);
				if (!result)
				{
					return Ok(new { Status = 404, Message = "Something went wrong" });
				}

				else { return Ok(new { Status = 200, Data = "Successful" }); }

			}
			catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
		}

		[HttpGet("UpcommingAppointment")]
		public IActionResult UpcommingAppointment(int Id)
		{
			try
			{
				var result = _services.UpcommingAppointment(Id);
				if (result == null)
				{
					return Ok(new { Status = 404, Message = "User Not Found" });
				}

				else { return Ok(new { Status = 200, Data = result }); }

			}
			catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }

		}

		[HttpGet("AppointmentHistory")]
		public IActionResult AppointmentHistory(int Id)
		{
			try
			{
				var result = _services.AppointmentHistory(Id);
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
				var checkUserServiceStatus = _services.getUserserviceStatus(uv.userId);
				if (checkUserServiceStatus)
				{
                    return Ok(new { Status = 404, Message = "You already use another service please try after some time" });

                }

                var checkAmountValidation = _admin.getAppointmentCharges();
				if (checkAmountValidation.charges != uv.WalletAmount)
				{
                    return Ok(new { Status = 404, Message = "Some error occured" });

                }

                var Result = _services.PurchaseWithUserWallets(uv);
				if (Result == "Successful")
				{
					WalletHistoryViewmodel js = new WalletHistoryViewmodel
					{
						UId = (int)uv.userId,
						JId =null,
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

		[AllowAnonymous]
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

		[HttpPost("AddRating")]
		public IActionResult AddRating(JyotishRatingViewModel data)
		{
			try
			{
				var result = _services.AddRating(data);
				if (result == "Successful")
				{
					return Ok(new { Status = 200, Message = "Record Added Successfully." });
				}
				else if (result == "Internal Server Error")
				{
					return Ok(new { Status = 500, Message = result });
				}
				else
				{
					return Ok(new { Status = 400, Message = result });
				}

			}
			catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
		}
		[AllowAnonymous]
		[HttpGet("JyotishRatingList")]
		public IActionResult JyotishRatingList(int Id)
		{
			try
			{
				var record = _services.JyotishRatingList(Id);
				if (record == null)
				{
					return Ok(new { Status = 500, Message = "Internal Server Error" });
				}
				else
				{
					return Ok(new { Status = 200, Data = record, Message = "Jyotish Rating List" });
				}
			}
			catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
		}

		[HttpGet("IsUserValidForRating")]
		public IActionResult IsUserValidForRating(int UserId, int JyotishId)
		{
			try
			{
				var record = _services.IsUserValidForRating(UserId, JyotishId);
				if (record == "Invalid Id")
				{
					return Ok(new { Status = 500, Message = record });
				}
				else
				{
					return Ok(new { Status = 200, Data = record });
				}
			}
			catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
		}

		[HttpPost("AddUserServiceRecord")]
		public IActionResult AddUserServiceRecord(UserServiceRecordViewModel data)
		{
			try
			{
				var result = _services.AddUserServiceRecord(data);
				if (result)
				{
					return Ok(new { Status = 200, Message = "Record Added Successfully." });
				}
				else
				{
					return Ok(new { Status = 400, Message = "Something went wrong" });
				}

			}
			catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
		}

		[HttpGet("GetUserDataForService")]
		public IActionResult GetUserDataForService(int Id)
		{
			try
			{
				var record = _services.GetUserDataForService(Id);
				if (record == null)
				{
					return Ok(new { Status = 500, Message = "Record not found" });
				}
				else
				{
					return Ok(new { Status = 200, Data = record });
				}
			}
			catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
		}

		[HttpGet("GetAllKundaliMatchingRecord")]
		public IActionResult GetAllKundaliMatchingRecord(int Id)
		{
			try
			{
				var record = _services.GetAllKundaliMatchingRecord(Id);
				if (record == null)
				{
					return Ok(new { Status = 500, Message = "Record not found" });
				}
				else
				{
					return Ok(new { Status = 200, Data = record });
				}
			}
			catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
		}
		[HttpGet("GetLatestKundaliRecord")]
		public IActionResult GetLatestKundaliRecord(int Id)
		{
			try
			{
				var record = _services.GetLatestKundaliRecord(Id);
				if (record == null)
				{
					return Ok(new { Status = 500, Message = "Record not found" });
				}
				else
				{
					return Ok(new { Status = 200, Data = record });
				}
			}
			catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
		}

		[HttpDelete("DeleteKundaliRecord")]
		public IActionResult DeleteKundaliRecord(int Id)
		{
			try
			{
				var record = _services.DeleteKundaliRecord(Id);
				if (!record)
				{
					return Ok(new { Status = 500, Message = "Record not found" });
				}
				else
				{
					return Ok(new { Status = 200, Data = record });
				}
			}
			catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
		}

		[HttpPost("AddKundaliMatchingRecord")]
		public IActionResult AddKundaliMatchingRecord(List<KundaliMatchingViewModel> DataList)
		{
			try
			{
				var record = _services.AddKundaliMatchingRecord(DataList);
				if (!record)
				{
					return Ok(new { Status = 500, Message = "Something went wrong" });
				}
				else
				{
					return Ok(new { Status = 200, Data = "Successful" });
				}
			}
			catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
		}

		[AllowAnonymous]
		[HttpGet("GetTimezone")]
		public IActionResult GetTimezone(string Country)
		{
			try
			{
				var record = _services.GetTimezone(Country);
				if (record == null)
				{
					return Ok(new { Status = 400, Message = "Record not found" });
				}
				else
				{
					return Ok(new { Status = 200, Data = record });
				}
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


		//get all pooja list
		[AllowAnonymous]
		[HttpGet("getAllPoojaList")]
		public IActionResult getAllPoojaList()
		{
			var res = _services.getAllPoojaList();
			return Ok(new { status = 200, message = "data retrieved", data = res });
		}
		[AllowAnonymous]
		[HttpGet("getPoojaDetailByPoojaId")]
		public IActionResult getPoojaDetailByPoojaId(int id)
		{
			var res = _services.getPoojaDetailByPoojaId(id);
			return Ok(new { status = 200, message = "data retrieved", data = res });
		}

		[HttpPost("BookPooja")]
		public IActionResult BookPooja()
		{
			try
			{
				var httpRequest = HttpContext.Request;
				BookedPoojaViewModel book = new BookedPoojaViewModel
				{
					PoojaId = Convert.ToInt32(httpRequest.Form["poojaId"]),
					userId = Convert.ToInt32(httpRequest.Form["userId"]),
					jyotishId = Convert.ToInt32(httpRequest.Form["jyotishId"]),
					PoojaDate = Convert.ToDateTime(httpRequest.Form["poojadate"]),
				};
				var res = _services.BookPooja(book);
				if (res)
				{
					return Ok(new { status = 200, message = "your pooja booked successfully. after some times our astrologer try to make contact with you" });
				}
				else
				{
					return Ok(new { status = 500, message = "Something went wrong or may be already Booked" });

				}
			}
			catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
		}

		[HttpGet("GetJyotishByPoojaType")]
		public IActionResult GetJyotishByPoojaType(int PoojaTyId)
		{
			var res = _services.getJyotishRecordByPoojaType(PoojaTyId);
			return Ok(new { status = 200, message = "data retrieve", data = res });

		}
		[HttpGet("getAllmembers")]
		public IActionResult getAllmembers(int userId)
		{
			var res = _services.getAllmembers(userId);
			return Ok(new { status = 200, message = "data retrieve", data = res });

		}
		[HttpGet("getAllAppointmentBymemebersanduser")]
		public IActionResult getAllAppointmentBymemebersanduser(int memberId, int userId, int jyotishId)
		{
			var res = _services.getAllAppointmentBymemebersanduser(memberId, userId, jyotishId);
			return Ok(new { status = 200, message = "data retrieve", data = res });

		}
		[HttpGet("getjyotishByuserAppointment")]
		public IActionResult getjyotishByuserAppointment(int userId, int memberId)
		{
			var res = _services.getjyotishByuserAppointment(userId, memberId);
			return Ok(new { status = 200, message = "data retrieve", data = res });

		}


		[HttpGet("applyChargesForCall")]
		public IActionResult ChargesForCallService(int userId, int jyotishId, int totalSecond)

		{
			try
			{
				var jyotishCallCharges = _services.getJyotishCallServicesCharges(jyotishId);
				var totalMinute = Math.Ceiling(Math.Abs((float)totalSecond / 60));
				var totalAmount = jyotishCallCharges * totalMinute;
				var applyCharges = _services.ApplyChargesFromUserWalletForService(userId, Convert.ToInt32(totalAmount), "Call with astrologers", jyotishId);
				if (applyCharges)
				{
					return Ok(new { status = 200, message = "Charges Apply Successfully" });
				}
				else
				{
					return Ok(new { status = 500, message = "something went wrong" });
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		[HttpGet("changeUserActiveStatus")]
		public IActionResult changeUserServiceStatus(int userId, bool status)

		{
			try
			{
				var jyotishCallCharges = _services.changeUserServiceStatus(userId, status);
				
				if (jyotishCallCharges)
				{
					return Ok(new { status = 200, message = "changes made successfully Successfully" });
				}
				else
				{
					return Ok(new { status = 500, message = "something went wrong" });
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}



		[HttpGet("jyotishChatCharges")]
		public IActionResult GetJyotishChatCharges(int jyotishId)
		{
			try
			{
				var res = _services.getJyotishServicesCharges(jyotishId);
				return Ok(new { status = 200, message = "data retrived", chatCharges = res });
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		[HttpGet("jyotishCallCharges")]
		public IActionResult GetJyotishCallCharges(int jyotishId)
		{
			try
			{
				var res = _services.getJyotishCallServicesCharges(jyotishId);
				return Ok(new { status = 200, message = "data retrived", callCharges = res });
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		
		[HttpGet("getservicePriceAndWalletAmount")]
		public IActionResult GetJyotishChatCharges(int jyotishId,int userId,string type)
		{
			try
			{
				dynamic jyotishserviceCharges = 0;
				if (type == "call")
				{
					jyotishserviceCharges = _services.getJyotishCallServicesCharges(jyotishId);
				}
				else
				{
					jyotishserviceCharges = _services.getJyotishServicesCharges(jyotishId);
				}

				var userWallet = _services.GetWallet(userId);
				return Ok(new { status = 200, message = "data retrived", type=type,Charges= jyotishserviceCharges, userWallet=userWallet });
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[HttpGet("getAppointmentCharges")]
		public IActionResult getAppointemntCharges()
		{
			var res = _services.getAppointmentCharges();
			return Ok(new { status = 200, message = "data retrieved", charges = res });

        }
		[HttpGet("GettopTenWalletHistory")]
		public IActionResult GettopTenWalletHistory(int userId)
		{
			var res = _services.GettopTenWalletHistory(userId);
			return Ok(new { status = 200, message = "data retrieved", charges = res });

        }
		[AllowAnonymous]
		[HttpGet("AdvertisementBanner")]
		public IActionResult AdvertisementBanner()
		{
			var res = _services.AdvertisementBanner();
			return Ok(new { status = 200, message = "data retrieved", data = res });

        }
		[AllowAnonymous]
		[HttpGet("GetTopOneAdvertisementBanner")]
		public IActionResult GetTopOneAdvertisementBanner()
		{
			var res = _services.GetTopOneAdvertisementBanner();
			return Ok(new { status = 200, message = "data retrieved", data = res });

        }
		
		[HttpGet("getUserServiceRevordForUser")]
		public IActionResult getUserServiceRevordForUser(int userId)
		{
			var res = _services.getUserServiceRevordForUser(userId);
			return Ok(new { status = 200, message = "data retrieved", data = res });

        }
		[AllowAnonymous]
        [HttpPost("AddContactUs")]
        public IActionResult AddContactUs(ContactUsServices cs)
        {
            var res = _services.AddContactUs(cs);
            if (res)
            {
                return Ok(new { status = 200, message = "Message added successfully" });

            }
            return Ok(new { status = 500, message = "some error occured" });

        }
       
    }
}
