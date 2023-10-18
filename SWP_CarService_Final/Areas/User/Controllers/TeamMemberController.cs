﻿using Microsoft.AspNetCore.Mvc;
using SWP_CarService_Final.Services;
using User = SWP_CarService_Final.Areas.User.Models.User;

namespace Areas
{
    [Area("User")]
    public class TeamMemberController : Controller
    {
        private readonly UserAccountServices _userAccount;

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
                _userAccount.createAccount(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Redirect("ListOfTeamMembers");
        }

        [HttpPost]
        public IActionResult EditAccount(string username, string fullname, string password, string email, string phonenumber, string status)
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
                _userAccount.editAccount(user);
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

    }
}