using Microsoft.AspNetCore.Mvc;

namespace MyJyotishGWeb.Controllers
{
    public class PendingJyotishController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
       
        public IActionResult Login() { return View(); }
        public IActionResult Documents() { return View(); }
        public IActionResult UploadDocument() { return View(); }
        public IActionResult Profile() { return View(); }
        public IActionResult UpdateProfile() { return View(); }
        public IActionResult ForgotPasswordRequest() { return View(); }
        public IActionResult ForgotPasswordOtp() { return View(); }

    }
}
