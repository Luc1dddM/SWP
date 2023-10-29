using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Newtonsoft.Json;
using SWP_CarService_Final.Areas.User.Models;
using SWP_CarService_Final.Models;
using SWP_CarService_Final.Services;
using System.Diagnostics.Metrics;

namespace Areas
{
    [Area("User")]
    public class TaskDetailController : Controller
    {
        private readonly TaskDetailService _taskDetailService;
        private readonly IHttpContextAccessor _context;


        public TaskDetailController(TaskDetailService taskDetailService, IHttpContextAccessor context)
        {
            _taskDetailService = taskDetailService;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult view()
        {
            string cUserString = _context.HttpContext.Session.GetString("cUser");
            User cUser = JsonConvert.DeserializeObject<User>(cUserString);
            List<TaskDetail> listDetails = _taskDetailService.GetTaskDetailsOfMember(cUser.UserName);
            return View(listDetails);
        }

        public IActionResult ViewListRequest()
        {
            string cUserString = _context.HttpContext.Session.GetString("cUser");
            User cUser = JsonConvert.DeserializeObject<User>(cUserString);
            List<TaskDetail> listDetails = _taskDetailService.getRequestCompleteList(cUser.UserName);
            return View(listDetails);
        }

        public IActionResult RequestComplete(string wodID)
        {
            TaskDetail newTaskDetail = _taskDetailService.getTaskDetailByID(wodID);
            newTaskDetail.status = "Request Complete";
            _taskDetailService.updateTaskDetail(newTaskDetail);
            Console.WriteLine(newTaskDetail.WorkOrder.WorkOrderID);
            return Redirect("/user/TaskDetail/view");
        }

        public IActionResult Edit(string wodID)
        {
            TaskDetail detail = _taskDetailService.getTaskDetailByID(wodID);
            return View(detail);
        }

        [HttpPost]
        public IActionResult Edit(string id, int quantity, string member)
        {
            TaskDetail newTaskDetail = _taskDetailService.getTaskDetailByID(id);
            newTaskDetail.quantity = quantity;
            newTaskDetail.userName = member;
            _taskDetailService.updateTaskDetail(newTaskDetail);
            Console.WriteLine(newTaskDetail.WorkOrder.WorkOrderID);
            return RedirectToAction("view", "OrderDetail", new { WorkOrderID = newTaskDetail.WorkOrder.WorkOrderID });
        }

        public IActionResult ResponseRequest(string wodID, string Response)
        {
            TaskDetail newTaskDetail = _taskDetailService.getTaskDetailByID(wodID);
            newTaskDetail.status = Response;
            _taskDetailService.updateTaskDetail(newTaskDetail);
            return Redirect("/user/TaskDetail/view");
        }
    }
}
