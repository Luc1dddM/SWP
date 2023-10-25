using Microsoft.AspNetCore.Mvc;

namespace SWP_CarService_Final.Areas.User.Controllers
{
    public class PartController : Controller
    {
        public IActionResult ListOfComponent()
        {
            return View();
        }
    }
}
