using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SWP_CarService_Final.Models;
using SWP_CarService_Final.Services;
using SWP_CarService_Final.Areas.User.Models;


namespace Areas
{
    [Area("User")]
    public class UserProfileController : Controller
    {
      /*  readonly string rootFolder = @"D:\SE1702\SWP code\SWP_CarService_Final\wwwroot\img";*/
        private readonly UserProfileServices _userProfileServices;
        private readonly IHttpContextAccessor _contx;

        public UserProfileController(UserProfileServices userProfileServices, IHttpContextAccessor contx)
        {
            _userProfileServices = userProfileServices;
            _contx = contx;
        }

        public IActionResult EditProfileUser() 
        {
            string cCustomerString = _contx.HttpContext.Session.GetString("cUser");
            User cCustomer = JsonConvert.DeserializeObject<User>(cCustomerString);
            /*ViewBag.username = userName;*/
            /* User cCustomer = new User();*/
            return View(cCustomer); 
        }

        [HttpPost]
        public IActionResult EditProfileUser(string UserName, string User_fullname, string phone_number, string email, string account_status)
        {
            try 
            {
                string cCustomerString = _contx.HttpContext.Session.GetString("cUser");
                User cCustomer = JsonConvert.DeserializeObject<User>(cCustomerString);
                User user = new User()
                {
                    UserName = cCustomer.UserName,
                    User_fullname = User_fullname,
                    email = email,  
                    phone_number = phone_number,
                    account_status = (account_status == "active")
                };
                _userProfileServices.editUserProfile(user);

                string currentCustomer = JsonConvert.SerializeObject(user);
                _contx.HttpContext.Session.SetString("cUser", currentCustomer);


            }
             catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
