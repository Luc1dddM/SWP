using Microsoft.AspNetCore.Mvc;
using SWP_CarService_Final.Models;
using SWP_CarService_Final.Services;

namespace SWP_CarService_Final.Areas.User.Controllers
{
    [Area("User")]
    public class AppointmentController : Controller
    {
        private readonly AppointmentService _appointmentService;

        public AppointmentController(AppointmentService appointmentService, IHttpContextAccessor contx)
        {
            _appointmentService = appointmentService;
        }
        public IActionResult Index()
        {
            return view();
        }

        public IActionResult view()
        {
            List<Appointment> appointments = _appointmentService.getAllApppointments();
            return View(appointments);
        }
    }
}
