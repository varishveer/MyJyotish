using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;



namespace MyJyotishJiWebDesign.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

       
        public async Task<IActionResult> Privacy()
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
        public IActionResult SignUpUser() {return View(); }
        public IActionResult OtpVerification() { return View(); }
        public IActionResult Login() { return View();}
        public async Task<IActionResult> Index() {
                dynamic res = null;
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://api.myjyotishg.in/api/user/GetTopOneAdvertisementBanner");
                if (response.IsSuccessStatusCode)
                {
                    res = await response.Content.ReadAsStringAsync();
                    res = JObject.Parse(res);
                    res = res.data.result;
                    if (res.Count == 0)
                    {
                        res = null;
                    }
                }
            }catch(Exception ex)
            {
                res = null;
            }
            return View(res);
        }

        public IActionResult Search(string filterValue,string searchInput) {
            ViewBag.filterValue = filterValue;
            ViewBag.searchInput = searchInput;
            return View(); 
        }

        public IActionResult Register() {return View(); }
        public IActionResult HomePage() { return View(); }  
        public IActionResult Talk() { return View(); }
        public IActionResult Chat() { return View(); }
        public IActionResult BookAPooja() { return View(); }
        public IActionResult Mall() { return View(); }
        public IActionResult Blog() { return View(); }
        public IActionResult MallProduct() { return View(); }
        public IActionResult ProductDetails() { return View(); }
        public IActionResult AddNewAddress() { return View(); }
        public IActionResult Payment() { return View(); }
        public IActionResult RazorPay() { return View(); }
        public IActionResult MallPooja() { return View(); }
        public IActionResult PoojaDetails() { return View(); }
        public IActionResult Profile() { return View(); }
        public IActionResult EditProfile() { return View(); }
        public IActionResult Notification() {return View(); }
        public IActionResult Wallet() { return View(); }
        public IActionResult Orders() { return View(); }
        public IActionResult CustomerSupport() { return View(); }
        public IActionResult BookAAppointment() { return View(); }
        public IActionResult AppointmentForm() { return View(); }
        public IActionResult JyotishProfile() { return View(); }
        public IActionResult TodayHoroscope() { return View(); }
        public IActionResult Panchang() { return View(); }
        public IActionResult FreeKundaliReport() { return View(); }
        public IActionResult IndianCalendarFestivals() { return View(); }
        public IActionResult BlogDetails() { return View(); }
        public IActionResult SignUp() { return View(); }
        public IActionResult ResetPassword() { return View(); }
        public IActionResult Appointments() { return View(); }
        public IActionResult AppointmentHistory() { return View(); }
        public IActionResult ProblemSolution() { return View(); }
        public IActionResult ProblemSolutionDetails() { return View(); }
        public IActionResult KundaliMatching() { return View(); }
        public IActionResult KundaliMatchingReport() { return View(); }
        public IActionResult BookPooja() { return View(); }
        public IActionResult FreeKundali() { return View(); }
        public IActionResult Astrology() { return View(); }
        public IActionResult Palmistry() { return View(); }
        public IActionResult Tarot() { return View(); }
        public IActionResult Vastu() { return View(); }
        public IActionResult Compatibility() { return View(); }
        public IActionResult ServiceRecord() { return View(); }
        public async Task<IActionResult> TermCondition() {
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
        public IActionResult WalletPaymentLayout(int amount,int jyotishId,string message,string paymentby) {
            return PartialView("_WalletPartialView", new { amount = amount,jyotishId=jyotishId,message=message, paymentby = paymentby });
        }

        // Error handler for specific HTTP status codes
        public IActionResult Error(int? statusCode)
        {
            if (statusCode == 404)
            {
                ViewData["ErrorMessage"] = "Oops! The page you are looking for could not be found.";
            }
            else if (statusCode == 500)
            {
                ViewData["ErrorMessage"] = "Oops! Something went wrong on our end. Please try again later.";
            }
            else
            {
                ViewData["ErrorMessage"] = "An unknown error occurred.";
            }

            return View("Error");
        }

        public IActionResult ContactUs()
        {
            return View();
        }
        public IActionResult AboutUs()
        {
            return View();
        } 
        public IActionResult FrequentlyAskQuestion()
        {
            return View();
        }
        public IActionResult MoreVideos()
        {
            return View();
        }

    }
}
