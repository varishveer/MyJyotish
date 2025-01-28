using BusinessAccessLayer.Abstraction;
using BusinessAccessLayer.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
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
			return Ok(new { Status = 200, data = records });
		}
		[HttpGet("GetTodayAppointment")]
		public IActionResult GetTodayAppointment(int Id)
		{
			var records = _jyotish.GetTodayAppointment(Id);
			return Ok(new { Status = 200, data = records });
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
				else { return Ok(new { Status = 200, Data = result, message = "Successful" }); }
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


		[HttpGet("updateArrivedClient")]
		public IActionResult UpdateArrivedClient(int appointmentId, int jyotishId)
		{
			try
			{
				var result = _jyotish.updateArrivedClient(appointmentId, jyotishId);

				if (!result)
				{
					return Ok(new { Status = 400, Message = "Invalid Data or maybe some error internal error occured" });
				}

				else if (result) { return Ok(new { Status = 200, message = "Client Marked Arrived" }); }
				else { return Ok(new { Status = 500, message = "Internal server error" }); }
			}
			catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
		}

		[HttpPost("AddClientMembers")]

		public IActionResult AddClientMembers(ClientMembersViewModel model)
		{
			try
			{
				var result = _jyotish.AddClientMembers(model);
				if (result)
				{
					return Ok(new { Status = 200, Message = "Member Added Successfully" });
				}
				else
				{
					return Ok(new { Status = 400, Message = "something went wrong or maybe appointment is invalid" });

				}
			}
			catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
		}

		[HttpGet("GetClientMembers")]
		public IActionResult GetClientMembers(int Id, int jyotishId)
		{
			try
			{
				var result = _jyotish.getClientMembers(Id, jyotishId);
				if (result != "not found")
				{
					return Ok(new { Status = 200, Message = "Retrieve Successfully", Data = result });
				}
				else
				{
					return Ok(new { Status = 400, Message = "something went wrong or maybe appointment is invalid" });
				}
			}
			catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
		}


		[HttpGet("GetAllUpcommingAppointment")]
		public IActionResult GetAllUpcommingAppointment(int jyotishId)
		{
			try
			{
				var result = _jyotish.GetAllUpcommingAppointment(jyotishId);
				return Ok(new { Status = 200, Data = result, Message = "Data Retrieve Successfully" });
			}
			catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
		}

        [HttpGet("GetUpcommingAppointmentById")]
        public IActionResult GetUpcommingAppointmentById(int appointmentId)
        {
            try
            {
                var result = _jyotish.GetUpcommingAppointmentById(appointmentId);
                return Ok(new { Status = 200, Data = result, Message = "Data Retrieve Successfully" });
            }
            catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
        }

        [HttpGet("GetAllAppointmentHistory")]
		public IActionResult GetAllAppointmentHistroy(int jyotishId)
		{
			try
			{
				var result = _jyotish.GetAllAppointmentHistory(jyotishId);

				return Ok(new { Status = 200, Data = result, Message = "Data Retrieve Successfully" });
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
		public IActionResult UpdateProfile(JyotishProfileUpdateViewModal model)
		{
			try
			{
				var Result = _jyotish.UpdateProfile(model);
				if (Result == "Jyotish Not Found") { return Ok(new { Status = 404, Message = Result }); }
				else if (Result == "Invalid Email") { return Ok(new { Status = 404, Message = Result }); }
				else if (Result == "Successful") { return Ok(new { Status = 200, Message = Result }); }
				else { return Ok(new { Status = 500, Message = Result }); }


			}
			catch (Exception ex)
			{
				return StatusCode(500, new { Status = 500, Message = ex.Message, Error = ex });
			}
		}

		[HttpPost("AddJyotishVideo")]
public IActionResult AddJyotishVideo(JyotishVideosViewModel model)
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
public IActionResult AddJyotishGallery(JyotishGalleryViewModel model)
{
	try
	{
		var result = _jyotish.AddJyotishGallery(model);
		if (result == "invalid data") return BadRequest(new { Status = 400, Message = result });
		else if (result == "Successful") return Ok(new { Status = 200, Message = result });
		else return Ok( new { Status = 500, Message = result });
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
		// Method to fetch Jyotish gallery images by Jyotish ID
[HttpGet("JyotishVideos")]
public IActionResult JyotishVideos(int id)
{
	try
	{
		var gallery = _jyotish.JyotishVideos(id);
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
			return Ok(new { status = 200, data = records });

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

[HttpPost("PurchaseWithJyotishWallets")]
public IActionResult PurchaseWithJyotishWallets(JyotishWalletViewmodel jw)
{
	try
	{
		var Result = _jyotish.PurchaseWithJyotishWallets(jw);
		if (Result == "Successful")
		{

			WalletHistoryViewmodel js = new WalletHistoryViewmodel
			{
				JId = (int)jw.jyotishId,
				amount = (long)jw.WalletAmount,
				PaymentAction = "Debit",
				PaymentStatus = "success",
				PaymentFor = jw.paymentfor
			};
			var historyres = _jyotish.AddWalletHistory(js);

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

[HttpGet("GetWallet")]
public IActionResult GetWallet(int JyotishId)
{
	try
	{
		var Result = _jyotish.GetWallet(JyotishId);
		if (Result != null)
		{ return Ok(new { Status = 200, Data = Result }); }
		else
		{ return Ok(new { Status = 404, Message = "Some error occured" }); }
	}
	catch (Exception ex)
	{ return StatusCode(500, new { Status = 500, Message = "Internal Server Error ", Error = ex }); }
}

[HttpPost("AddWalletHistory")]
public IActionResult AddWalletHistory(WalletHistoryViewmodel jw)
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

		else if (Result == "Invalid Jyotish")
		{ return Ok(new { Status = 409, Message = Result }); }
		else if (Result == "Invalid Data")
		{ return Ok(new { Status = 409, Message = Result }); }
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
		var result = _jyotish.RemoveSlotWithskipDates(model);
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
		else if (Result == "Slot Has Been Booked It Can't Be Updated.")
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
		else if (Result == "Invalid Jyotish")
		{ return Ok(new { Status = 409, Message = Result }); }
		else if (Result == "Invalid Data")
		{ return Ok(new { Status = 409, Message = Result }); }
		else if (Result == "Data Not Saved")
		{ return Ok(new { Status = 500, Message = Result }); }
		else if (Result == "Successful")
		{ return Ok(new { Status = 200, Message = Result }); }
		else if (Result == "Slot Has Been Booked It Can't Be Updated.")
		{ return Ok(new { Status = 500, Message = Result }); }
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
		if (Result.Count == 0)
		{ return Ok(new { Status = 400, Message = "Data Not Found" }); }


		else
		{ return Ok(new { Status = 200, data = Result, Message = "Successful" }); }
	}
	catch (Exception ex)
	{ return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
}
[HttpGet("GetAllBookedAppointmentSlot")]
public IActionResult GetAllbookedAppointment(int Id)
{
	try
	{
		var Result = _jyotish.GetAllbookedAppointment(Id);
		if (Result.Count == 0)
		{ return Ok(new { Status = 400, Message = "Data Not Found" }); }
		else
		{ return Ok(new { Status = 200, data = Result, Message = "Successful" }); }
	}
	catch (Exception ex)
	{ return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }
}

[HttpPost("UpgradePackages")]
public IActionResult UpgradePackages(packages packages)
{
	try
	{
		var res = _jyotish.upgradePackages(packages);
		if (res)
		{
			return Ok(new { Status = 200, Message = "Plan Puchase Successfully" });
		}
		else
		{
			return Ok(new { Status = 500, Message = "Something went wrong" });

		}
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
		else if (Result == "Record Already Present.")
		{ return Ok(new { Status = 400, Message = Result }); }

		else if (Result == "Successful")
		{ return Ok(new { Status = 200, Message = "Problem solution added successfully" }); }
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


		if (result == null)
		{
			return Ok(new { Status = 400, Message = "Data Not Found" });
		}

		if (result == null)
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



[HttpGet("GetProblemSolutionDetail")]
public IActionResult GetProblemSolutionDetail(int appointmentId)
{
	try
	{

		var result = _jyotish.GetProblemSolutionDetail(appointmentId);



		return Ok(new { Status = 200, Data = result, Message = "Successful" });
	}
	catch (Exception ex)
	{

		return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex.Message });
	}
}
[HttpGet("GetAllProblemSolution")]
public IActionResult GetAllProblemSolution(int Id)
{
	try
	{
		var Result = _jyotish.GetAllProblemSolution(Id);
		if (Result.Count == 0 || Result == null)
		{ return Ok(new { Status = 404, Message = "Data Not Found" }); }

		else
		{ return Ok(new { Status = 200, Data = Result, Message = "Successful" }); }
	}
	catch (Exception ex)
	{ return StatusCode(500, new { Status = 500, Message = "Internal Server Error ", Error = ex }); }

}

[HttpGet("GetAllProblemSolutionByUser")]
public IActionResult GetAllProblemSolutionByUser(int JyotishId, int UId)
{
	try
	{
		var Result = _jyotish.GetAllProblemSolutionByUser(JyotishId, UId);
		if (Result.Count == 0 || Result == null)
		{ return Ok(new { Status = 404, Message = "Data Not Found" }); }

		else
		{ return Ok(new { Status = 200, Data = Result, Message = "Successful" }); }
	}
	catch (Exception ex)
	{ return StatusCode(500, new { Status = 500, Message = "Internal Server Error ", Error = ex }); }

}

[HttpPost("UpdateProblemSolution")]
public IActionResult UpdateProblemSolution()
{
	try
	{
		var httpRequest = HttpContext.Request;
		ProblemSolutionViewModel model = new ProblemSolutionViewModel
		{
			Id = Convert.ToInt32(httpRequest.Form["Id"]),
			Problem = httpRequest.Form["problem"].ToString().Split(','),
			Solution = httpRequest.Form["solution"],
		};
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
		var titleList = httpRequest.Form["title"];
		var member = Convert.ToInt32(httpRequest.Form["member"]);

		// Collect Image URLs
		var images = httpRequest.Form.Files["attachment"];
		if (images != null)
		{
			// Ensure directory exists for file upload
			string uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Jyotish/ProblemSolution");
			if (!Directory.Exists(uploadsFolderPath))
			{
				Directory.CreateDirectory(uploadsFolderPath);
			}

			// Process each uploaded file

			string uniqueString = Guid.NewGuid().ToString("N").Substring(0, 10);
			var filename = uniqueString + images.FileName;
			string filePath = Path.Combine(uploadsFolderPath, filename);
			string accessPath = Path.Combine("/Images/Jyotish/ProblemSolution", uniqueString + images.FileName);

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				images.CopyTo(stream);
			}
			// Prepare ViewModel
			JyotishUserAttachmentViewModel model = new JyotishUserAttachmentViewModel
			{
				UserId = int.Parse(httpRequest.Form["userId"]),
				JyotishId = int.Parse(httpRequest.Form["jyotishId"]),
				appointmentId = int.Parse(httpRequest.Form["appointmentId"]),
				Title = titleList,
				ImageUrl = accessPath,
				member = member
			};
			var result = _jyotish.AddUserAttachment(model);
			if (result == "Invalid data provided for the attachment.")
			{
				return Ok(new { Status = 400, Message = result });
			}
			else if (result == "Failed to save attachments.")
			{
				return Ok(new { Status = 500, Message = result });
			}
			else if (result == "Successful")
			{
				return Ok(new { Status = 200, Message = "Attachment Added Successfully" });
			}
			return Ok(new { Status = 500, Message = result });
		}
		else
		{
			return Ok(new { Status = 400, Message = "Attachment file are required" });

		}

		// Save to Database
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
[HttpGet("GetAllUserAttachmentsByAppointmentId")]
public IActionResult GetAllUserAttachmentsByAppointment(int Id, int memberId)
{
	try
	{
		var result = _jyotish.GetAttachmentByAppointment(Id, memberId);
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


[HttpGet("DeleteUserAttachment")]
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
			return Ok(new { Status = 200, Message = "Attachment remove successfully" });
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


[HttpGet("AppointmentSlotDetails")]
public IActionResult AppointmentSlotDetails(int Id)
{
	try
	{
		var result = _jyotish.AppointmentSlotDetails(Id);

		if (result == null)
		{
			return Ok(new { Status = 400, Message = "No appointment found for the provided Jyotish Id." });
		}
		if (result == null)
		{
			return Ok(new { Status = 400, Message = "No appointment found for the provided Jyotish Id." });
		}

		return Ok(new { Status = 200, Data = result, Message = "Successful" });
	}
	catch (Exception ex)
	{
		return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex.Message });
	}
}


[HttpGet("SkipDateList")]
public IActionResult SkipDateList(int Id)
{
	try
	{
		var result = _jyotish.SkipDateList(Id);

		if (result == null || result.Count == 0)
		{
			return Ok(new { Status = 400, Message = "No skip Date found for the provided Jyotish Id." });
		}
		if (result == null || result.Count == 0)
		{
			return Ok(new { Status = 400, Message = "No skip Date found for the provided Jyotish Id." });
		}

		return Ok(new { Status = 200, Data = result, Message = "Successful" });
	}
	catch (Exception ex)
	{
		return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex.Message });
	}
}

[HttpGet("NotificationData")]
public IActionResult NotificationData(int Id)
{
	try
	{
		var result = _jyotish.NotificationData(Id);

		if (result == null || result.Count == 0)
		{
			return Ok(new { Status = 400, Message = "No Data found for the provided Jyotish Id." });
		}
		if (result == null || result.Count == 0)
		{
			return Ok(new { Status = 400, Message = "No Data found for the provided Jyotish Id." });
		}

		return Ok(new { Status = 200, Data = result, Message = "Successful" });
	}
	catch (Exception ex)
	{
		return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex.Message });
	}
}

[AllowAnonymous]
[HttpGet("LayoutData")]
public IActionResult LayoutData(int Id)
{
	try
	{
		var result = _jyotish.LayoutData(Id);
		if (result == null)
		{
			return Ok(new { Status = 404, Message = "Not Found" });
		}

		else { return Ok(new { Status = 200, Data = result }); }

	}
	catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }

}

[HttpGet("getPlan")]
public IActionResult GetPlan(int Id)
{
	try
	{
		var result = _jyotish.getPlan(Id);
		if (result == null)
		{
			return Ok(new { Status = 404, Message = "Not Found" });
		}

		else { return Ok(new { Status = 200, Data = result }); }

	}
	catch (Exception ex) { return StatusCode(500, new { Status = 500, Message = "Internal Server Error", Error = ex }); }

}

[HttpGet("GetAllFeatures")]
public IActionResult GetAllFeatures()
{
	try
	{
		var Result = _jyotish.GetAllFeatures();
		if (Result == null)
		{ return Ok(new { Status = 404, Message = "Data Not Found" }); }

		else
		{ return Ok(new { Status = 200, Data = Result, Message = "Successful" }); }


	}

	catch { return StatusCode(500, new { Status = 500, Message = "Internal Server Error " }); }

}

[HttpGet("BookMark")]
public IActionResult BookMark(int appointmentId)
{
	try
	{
		var Result = _jyotish.BookMark(appointmentId);
		if (!Result)
		{ return Ok(new { Status = 404, Message = "Changes not applied" }); }

		else
		{ return Ok(new { Status = 200, Data = Result, Message = "Changes applied successful" }); }


	}

	catch { return StatusCode(500, new { Status = 500, Message = "Internal Server Error " }); }

}


//country code
[AllowAnonymous]
[HttpGet("countryCode")]
public IActionResult countryCode(int country)
{
	try
	{
		var res = _jyotish.countryCode(country);
		return Ok(new { Status = 200, message = "data retrieved", code = res });
	}
	catch { return StatusCode(500, new { Status = 500, Message = "Internal Server Error " }); }
}
[HttpGet("purchaseWithReadmCode")]
public IActionResult purchaseWithReadmCode(string redeamCode, int jyotishId, int planId)
{
	try
	{
		var res = _jyotish.purchaseWithReadmCode(redeamCode, jyotishId, planId);
		return Ok(new { Status = 200, message = "data retrieved", amount = res });
	}
	catch { return StatusCode(500, new { Status = 500, Message = "Internal Server Error " }); }
}

[HttpGet("JyotishDashboardData")]
public IActionResult JyotishDashboardData(int Id)
{
	try
	{
		var record = _jyotish.JyotishDashboardData(Id);
		if (record == null)
		{
			return Ok(new { Status = 400, Message = "Record Not Found" });
		}
		else
		{
			return Ok(new { Status = 200, Data = record, Message = "Successful" });
		}
	}
	catch { return StatusCode(500, new { Status = 500, Message = "Internal Server Error " }); }
}


//send request for redeem code
[HttpPost("SendRedeemCodeRequest")]
public IActionResult SendRequest(RedeemCodeRequestViewModel model)
{
	try
	{
		var res = _jyotish.SendRequest(model);
		if (res)
		{
			return Ok(new { status = 200, message = "Request Send Successfully" });
		}
		else
		{
			return Ok(new { status = 501, message = "Your request already has been registered" });

		}
	}
	catch
	{
		return StatusCode(500, new { Status = 500, Message = "Internal Server Error " });
	}
}


[HttpPost("AddAppointmentBookmark")]
public IActionResult AddAppointmentBookmark(AppointmentBookmarkViewModal model)
{
	try
	{
		var res = _jyotish.AddAppointmentBookmark(model);
		if (res)
		{
			return Ok(new { status = 200, message = "Request Added Successfully" });
		}
		else
		{
			return Ok(new { status = 501, message = "Something went wrong" });

		}
	}
	catch
	{
		return StatusCode(500, new { Status = 500, Message = "Internal Server Error " });
	}
}


[HttpDelete("DeleteAppointmentBookmark")]
public IActionResult DeleteAppointmentBookmark(int Id)
{
	try
	{
		var res = _jyotish.DeleteAppointmentBookmark(Id);
		if (res)
		{
			return Ok(new { status = 200, message = "Request Deleted Successfully" });
		}
		else
		{
			return Ok(new { status = 501, message = "Your request already has been registered" });

		}
	}
	catch
	{
		return StatusCode(500, new { Status = 500, Message = "Internal Server Error " });
	}
}
				
			
			
		[HttpGet("getRedeemCode")]
public IActionResult getRedeemCode(int jyotishId)
{
	var res = _jyotish.getRedeemCode(jyotishId);

	return Ok(new { status = 200, message = "data retrieved", data = res });

}


//handle pooja
[HttpPost("AddJyotishPooja")]
public IActionResult AddJyotishPooja()
{
	try
	{
		var httpRequest = HttpContext.Request;
		JyotishPoojaViewModel model = new JyotishPoojaViewModel
		{
			poojaType = Convert.ToInt32(httpRequest.Form["poojatype"]),
			JyotishId = Convert.ToInt32(httpRequest.Form["jyotishId"]),
			amount = Convert.ToInt32(httpRequest.Form["amount"])
		};

		var res = _jyotish.AddJyotishPooja(model);
		if (res)
		{
			return Ok(new { status = 200, message = "Record Added Successfully" });
		}
		else
		{
			return Ok(new { status = 500, message = "Something went wrong or maybe Pooja Type already selected" });

		}
	}
	catch { return StatusCode(500, new { Status = 500, Message = "Internal Server Error " }); }
}
//handle pooja
[HttpPost("UpdateJyotishPooja")]
public IActionResult UpdateJyotishPooja()
{
	try
	{
		var httpRequest = HttpContext.Request;
		JyotishPoojaViewModel model = new JyotishPoojaViewModel
		{
			poojaType = Convert.ToInt32(httpRequest.Form["poojatype"]),
			JyotishId = Convert.ToInt32(httpRequest.Form["jyotishId"]),
			Id = Convert.ToInt32(httpRequest.Form["Id"]),
			amount = Convert.ToInt32(httpRequest.Form["amount"])
		};

		var res = _jyotish.UpdateJyotishPooja(model);
		if (res)
		{
			return Ok(new { status = 200, message = "Record Updated Successfully" });
		}
		else
		{
			return Ok(new { status = 500, message = "Something went wrong or maybe Pooja Type already selected" });

		}
	}
	catch { return StatusCode(500, new { Status = 500, Message = "Internal Server Error " }); }
}


[HttpGet("GetAppointmentBookmark")]
public IActionResult GetAppointmentBookmark(int Id)
{
	try
	{
		var record = _jyotish.GetAppointmentBookmark(Id);
		if (record == null)
		{
			return Ok(new { Status = 400, Message = "Record Not Found" });
		}
		else
		{
			return Ok(new { Status = 200, Data = record, Message = "Successful" });
		}
	}
	catch { return StatusCode(500, new { Status = 500, Message = "Internal Server Error " }); }
}

[HttpGet("getJyotishPoojaList")]
public IActionResult getJyotishPoojaList(int Id)
{
	var res = _jyotish.getJyotishPoojaList(Id);
	return Ok(new { status = 200, message = "data retrieved", data = res });
}
[HttpGet("removeJyotishPooja")]
public IActionResult removeJyotishPooja(int poojaId)
{
	try
	{
		var res = _jyotish.removeJyotishPooja(poojaId);
		if (res)
		{
			return Ok(new { status = 200, message = "Record Deleted Successfully" });
		}
		else
		{
			return Ok(new { status = 500, message = "Something went wrong" });

		}
	}
	catch { return StatusCode(500, new { Status = 500, Message = "Internal Server Error " }); }
}
		[HttpGet("getBookedPoojaList")]
public IActionResult getBookedPoojaList(int jyotishId)
{
	try
	{
		var res = _jyotish.getBookedPoojaList(jyotishId);
		
			return Ok(new { status = 200, message = "data retirieved",data=res });

		
	}
	catch { return StatusCode(500, new { Status = 500, Message = "Internal Server Error " }); }
}
		[HttpGet("changeJyotishActiveStatus")]
		public IActionResult changeJyotishActiveStatus(int jyotishId,bool status)
		{
			try { 
			var res = _jyotish.changeJyotishActiveStatus(jyotishId, status);
			return Ok(new { status = 200, message = "successfull" });
        }
	catch { return StatusCode(500, new { status = 500, message = "Internal Server Error " }); }
        }
		[HttpGet("changeJyotishServiceStatus")]
		public IActionResult changeJyotishServiceStatus(int jyotishId,bool status)
		{
			try { 
			var res = _jyotish.changeJyotishServiceStatus(jyotishId, status);
			return Ok(new { status = 200, message = "successfull" });
        }
	catch { return StatusCode(500, new { status = 500, message = "Internal Server Error " }); }
        }
		
		[HttpGet("getJyotishDashboardRecord")]
		public IActionResult getJyotishDashboardRecord(int jyotishId)
		{
			try { 
			var res = _jyotish.getJyotishDashboardRecord(jyotishId);
			return Ok(new { status = 200, message = "data retrieved",data=res });
        }
	catch { return StatusCode(500, new { status = 500, message = "Internal Server Error " }); }
        }
		[HttpGet("GetTopTenWalletHistory")]
		public IActionResult GetTopTenWalletHistory(int jyotishId)
		{
			try { 
			var res = _jyotish.GetTopTenWalletHistory(jyotishId);
			return Ok(new { status = 200, message = "data retrieved",data=res });
        }
	catch { return StatusCode(500, new { status = 500, message = "Internal Server Error " }); }
        }



	}
}
