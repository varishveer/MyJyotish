using Microsoft.AspNetCore.Mvc;



namespace MyJyotishJiWebDesign.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

       
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult SignUpUser() {return View(); }
        public IActionResult OtpVerification() { return View(); }
        public IActionResult Login() { return View();}
        public IActionResult Index() { return View(); }
        [HttpPost]
        public IActionResult Search(string? filterValue,string? searchInput) {
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
        public IActionResult FreeKundli() { return View(); }
        public IActionResult Compatibility() { return View(); }
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
        public ActionResult BookPooja() { return View(); }

        public IActionResult WalletPaymentLayout(int amount,int jyotishId,string message,string paymentby) { return PartialView("_WalletPartialView", new { amount = amount,jyotishId=jyotishId,message=message, paymentby = paymentby }); }
    }
}
