using Microsoft.AspNetCore.Mvc;
using SWP_CarService_Final.Models;
using SWP_CarService_Final.Services;

namespace SWP_CarService_Final.Areas.User.Controllers
{
    [Area("User")]
    public class TeamController : Controller
    {
        private readonly IHttpContextAccessor _contx;
        private readonly TeamService _teamService;

        public TeamController(IHttpContextAccessor contx, TeamService teamService)
        {
            _contx = contx;
            _teamService = teamService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ViewAllTeam()
        {
            /*List<Team> allTeam = new List<Team>();
            if (!allTeam.Any())
            {
                _teamService.GetAllTeam();
            }*/

            return View();
        }


        public IActionResult CreateTeam()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateTeam(string teamID)
        {
            Team newTeam = new Team()
            {
                team_id = teamID,
            };
            _teamService.CreateTeam(newTeam);
            return RedirectToAction("Index", "Home");
        }
    }
}
