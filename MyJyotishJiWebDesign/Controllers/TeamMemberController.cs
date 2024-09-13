using Microsoft.AspNetCore.Mvc;

namespace MyJyotishJiWebDesign.Controllers
{
    public class TeamMemberController : Controller
    {
      
        public ActionResult Login() { return View(); }
        public ActionResult Profile() { return View(); }
        public ActionResult Dashboard() { return View(); }
        public ActionResult Appointments() { return View(); }
        public ActionResult CustomerSupport() { return View(); }
        public ActionResult AddAppointment() { return View(); }
        public ActionResult EditAppointment() { return View(); }
        public ActionResult UpcomingAppointment() { return View(); }
        public ActionResult EditProfile() { return View(); }
        public ActionResult EditUpcomingAppointment() { return View(); }
    }
}
