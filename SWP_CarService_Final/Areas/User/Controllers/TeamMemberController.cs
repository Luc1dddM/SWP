using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SWP_CarService_Final.Areas.User.Models;
using SWP_CarService_Final.Models;
using SWP_CarService_Final.Services;
using User = SWP_CarService_Final.Areas.User.Models.User;

namespace Areas
{
    [Area("User")]
    [Authorize(Roles = "admin")]
    public class TeamMemberController : Controller
    {
        private readonly UserAccountServices _userAccount;
        private readonly TeamMemberService _teamMemberService = new TeamMemberService();

        public IActionResult ListOfTeamMembers()
        {
            return View();
        }
        public TeamMemberController(UserAccountServices userAccount)
        {
            _userAccount = userAccount;
        }

        public IActionResult CreateAccount()
        {
            return View();
        }

        public IActionResult EditAccount(string UserName)
        {
            ViewBag.username = UserName;

            return View();
        }

        [HttpPost]
        public IActionResult CreateAccount(string username, string fullname, string password, string email, string phonenumber, string status, string roleId)
        {
            try
            {
                User user = new User()
                {
                    UserName = username,
                    User_fullname = fullname,
                    password = password,
                    email = email,
                    phone_number = phonenumber,
                    account_status = status == "active" ? true : false,
                    created = DateTime.Now,
                };
                User existingUser = _userAccount.getUserByUserName(username);
                if (existingUser != null)
                {
                    TempData["ErrorMsg"] = "Username already exists";
                    TempData["UserExist"] = existingUser.UserName;
                    TempData["InputValues"] = user; // Save input values in TempData
                    TempData["InputRoleId"] = roleId;
                    return View();
                }
                _userAccount.createAccount(user, roleId);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Redirect("ListOfTeamMembers");
        }

        [HttpPost]
        public IActionResult EditAccount(string username, string fullname, string password, string email, string phonenumber, string status, string roleId)
        {
            try
            {
                User user = new User()
                {
                    UserName = username,
                    User_fullname = fullname,
                    password = password,
                    email = email,
                    phone_number = phonenumber,
                    account_status = status == "active" ? true : false,
                    created = DateTime.Now,
                };
                _userAccount.editAccount(user, roleId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Redirect("ListOfTeamMembers");
        }

        public IActionResult DeleteAccount(string UserName)
        {
            _userAccount.deleteAccount(UserName);
            return Redirect("ListOfTeamMembers");
        }

        /*---------------Begin CRUD Team Member _ Tu Quoc Phat---------------*/

        public IActionResult AddMember(string teamId)
        {
            ViewBag.teamId = teamId;
            return View();
        }

        [HttpPost]
        public IActionResult AddMember(string teamId, List<string> username)
        {
            bool leaderExist = _teamMemberService.CheckLeaderExist(teamId);
            var user = _teamMemberService.CheckIfListMemberExistLeader(username);
            if (leaderExist && user)
            {
                TempData["msg"] = "This Team Already Has A Leader";
                return Redirect("/user/TeamMember/AddMember?teamId=" + teamId);
            }
            else
            {
                _teamMemberService.AddTeamMember(teamId, username);
                return Redirect("/user/team/ViewAllTeam");
            }

        }


        public IActionResult EditTeamMember(string Username)
        {
            ViewBag.user_name = Username;
            return View();
        }

        [HttpPost]
        public IActionResult EditTeamMember(string username, string role_id, string team_id)
        {
            User user = null;
            try
            {
                bool leaderExist = _teamMemberService.CheckLeaderExist(team_id);
                var userRole = _teamMemberService.GetRoleNameByRoleID(role_id);
                if (leaderExist && userRole.role_name.Equals("leader"))
                {
                    TempData["msg"] = "This Team Already Has A Leader";
                    return Redirect("/user/TeamMember/EditTeamMember?Username=" + username);
                }
                else
                {
                    user = new User()
                    {
                        UserName = username,
                    };

                    _teamMemberService.EditTeamMemberRoleByUserName(username, role_id);
                    _teamMemberService.EditMemberTeam(username, team_id);
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

            return Redirect("/user/team/ViewAllTeam");
        }


        public IActionResult DeteleMemberFromTeam(string Username)
        {
            _teamMemberService.DeteleMemberFromTeam(Username);
            return Redirect("/user/team/ViewAllTeam");
        }

        /*---------------End CRUD Team Member _ Tu Quoc Phat---------------*/



    }
}
