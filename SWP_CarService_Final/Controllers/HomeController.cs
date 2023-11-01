using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SWP_CarService_Final.Models;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using SWP_CarService_Final.Services;
using Newtonsoft.Json;
using System.Security.Principal;
using SWP_CarService_Final.Areas.User.Models;

namespace SWP_CarService_Final.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _contx;
        private readonly ILogger<HomeController> _logger;
        private readonly UserServices _userService;
        private readonly CustomerAccountService _createAccount;


        public HomeController(ILogger<HomeController> logger, UserServices userService, IHttpContextAccessor contx, CustomerAccountService createAccount)
        {
            _logger = logger;
            _userService = userService;
            _contx = contx;
            _createAccount = createAccount;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> login(String userName, String password, bool rememberMe)
        {
            Customer cUser = _userService.CustomerLogin(userName, password);
            if (cUser != null)
            {

                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, userName),
                    new Claim(ClaimTypes.Role, "customer"),
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                        CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = false
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), properties);


                //create session for current user
                string currentCustomer = JsonConvert.SerializeObject(cUser);
                _contx.HttpContext.Session.SetString("cCus", currentCustomer);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Msg = "Invalid information!!!";
            }
            return View();
        }

        public async Task<IActionResult> logout()
        {
            _contx.HttpContext.Session.Remove("cCus");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("login");
        }

        public IActionResult ResetPassword(string Username)
        {
            ViewBag.username = Username;
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(string Username, string Password)
        {
            try
            {
                Customer customer = new Customer()
                {
                    user_name = Username,
                    password = Password
                };
            _userService.ResetCustomerPassword(Username, Password);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            return Redirect("login");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult register(string user_name, string password, string fullName, string email, string phone_number, bool account_status)
        {
            Customer newAccount = new Customer()
            {
                user_name = user_name,
                password = password,
                fullName = fullName,
                email = email,
                phone_number = phone_number,
                account_status = true,
                /*img = null*/
            };
            _createAccount.CreateCustomer(newAccount);
            string currentCustomer = JsonConvert.SerializeObject(newAccount);
            _contx.HttpContext.Session.SetString("cCus", currentCustomer);

            return View("Index");
        }
    }
}