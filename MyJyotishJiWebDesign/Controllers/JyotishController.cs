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
        public ActionResult Dashboard() { return View(); }
        public ActionResult Appointments() { return View(); }
        public ActionResult CustomerSupport() { return View(); }
        public ActionResult UpcomingAppointment() { return View(); }
        public ActionResult AddAppointment() { return View(); }
        public ActionResult AddTeamMember() { return View(); }
        public ActionResult TeamMembers() { return View(); }
        public ActionResult EditAppointment() { return View(); }
        public ActionResult EditTeamMember() { return View(); }
        public ActionResult EditProfile() { return View(); }
        public ActionResult EditUpcomingAppointment() { return View(); }
        public ActionResult Login() { return View(); }

        public ActionResult Payment() { return View(); }


    }
}
