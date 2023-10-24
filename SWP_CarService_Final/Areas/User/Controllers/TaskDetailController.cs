using Microsoft.AspNetCore.Mvc;
using SWP_CarService_Final.Models;
using SWP_CarService_Final.Services;

namespace SWP_CarService_Final.Areas.User.Controllers
{
    [Area("User")]
    public class TaskDetailController : Controller
    {
        private readonly TaskDetailService _taskDetailService;

        public TaskDetailController(TaskDetailService taskDetailService)
        {
            _taskDetailService = taskDetailService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Edit(string wodID)
        {
            TaskDetail detail = _taskDetailService.getTaskDetailByID(wodID);
            return View(detail);
        }
    }
}
