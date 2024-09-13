using Microsoft.AspNetCore.Mvc;

namespace MyJyotishGWeb.Controllers
{
    public class ServicesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Astrology()
        {
            return View();
        }
        public IActionResult Palmistry()
        { return View(); }
        public IActionResult Tarot()
        { return View(); }
        public IActionResult Pooja()
        { return View(); }
        public IActionResult Vastu()
        { return View(); }
        public IActionResult Kundali()
        { return View(); }
        public IActionResult KundaliMatching()
        { return View(); }
       
        public IActionResult Horoscope()
        { return View(); }
    }
}
