using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SWP_CarService_Final.Areas.User.Models;
using SWP_CarService_Final.Models;
using SWP_CarService_Final.Services;

namespace Areas
{
    [Area("User")]
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
            return view();
        }

        public IActionResult view()
        {
            string cUserString = _contx.HttpContext.Session.GetString("cUser");
            User cUser = JsonConvert.DeserializeObject<User>(cUserString);
            if(cUser.role_name.Trim().Equals("admin"))
            {
                ViewBag.role = "admin";
            }
            List<Appointment> appointments = _appointmentService.getAllApppointments();
            return View(appointments);
        }
    }
}
