using Microsoft.AspNetCore.Mvc;

namespace SWP_CarService_Final.Controllers
{
    public class TaskController : Controller
    {
        public IActionResult ListOfServices()
        {
            return View();
        }
        public IActionResult AddService()
        {
            return View();
        }
        public IActionResult EditService()
        {
            return View();
        }
    }
}
