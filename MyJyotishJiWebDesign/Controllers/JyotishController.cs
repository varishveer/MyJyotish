using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyJyotishJiWebDesign.Controllers
{
    public class JyotishController : Controller
    {
        
        public ActionResult SignUp() { return View();}
        public ActionResult OtpVerification() { return View(); }
        public ActionResult Register() { return View(); }
        public ActionResult Profile() { return View(); }
        public ActionResult UpdateProfile() { return View(); }
        public ActionResult Dashboard() { return View(); }
        public ActionResult CustomerSupport() { return View(); }
        public ActionResult UpcomingAppointment() { return View(); }
        public ActionResult AppointmentHistory() { return View(); }
        public ActionResult AddAppointment() { return View(); }
        public ActionResult AddTeamMember() { return View(); }
        public ActionResult TeamMembers() { return View(); }
        public ActionResult EditAppointment() { return View(); }
        public ActionResult EditTeamMember() { return View(); }
        public ActionResult EditProfile() { return View(); }
        public ActionResult EditUpcomingAppointment() { return View(); }
        public ActionResult Login() { return View(); }

        public ActionResult Payment() { return View(); }
        public ActionResult Subscription() { return View(); }
        public ActionResult Gallery() { return View(); }
        public ActionResult Video() { return View(); }

        public ActionResult AppointmentSlot() { return View(); }
        public ActionResult JyotishSolution() { return View(); }
        public ActionResult ProblemSolution() { return View(); }
        public ActionResult ProblemSolutionDetail() { return View(); }
        public ActionResult Wallets() { return View(); }
        public ActionResult CreatePooja() { return View(); }
        public IActionResult WalletPaymentLayout(int amount, string message,string paymentby) { return PartialView("_WalletPaymentJyotish", new { amount = amount, message = message,paymentby=paymentby }); }

    }
}
