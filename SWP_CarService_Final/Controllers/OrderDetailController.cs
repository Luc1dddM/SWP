using Microsoft.AspNetCore.Mvc;
using SWP_CarService_Final.Models;
using SWP_CarService_Final.Services;

namespace SWP_CarService_Final.Controllers
{
    public class OrderDetailController : Controller
    {
        private readonly OrderService _orderService;
        public OrderDetailController(OrderService orderService) { 
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult view(string WorkOrderID)
        {
            WorkOrder order =  _orderService.getWorkOrderById(WorkOrderID);
            return View(order);
        }
    }
}
