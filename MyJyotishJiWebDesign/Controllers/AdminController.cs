
using Microsoft.AspNetCore.Mvc;


namespace MyJyotishJiWebDesign.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Login() { return View(); }
        public IActionResult Dashboard() { return View();}
        public IActionResult Appointments() { return View(); }
        public IActionResult PendingJyotish() { return View(); }
        public IActionResult JyotishList() { return View(); }
        public IActionResult UserList() { return View(); }
        public IActionResult PoojaCategoryList() { return View(); }
        public IActionResult ExpertiseList() { return View(); }
        public IActionResult AdminProfile() { return View(); }
        public IActionResult SlotList() { return View(); }
        public IActionResult Subscription() { return View(); }
        public IActionResult Services() { return View(); }
        public IActionResult Features() { return View(); }
        public IActionResult Documents() { return View(); }
        public IActionResult JyotishDocument() { return View(); }
        public IActionResult JyotishProfile() { return View(); }
        public IActionResult InterviewList() { return View(); }
        public IActionResult Specialization() { return View(); }
        public IActionResult CreateCountryCode() { return View(); }
        public IActionResult RedeamCode() { return View(); }
        public IActionResult Department() { return View(); }
        public IActionResult Levels() { return View(); }
        public IActionResult EmployeeAccessPages() { return View(); }
        public IActionResult Employee() { return View(); }
        public IActionResult Feedback() { return View();  }
        public IActionResult PendingFeedback() { return View();  }
        public IActionResult ApproveJyotishInterview() { return View();  }
        public IActionResult AutherizePages() { return View();  }
		public IActionResult ApproveRedeamCode()
		{return View();}
        public IActionResult Banners() { return View(); }
        public IActionResult CreateRedeemCode() { return View(); }


	}
}
