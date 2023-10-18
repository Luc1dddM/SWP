using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SWP_CarService_Final.Areas.User.Controllers
{
    [Authorize(Roles ="admin")]
    [Area("User")]
    public class GarageManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
