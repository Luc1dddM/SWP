using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWP_CarService_Final.Models;
using SWP_CarService_Final.Services;

namespace SWP_CarService_Final.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "admin, leader, member")]
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

        [Authorize(Roles = "admin")]
        public IActionResult CreateTeam()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult CreateTeam(string teamName)
        {
            Team newTeam = new Team()
            {
                team_name = teamName.Trim(),
            };
            _teamService.CreateTeam(newTeam);
            return Redirect("ViewAllTeam");
        }

        [Authorize(Roles = "admin")]
        public IActionResult DeleteTeam(string team_id)
        {
            _teamService.DeleteTeam(team_id);
            return Redirect("ViewAllTeam");
        }

        [Authorize(Roles = "admin")]
        public IActionResult EditTeam(string team_id)
        {
            ViewBag.teamId = team_id;
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult EditTeam(string teamId, string teamName)
        {
            try
            {
                Team team = new Team()
                {
                    team_id = teamId,
                    team_name = teamName,
                };
                _teamService.Edit_Team(team);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            return Redirect("ViewAllteam");
        }
    }
}
