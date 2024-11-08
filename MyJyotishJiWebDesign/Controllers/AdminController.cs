
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
        public IActionResult Features() { return View(); }
        public IActionResult Documents() { return View(); }
        public IActionResult JyotishDocument() { return View(); }
        public IActionResult JyotishProfile() { return View(); }

        public IActionResult InterviewList() { return View(); }




    }
}
