﻿using Microsoft.AspNetCore.Authorization;
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
        public IActionResult create(string description, DateTime timeArrived, List<string> servicesIDs)
        {
            if (servicesIDs == null || servicesIDs.Count == 0)
            {
                ModelState.AddModelError("servicesIDs", "Please select at least one service.");
            }

            if (string.IsNullOrEmpty(description))
            {
                ModelState.AddModelError("description", "The Description field is required.");
            }

            if (timeArrived < DateTime.Today)
            {
                ModelState.AddModelError("timeArrived", "Please select a valid date and make sure it is not in the past.");
            }

            if (!ModelState.IsValid)
            {
                return View();
            }
            string cCustomerString = _contx.HttpContext.Session.GetString("cCus");
            Customer cCustomer = JsonConvert.DeserializeObject<Customer>(cCustomerString);
            Appointment newAppointment = new Appointment()
            {
                description = description,
                timeArrived = timeArrived,
                customer = cCustomer
            };
            _appointmentService.createAppointment(newAppointment, servicesIDs);
            return RedirectToAction("view");
        }

        public IActionResult view()
        {
            List<Appointment> appointments = new List<Appointment>();
            string cCustomerString = _contx.HttpContext.Session.GetString("cCus");
            if(cCustomerString != null)
            {
                Customer cCustomer = JsonConvert.DeserializeObject<Customer>(cCustomerString);
                appointments = _appointmentService.getAllApppointments(cCustomer);
            }
            else
            {
                RedirectToAction("login", "Home");
            }
            return View(appointments);
        }

        public IActionResult cancel(string id)
        {
            _appointmentService.updateStatus(id, "Cancel");
            return RedirectToAction("view");
        }
    }
}
