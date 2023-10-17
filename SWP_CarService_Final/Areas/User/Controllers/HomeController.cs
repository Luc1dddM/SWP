using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SWP_CarService_Final.Services;
using System.Security.Claims;
using SWP_CarService_Final.Areas.User.Models;

namespace Areas
{
    [Area("User")]
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _context;
        private readonly ILogger<HomeController> _logger;
        private readonly UserServices _userService;

        public HomeController(IHttpContextAccessor context, ILogger<HomeController> logger, UserServices userService)
        {
            _logger = logger;
            _userService = userService;
            _context = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }
            return View(); //note: chua add view cua admin muon do cua user 
        }

        [HttpPost]
        public async Task<IActionResult> login(String userName, String password, bool rememberMe)
        {
            User user = _userService.UserLogin(userName, password);
            if (user != null)
            {
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
                string currentCustomer = JsonConvert.SerializeObject(user);
                _context.HttpContext.Session.SetString("user", currentCustomer);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Msg = "ko hop le";
            }
            return View();
        }

        public async Task<IActionResult> logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("login");
        }
    }
}