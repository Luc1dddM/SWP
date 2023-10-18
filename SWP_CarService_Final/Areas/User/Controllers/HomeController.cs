using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using SWP_CarService_Final.Services;
using SWP_CarService_Final.Areas.User.Models;
using System.Data;
using System.Data.SqlClient;


namespace Areas.Controllers
{
    [Area("User")]
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _context;
        private readonly ILogger<HomeController> _logger;
        private readonly UserServices _userService;
        private readonly DBContext _dBContext;

        public HomeController(IHttpContextAccessor context, ILogger<HomeController> logger, UserServices userService, DBContext dBContext)
        {
            _logger = logger;
            _userService = userService;
            _context = context;
            _dBContext = dBContext;
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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(String userName, String password, bool rememberMe)
        {

            User user = _userService.UserLogin(userName, password);

            if (user != null && user.role_name.Trim() == "admin")
            {
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, userName),
                    new Claim(ClaimTypes.Role, "admin"),
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
                string currentAdmin = JsonConvert.SerializeObject(user);
                _context.HttpContext.Session.SetString("adminUser", currentAdmin);
                _context.HttpContext.Session.SetString("role", user.role_name.Trim());

                return RedirectToAction("Index");
            }

            else if (user != null && user.role_name.Trim() == "leader")
            {
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, userName),
                    new Claim(ClaimTypes.Role, "leader"),
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
                string currentLeader = JsonConvert.SerializeObject(user);
                _context.HttpContext.Session.SetString("leaderUser", currentLeader);
                _context.HttpContext.Session.SetString("role", user.role_name.Trim());
                return RedirectToAction("Index");
            }

            else if (user != null && user.role_name.Trim() == "member")
            {
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, userName),
                    new Claim(ClaimTypes.Role, "member"),
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
                string currentMember = JsonConvert.SerializeObject(user);
                _context.HttpContext.Session.SetString("memberUser", currentMember);
                _context.HttpContext.Session.SetString("role", user.role_name.Trim());
                return RedirectToAction("Index");
            }

            else
            {
                ViewBag.Msg = "Invalid information!!!";
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            _context.HttpContext.Session.Remove("role");
            Console.WriteLine(_context.HttpContext.Session.GetString("role"));
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}