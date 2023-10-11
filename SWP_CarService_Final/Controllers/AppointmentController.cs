using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SWP_CarService_Final.Models;
using SWP_CarService_Final.Services;

namespace SWP_CarService_Final.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly AppointmentService _appointmentService;
        private readonly IHttpContextAccessor _contx;


        public AppointmentController(AppointmentService appointmentService, IHttpContextAccessor contx)
        {
            _appointmentService = appointmentService;
            _contx = contx;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult create()
        {

            
            return View();
        }

        [HttpPost]
        public IActionResult create(string vehicleType, string description, DateTime timeArrived)
        {
            string cCustomerString = _contx.HttpContext.Session.GetString("cCus");
            Customer cCustomer = JsonConvert.DeserializeObject<Customer>(cCustomerString);
            Appointment newAppointment = new Appointment()
            {
                description = description,
                vehicalType = vehicleType,
                timeArrived = timeArrived,
                customer = cCustomer
            };
            _appointmentService.createAppointment(newAppointment);
            
            return RedirectToAction("index", "Home");
        }

        public IActionResult view()
        {
            return View();
        }
    }
}
