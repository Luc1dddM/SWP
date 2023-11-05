using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SWP_CarService_Final.Areas.User.Models;
using SWP_CarService_Final.Models;
using SWP_CarService_Final.Services;

namespace SWP_CarService_Final.Controllers
{
    [Authorize]
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
            string cCustomerString = _contx.HttpContext.Session.GetString("cCus");
            Customer cCustomer = JsonConvert.DeserializeObject<Customer>(cCustomerString);
            List<WorkOrder> workOrders = _OrderService.getAllWorkOrdersOfOwner(cCustomer.user_name);
            return View(workOrders);
        }
    }
}
