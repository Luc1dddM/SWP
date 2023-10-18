using Microsoft.AspNetCore.Mvc;
using SWP_CarService_Final.Services;
using User = SWP_CarService_Final.Areas.User.Models.User;

namespace Areas
{
    [Area("User")]
    public class TeamMemberController : Controller
    {
        UserAccount userAccount = new UserAccount();

        public IActionResult ListOfTeamMembers()
        {
            return View();
        }


        public IActionResult CreateAccount()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateAccount(string username, string fullname, string password, string email, string phonenumber, string status)
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
                userAccount.createAccount(user);
            }catch (Exception ex) 
            { 
                throw new Exception(ex.Message); 
            }
            return Redirect("ListOfTeamMembers");
        }

    }
}
