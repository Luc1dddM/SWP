using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SWP_CarService_Final.Areas.User.Models;
using SWP_CarService_Final.Models;
using SWP_CarService_Final.Services;

namespace Areas
{
    [Area("User")]
    public class OrderController : Controller
    {
        private readonly AppointmentService _appointmentService;
        private readonly OrderService _OrderService;
        private readonly IHttpContextAccessor _contx;


        public OrderController(AppointmentService appointmentService, OrderService orderService, IHttpContextAccessor contx)
        {
            _appointmentService = appointmentService;
            _OrderService = orderService;
            _contx = contx;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult view()
        {
            string cUserString = _contx.HttpContext.Session.GetString("cUser");
            List<WorkOrder> orders;
            User cUser = JsonConvert.DeserializeObject<User>(cUserString);
            if (cUser.role_name.Trim().Equals("admin"))
            {
                orders = _OrderService.getAllWorkOrders();
            }
            else
            {
                orders = _OrderService.getAllWorkOrdersCreatedByUser(cUser.UserName);
            }
            return View(orders);
        }

        public IActionResult create(string ApmID)
        {
            string cUserString = _contx.HttpContext.Session.GetString("cUser");
            User cUser = JsonConvert.DeserializeObject<User>(cUserString);
            Appointment appointment = _appointmentService.getAppointmentByID(ApmID);
            _OrderService.createWorkOrderByAPM(appointment, cUser.UserName);
            return Redirect("/user/order/view");
        }
    }
}
