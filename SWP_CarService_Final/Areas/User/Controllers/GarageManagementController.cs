using Microsoft.AspNetCore.Mvc;

namespace SWP_CarService_Final.Areas.User.Controllers
{
    [Area("User")]
    public class GarageManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
