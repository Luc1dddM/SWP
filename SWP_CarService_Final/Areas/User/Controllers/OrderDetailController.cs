using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SWP_CarService_Final.Areas.User.Models;
using SWP_CarService_Final.Models;
using SWP_CarService_Final.Services;
using System.Security.Claims;

namespace Areas
{
    [Area("User")]
    [Authorize]
    public class OrderDetailController : Controller
    {
        private readonly OrderService _orderService;
        private readonly IHttpContextAccessor _context;
        public OrderDetailController(OrderService orderService, IHttpContextAccessor context)
        {
            _orderService = orderService;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult view(string WorkOrderID)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var roleClaim = identity.FindFirst(ClaimTypes.Role);
            if (roleClaim != null)
            {
                var role = roleClaim.Value;
                // Use the role as needed
                Console.WriteLine(role);

            }
            string cUserString = _context.HttpContext.Session.GetString("cUser");
            User cUser = JsonConvert.DeserializeObject<User>(cUserString);
            WorkOrder order = null;
            if (cUser.role_name.Trim() == "member")
            {
                order = _orderService.getWorkOrderById(WorkOrderID, cUser);
            }
            else
            {
                order = _orderService.getWorkOrderById(WorkOrderID);
            }
            return View(order);
        }
    }
}
