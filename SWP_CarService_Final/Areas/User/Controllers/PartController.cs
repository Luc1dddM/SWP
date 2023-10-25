using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWP_CarService_Final.Services;
using SWP_CarService_Final.Models;

namespace SWP_CarService_Final.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "admin")]
    public class PartController : Controller
    {
        private readonly PartService _partService;


        public PartController(PartService partService)
        {
            _partService = partService;
        }

        public IActionResult ListOfComponent(int pageNumber)
        {
            List<Part> partList = _partService.GetAllPart(pageNumber);
            Console.WriteLine(_partService.GetNumberOfPage());
            Console.WriteLine(_partService.GetNumberOfPage().GetType);

            return View(partList);
        }
    }
}
