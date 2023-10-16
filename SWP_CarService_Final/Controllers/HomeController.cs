using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SWP_CarService_Final.Models;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using SWP_CarService_Final.Services;
using Newtonsoft.Json;

namespace SWP_CarService_Final.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _contx;
        private readonly ILogger<HomeController> _logger;
        private readonly UserServices _userService;


        public HomeController(ILogger<HomeController> logger, UserServices userService, IHttpContextAccessor contx)
        {
            _logger = logger;
            _userService = userService;
            _contx = contx;
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
            Customer cUser = _userService.login(userName, password);
            if (cUser != null)
            {
                string currentCustomer = JsonConvert.SerializeObject(cUser);
                _contx.HttpContext.Session.SetString("cCus", currentCustomer);


                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, userName),
                    new Claim(ClaimTypes.Role, "admin"),
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                        CookieAuthenticationDefaults.AuthenticationScheme
                    );

                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = false
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), properties);


                //create session for current user
                
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Msg = "del dc roi";
            }
            return View();
        }

        public async Task<IActionResult> logout()
        {
            _contx.HttpContext.Session.Remove("cCus");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("login");
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
    }
}