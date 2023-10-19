using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWP_CarService_Final.Models;
using SWP_CarService_Final.Services;

namespace SWP_CarService_Final.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "admin")]
    public class TeamController : Controller
    {
        private readonly IHttpContextAccessor _contx;
        private readonly TeamService _teamService;

        public TeamController(IHttpContextAccessor contx ,TeamService teamService)
        {
            _contx = contx;
            _teamService = teamService;
        }

        public IActionResult ViewAllTeam()
        {
            return View();
        }

        public IActionResult CreateTeam()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateTeam(string teamName)
        {
            Team newTeam = new Team()
            {
                team_name = teamName,
            };
            _teamService.CreateTeam(newTeam);
            return Redirect("ViewAllTeam");
        }


    }
}
