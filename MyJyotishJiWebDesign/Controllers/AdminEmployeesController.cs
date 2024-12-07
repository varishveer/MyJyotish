using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyJyotishGWeb.Controllers
{
    public class AdminEmployeesController : Controller
    {

    public IActionResult Login()
        {
           return View();
        }

        [HttpGet]
        public IActionResult SetCookieAfterLogin()
        {
            
               Response.Cookies.Append("role", "employee", new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddDays(7),
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });
            
            return Json(true);
        }

        [HttpGet]
        public IActionResult logoutEmployee()
        {
            Response.Cookies.Delete("role");

            return Json(true);
        }

        public IActionResult Index()
        {
               Response.Cookies.Append("role", "employee", new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddDays(7),
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });
            

            return View();
        } 
       
    }
}
