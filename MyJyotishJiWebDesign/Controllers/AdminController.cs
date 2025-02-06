
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


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
        public IActionResult ExistingPrice() { return View(); }
        public IActionResult Documents() { return View(); }
        public IActionResult JyotishDocument() { return View(); }
        public IActionResult JyotishProfile() { return View(); }
        public IActionResult InterviewList() { return View(); }
        public IActionResult Specialization() { return View(); }
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
        public IActionResult CreatePoojaList() { return View(); }
		public ActionResult CreatePooja() { return View(); }
		public ActionResult PoojaList() { return View(); }
		public ActionResult JyotishWalletManagement() { return View(); }
		public ActionResult EmployeeList() { return View(); }
		public ActionResult Advertisement() { return View(); }
		public ActionResult AdvertisementRequest() { return View(); }
		public ActionResult MailService() { return View(); }

        public async Task<ActionResult> PrivacyPolicy()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage message = await client.GetAsync("https://api.myjyotishg.in/api/admin/getPrivacyPolicy");
            dynamic res = null;

            if (message.IsSuccessStatusCode)
            {
                res = await message.Content.ReadAsStringAsync();
                res = JsonConvert.DeserializeObject<dynamic>(res);
            }

            return View(res);
        }

	}
}
