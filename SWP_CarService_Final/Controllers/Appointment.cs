using Microsoft.AspNetCore.Mvc;

namespace SWP_CarService_Final.Controllers
{
    public class Appointment : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult create()
        {
            return View();
        }
    }
}
