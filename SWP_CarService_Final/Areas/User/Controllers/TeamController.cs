using Microsoft.AspNetCore.Mvc;
using SWP_CarService_Final.Models;
using SWP_CarService_Final.Services;

namespace SWP_CarService_Final.Areas.User.Controllers
{
    [Area("User")]
    public class TeamController : Controller
    {
        /*private readonly IHttpContextAccessor _contx;*/
        /*private readonly TeamService _teamService;*/

        TeamService TeamService = new TeamService();

        /*public TeamController(TeamService teamService)
        {
            _teamService = teamService;
        }*/

        public IActionResult Index()
        {
            return View();
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
            TeamService.CreateTeam(newTeam);
            return Redirect("ViewAllTeam");
        }


    }
}
