using Microsoft.AspNetCore.Mvc;

namespace SWP_CarService_Final.Controllers
{
    public class GarageManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
