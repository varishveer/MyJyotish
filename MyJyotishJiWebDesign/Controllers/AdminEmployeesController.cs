using Microsoft.AspNetCore.Mvc;

namespace MyJyotishGWeb.Controllers
{
    public class AdminEmployeesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
    }
}
